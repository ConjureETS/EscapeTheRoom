using UnityEngine;
using System.Collections;

public class monsterAI : MonoBehaviour
{
	public GameObject player;
	private Transform target;
	public float deadlyDistance;

	private NavMeshAgent agent;

	// Use this for initialization
	void Start ()
	{
		target = player.transform;

		agent = gameObject.GetComponent<NavMeshAgent> ();
		
		agent.SetDestination (target.position);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsAtDeadlyDistance ()) {
			// Kill the player
		}

		agent.SetDestination (target.position);
	
	}

	// Check is the current distance is deadly for the target 
	bool IsAtDeadlyDistance ()
	{
		return (Vector3.Distance (target.position, transform.position) <= deadlyDistance);
	}
}
