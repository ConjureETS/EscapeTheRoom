using UnityEngine;
using System.Collections;

public class BladeTrapTriggers : MonoBehaviour {

    public GameObject bladeTrap;
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            bladeTrap.GetComponent<BladeTrapScript>().SetIsStarted(true);
            
        }

    }
}
