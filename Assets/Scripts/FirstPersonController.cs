using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField] private bool m_IsWalking;
		[SerializeField] private int m_NbrAllumettes = 10;
		[SerializeField] private Briquet[] m_Briquets;
		[SerializeField] private Allumettes m_Allumettes;
		[SerializeField] private float m_WalkSpeed;
		[SerializeField] private float m_RunSpeed;
		[SerializeField] private float m_Stamina =1000;
		[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_StickToGroundForce;
		[SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private bool m_UseFovKick;
		[SerializeField] private FOVKick m_FovKick = new FOVKick();
		[SerializeField] private bool m_UseHeadBob;
		[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
		[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
		[SerializeField] private AudioClip m_RespireSound;           // the sound played when character touches back on ground.
		[SerializeField] private AudioClip m_NoStaminaSound;           // the sound played when character touches back on ground.
		
		private Camera m_Camera;
		private bool m_Jump;
		private int indexBriquet;
		private GameObject lighter;
		private GameObject allumette;
		private bool canSprint;
		private float m_YRotation;
		private Vector2 m_Input;
		private Vector3 m_MoveDir = Vector3.zero;
		private CharacterController m_CharacterController;
		private CollisionFlags m_CollisionFlags;
		private bool m_PreviouslyGrounded;
		private Vector3 m_OriginalCameraPosition;
		private float m_StepCycle;
		private float m_NextStep;
		private bool m_Jumping;
		private AudioSource m_AudioSource;
		private AudioSource m_AudioSourceChild;
		private MilesStonesPARA currentMilestone;
		
		// Use this for initialization
		private void Start()
		{
			currentMilestone = GameObject.FindGameObjectWithTag ("MilesStones").GetComponent<MilesStonesPARA>();
			m_CharacterController = GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_FovKick.Setup(m_Camera);
			m_HeadBob.Setup(m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle/2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource>();
			m_AudioSourceChild = GameObject.FindGameObjectWithTag("AudioSourceBreath").GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
			lighter = GameObject.FindGameObjectWithTag (("Lighter"));
			lighter.SetActive (false);
			allumette = GameObject.FindGameObjectWithTag (("Allumette"));
			allumette.SetActive (false);
		}
		
		
		// Update is called once per frame
		private void Update()
		{
			RotateView();
			// the jump state needs to read here to make sure it is not missed
			if (!m_Jump)
			{
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
			
			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
			{
				StartCoroutine(m_JumpBob.DoBobCycle());
				PlayLandingSound();
				m_MoveDir.y = 0f;
				m_Jumping = false;
			}
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			{
				m_MoveDir.y = 0f;
			}
			
			m_PreviouslyGrounded = m_CharacterController.isGrounded;
		}
		
		
		private void PlayLandingSound()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play();
			m_NextStep = m_StepCycle + .5f;
		}

		private void PlayRespireSound()
		{
			bool changeSound;
			if(Input.GetButton("StopRespire") && canSprint)
			{
				//Stopper la respiration et auguementer les sons
				m_AudioSourceChild.volume -= 0.05f;
				if(m_AudioSourceChild.volume <0)
					m_AudioSourceChild.volume=0;
				
			}
			else
			{
				//Repiration sounds
				m_AudioSourceChild.volume += 0.3f;
				if(m_AudioSourceChild.volume >0.5f)
					m_AudioSourceChild.volume=0.5f;
			}
			if(m_Stamina < 100)
			{
				if(m_AudioSourceChild.clip == m_RespireSound)
					changeSound=false;
				else
					changeSound=true;

				m_AudioSourceChild.clip = m_RespireSound;
			}
			else
			{
				if(m_AudioSourceChild.clip == m_NoStaminaSound)
					changeSound=false;
				else
					changeSound=true;
				m_AudioSourceChild.clip = m_NoStaminaSound;
			}

			if(changeSound || !m_AudioSourceChild.isPlaying)
				m_AudioSourceChild.Play();
			m_NextStep = m_StepCycle + .5f;
		}
		private void FixedUpdate()
		{
			float speed;
			GetInput(out speed);
			// always move along the camera forward as it is the direction that it being aimed at
			Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;
			
			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
			                   m_CharacterController.height/2f);
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
			
			m_MoveDir.x = desiredMove.x*speed;
			m_MoveDir.z = desiredMove.z*speed;

			PlayRespireSound();
			//Actions joueur
			if(m_Briquets[indexBriquet]!=null)
			{
				//allumerBriquet
				if(Input.GetButton("Briquet"))
				{
					//Si un briquet n'est pas déja allumé
					if(!m_Briquets[indexBriquet].activer)
					{
						//Si il n'y a pas de briquet déja commencé
						if(GameObject.FindGameObjectWithTag("Briquet")==null)
						{
							Instantiate(m_Briquets[indexBriquet]);
							m_Briquets[indexBriquet] = GameObject.FindGameObjectWithTag("Briquet").GetComponent<Briquet>();
							//Si il y a une allumette. Éteint la
						}
						if(GameObject.FindGameObjectWithTag("Allumettes")!=null)
						{
							m_NbrAllumettes--;
							allumette.SetActive (false);
							DestroyObject(GameObject.FindGameObjectWithTag("Allumettes"));
						}
						m_Briquets[indexBriquet].activer=true;
						m_Briquets[indexBriquet].enabled=true;
						lighter.SetActive(true);
					}
					//Si le briquet est déja allumé
					else
					{
						m_Briquets[indexBriquet].activer=false;
						m_Briquets[indexBriquet].enabled=false;
						lighter.SetActive(false);
					}
					
				}
				//Si le briquet est vide, le jeté
				if (m_Briquets[indexBriquet].m_Essence <= 0)
				{
					m_Briquets[indexBriquet].activer=false;
					lighter.SetActive(false);
					DestroyObject(m_Briquets[indexBriquet].gameObject);
					indexBriquet++;
				}
			}

			if(Input.GetButton("Alumettes"))
			{
				//allumerAllumettes
				if(m_NbrAllumettes>0)
				{
					//Si il y a un briquet, l'éteindre, mais le conservé
					if(GameObject.FindGameObjectWithTag("Briquet")!=null)
					{
						m_Briquets[indexBriquet].activer=false;
						m_Briquets[indexBriquet].enabled = false;
						lighter.SetActive(false);
					}
					//Si il ny a pas d'allumettes déja allumé
					if(GameObject.FindGameObjectWithTag("Allumettes")==null)
					{
						Instantiate(m_Allumettes);
						allumette.SetActive (true);
						GameObject.FindGameObjectWithTag("Allumettes").GetComponent<Allumettes>().activer=true;
					}
				}
			}
			//Si l'allumette est morte
			if(GameObject.FindGameObjectWithTag("Allumettes")!=null)
				if(GameObject.FindGameObjectWithTag("Allumettes").GetComponent<Allumettes>().m_Duree<=0)
				{
					m_NbrAllumettes--;
					allumette.SetActive(false);
					DestroyObject(GameObject.FindGameObjectWithTag("Allumettes"));
				}
			if (m_CharacterController.isGrounded)
			{
				m_MoveDir.y = -m_StickToGroundForce;
				
				if (m_Jump)
				{
					m_MoveDir.y = m_JumpSpeed;
					PlayJumpSound();
					m_Jump = false;
					m_Jumping = true;
				}
			}
			else
			{
				m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
			}
			m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
			
			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);
		}
		
		
		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}
		
		
		private void ProgressStepCycle(float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
			{
				m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
					Time.fixedDeltaTime;
			}
			
			if (!(m_StepCycle > m_NextStep))
			{
				return;
			}
			
			m_NextStep = m_StepCycle + m_StepInterval;
			
			PlayFootStepAudio();
		}
		
		
		private void PlayFootStepAudio()
		{
			if (!m_CharacterController.isGrounded)
			{
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range(1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds[n];
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			m_FootstepSounds[n] = m_FootstepSounds[0];
			m_FootstepSounds[0] = m_AudioSource.clip;
		}
		
		
		private void UpdateCameraPosition(float speed)
		{
			Vector3 newCameraPosition;
			if (!m_UseHeadBob)
			{
				return;
			}
			if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
			{
				m_Camera.transform.localPosition =
					m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
					                    (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
			}
			else
			{
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
			}
			m_Camera.transform.localPosition = newCameraPosition;
		}
		
		
		private void GetInput(out float speed)
		{
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis("Vertical");
			
			bool waswalking = m_IsWalking;
			
			#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			m_IsWalking = !CrossPlatformInputManager.GetButton("Course");
			//Si le joueur marche. Il reprend de l'énergie
			if(m_IsWalking && !Input.GetButton("StopRespire"))
			{
				m_Stamina += 5;
				if(m_Stamina >= 500)
					canSprint=true;
				if(m_Stamina >= 1000)
					m_Stamina = 1000;
			}
			//Sinon il court, mais il perd de l'énergie
			else if(!m_IsWalking || Input.GetButton("StopRespire"))
			{
				m_Stamina -= 10;
				if(m_Stamina < 0)
				{
					canSprint=false;
					m_Stamina=0;
				}
			}
			//Si il n'a pas d'energie il doit marcher
			if(!canSprint)
				m_IsWalking=true;
			#endif
			// set the desired speed to be walking or running
			speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
			m_Input = new Vector2(horizontal, vertical);
			
			// normalize input if it exceeds 1 in combined length:
			if (m_Input.sqrMagnitude > 1)
			{
				m_Input.Normalize();
			}
			
			// handle speed change to give an fov kick
			// only if the player is going to a run, is running and the fovkick is to be used
			if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
			{
				StopAllCoroutines();
				StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
			}
		}
		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}
		
		
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			//Si le joueur touche un piege le blessant
			if(hit.collider.tag == "Trap")
			{
				hurtPlayer();
			}
			//dont move the rigidbody if the character is on top of it
			if (m_CollisionFlags == CollisionFlags.Below)
			{
				return;
			}

			if (body == null || body.isKinematic)
			{
				return;
			}
			body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
		}
		//Si le joueur est blessé, il est de retour au point de réapparition
		public void hurtPlayer()
		{
			print ("I'm hurt");
			m_Briquets [m_Briquets.Length - 1] = new Briquet ();
			transform.position = currentMilestone.Respawn.transform.position;
			transform.rotation = currentMilestone.Respawn.transform.rotation;
		}
		public void deathPlayer()
		{
			print ("I'm dead");
		}
	}
}
