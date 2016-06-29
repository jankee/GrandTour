using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
        {
            Destroy(other.gameObject);

            //Destroy(gameObject);
        }
    }


}
