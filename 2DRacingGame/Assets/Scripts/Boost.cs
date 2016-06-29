using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour
{
    public float lifeSpan = 3f;

    // Use this for initialization
    void Start()
    {
        DestroyBoost();
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    void DestroyBoost()
    {
        Destroy(gameObject, lifeSpan);
    }
}
