using UnityEngine;
using System.Collections;

public class BtnJouer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1"))
			Application.LoadLevel(1);
		if (Input.GetButton ("Quit"))
			Application.Quit();
	}
}
