using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public float speed = 6000f;



    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
