using UnityEngine;
using System.Collections;

public class BladeTrapTriggers : MonoBehaviour {

    public GameObject bladeTrap;
    //pris sur soundBible
    private AudioSource ac;
    // Use this for initialization
    void Start () {
        ac = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ac.Play();
            bladeTrap.GetComponent<BladeTrapScript>().SetIsStarted(true);
            
        }

    }
}
