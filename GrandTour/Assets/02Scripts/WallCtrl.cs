using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour
{
    public GameObject sparkEffect;

    public void OnCollisionEnter(Collision collision)
    {
        print("Wall");
        if (collision.collider.tag == "BULLET")
        {
            GameObject spark = (GameObject)Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);

            Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 0.2f);

            Destroy(collision.gameObject);
        }
    }

}
