using UnityEngine;
using System.Collections;

public class Briquet : MonoBehaviour {

	public float m_Essence=500;
	public bool activer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (activer)
		{
			m_Essence--;
			print("SA brule");
		}
	}
}
