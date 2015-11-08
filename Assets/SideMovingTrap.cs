using UnityEngine;
using System.Collections;

public class SideMovingTrap : MonoBehaviour {

    public float force;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        //rb.AddForce(100, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        //print(vel.magnitude);
        float toBeApplied = Mathf.Sin(Time.time) * force;
        //print(toBeApplied);
        rb.AddForce(toBeApplied, 0, 0);

    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            print("playerDed");
            Destroy(other.gameObject);
        }
    }
}
