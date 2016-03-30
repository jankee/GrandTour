using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    private float h = 0f;
    private float v = 0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        print("h = " + h + "\n" + "v = " + v);

        if (h >= 0.1f)
        {
            print("Horizontal");
        }
        else if (h <= -0.1f)
        {
            print("-Horizontal");
        }
        else if (v >= 0.1f)
        {
            print("Vertical");
        }
        else if (v <= -0.1f)
        {
            print("-Vertical");
        }
        else
        {
            print("Idle");
        }
	}
}
