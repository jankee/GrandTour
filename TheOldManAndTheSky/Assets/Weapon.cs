using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public float Damage = 10;
    public LayerMask notToHit;

    public Transform BulletTrailPrefab;

    public Transform MuzzleFlashPrefab;

    private float timeToSpawnEffect = 0;
    private float effectSpawnRate = 10;

    private float timeToFire = 0;
    private Transform firePoint;

    // Use this for initialization
    void Awake()
    {
        firePoint = this.transform.FindChild("firePoint");

        if (firePoint == null)
        {
            Debug.LogError("No firePoint? WHAT?!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();

        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, notToHit);

        if (Time.time >= timeToSpawnEffect)
        {
            Effect();    
        }

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.red);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.white);

            print("We hit " + hit.collider.name + " and did " + Damage + " damage.");
        }
    }

    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);
    }
}
