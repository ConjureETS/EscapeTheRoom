using UnityEngine;
using System.Collections;

public class HoleScript : MonoBehaviour {
    public GameObject floor;
    // Use this for initialization
    private GameObject player;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            print("floor ded");
            other.GetComponent<Rigidbody>().AddForce(0, 50, 0);
            floor.GetComponent<Collider>().enabled = false;
        }
    }

}
