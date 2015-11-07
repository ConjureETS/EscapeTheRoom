using UnityEngine;
using System.Collections;

public class monsterAI : MonoBehaviour
{
	public GameObject player;
	private Transform target = player.transform;
	public float deadlyDistance;
	public float rotationDamping;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Check is the current distance is deadly for the target 
	bool IsAtDeadlyDistance () {
		return (deadlyDistance <= Vector3.Distance(target.position, transform.position);
	}

	// Follow player from a certain initial distance at a certain speed.
	void FollowTarget ()
	{
	
	}

	// Look toward target
	void LookAtTarget () {
		Quaternion rotation = Quaternion.LookRotation (target.rotation - transform.rotation);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

	}
}
