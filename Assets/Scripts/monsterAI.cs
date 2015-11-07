using UnityEngine;
using System.Collections;

public class monsterAI : MonoBehaviour
{
	public GameObject player;
	private Transform target;
	public float deadlyDistance;
	public float rotationDamping; // Lower rotation damping = slower rotation
	public float moveSpeed;

	// Use this for initialization
	void Start ()
	{
		target = player.transform;
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsAtDeadlyDistance ()) {
			// Kill the player
		} else {
			LookAtTarget ();
			MoveTowardTarget ();
		}
	
	}

	// Check is the current distance is deadly for the target 
	bool IsAtDeadlyDistance ()
	{
		return (deadlyDistance <= Vector3.Distance (target.position, transform.position));
	}

	// Look toward target
	void LookAtTarget ()
	{
		//Quaternion rotation = Quaternion.LookRotation (target.rotation - transform.rotation);
		//transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);

	}
	// Follow player from a certain initial distance at a certain speed.
	void MoveTowardTarget ()
	{
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
	}

}
