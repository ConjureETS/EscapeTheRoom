using UnityEngine;
using System.Collections;

public class Allumettes : MonoBehaviour {

	public float m_Duree=500;
	public bool activer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		if (activer)
		{
			m_Duree--;
			print("SA brule, mais moins");
		}
	}
}
