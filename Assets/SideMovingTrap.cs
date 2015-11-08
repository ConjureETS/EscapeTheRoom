using UnityEngine;
using System.Collections;

public class SideMovingTrap : MonoBehaviour {

    public float force;
    private Rigidbody rb;
    bool addLeft = false;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(100, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        print(vel.magnitude);
        
             

    }

    void pushTheShit()
    {

    }
}
