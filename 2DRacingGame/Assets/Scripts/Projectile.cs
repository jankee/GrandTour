using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public float lifeSpan = 3f;
    public float projectileRotation = 10f;

    private Vector3 projectileAxis;

    // Use this for initialization
    void Start()
    {
        DestroyProjectile();
    }

    // Update is called once per frame
    void Update()
    {
	
	}  

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject, lifeSpan);
    }
}
