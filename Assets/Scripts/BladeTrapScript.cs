using UnityEngine;
using System.Collections;

public class BladeTrapScript : MonoBehaviour {
    public float trapSpeed;
    public int maxDistance;

    private Rigidbody rb;
    private Vector3 trapPosition;
    private bool goingUp;
    private bool isStarted;
	// Use this for initialization
	void Start () {
        isStarted = false;
        rb = this.GetComponent<Rigidbody>();
        trapPosition = this.transform.position;
        goingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isStarted) { 
            if (this.transform.position.y <= -0.5) {
                goingUp = true;
            }
            if(this.transform.position.y >= 0.5)
            {
                goingUp = false;
            }
            if(goingUp)
                this.transform.Translate(0, trapSpeed * Time.deltaTime, 0);
            else
                this.transform.Translate(0, -(trapSpeed * Time.deltaTime), 0);
            }
        
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            //temporaire
            Destroy(other.gameObject);
        }
    }

    public bool GetIsStarted()
    {
        return this.isStarted;
    }

    public void SetIsStarted(bool isStarted)
    {
        this.isStarted = isStarted;
    }
    
}
