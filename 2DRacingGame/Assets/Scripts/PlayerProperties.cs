using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    public enum PlayerState
    {
        CarDead                 = 0,
        CarNormal               = 1,
        CarProjectile           = 2,
        CarTrap                 = 3,    
        CarBoost                = 4,
    }

    public PlayerState playerState = PlayerState.CarNormal;

    //파워업 변수
    public GameObject projectile;
    public GameObject trap;
    public GameObject boost;

    public float projectileSpeed = 100f;

    //파워 소켓 변수
    public Transform projectileSocket;
    public Transform trapSocket;
    public Transform boostSocket;

    public bool hasProjectile           = false;
    public bool hasTrap                 = false;
    public bool hasBoost                = false;

    public bool changeState             = false;
    public bool canPickUp               = true;

    public float boostTimer             = 2f;
    public float resetBoostTimer        = 2f;
    public bool boostTimerActive        = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (changeState)
        {
            SetPlayerState();
        }

        if (hasProjectile)
        {
            GameObject cloneProjectile;
            Vector3 fireProjectile = transform.forward * projectileSpeed;

            if (Input.GetButtonDown("Fire1"))
            {
                cloneProjectile = (GameObject)Instantiate(projectile, projectileSocket.transform.position, transform.rotation);
                Rigidbody cloneRigi = cloneProjectile.GetComponent<Rigidbody>();
                cloneRigi.AddRelativeForce(fireProjectile);

                playerState = PlayerState.CarNormal;
                changeState = true;
            }
        }
    }

    void SetPlayerState()
    {
        switch (playerState)
        {
            case PlayerState.CarDead:
                hasProjectile           = false;
                hasTrap                 = false;
                hasBoost                = false;
                changeState             = false;
                canPickUp               = false;
                break;
            case PlayerState.CarNormal:
                hasProjectile           = false;
                hasTrap                 = false;
                hasBoost                = false;
                changeState             = false;
                canPickUp               = true;
                break;
            case PlayerState.CarProjectile:
                hasProjectile           = true;
                hasTrap                 = false;
                hasBoost                = false;
                changeState             = false;
                canPickUp               = false;
                break;
            case PlayerState.CarTrap:
                hasProjectile           = false;
                hasTrap                 = true;
                hasBoost                = false;
                changeState             = false;
                canPickUp               = false;
                break;
            case PlayerState.CarBoost:
                hasProjectile           = false;
                hasTrap                 = false;
                hasBoost                = true;
                changeState             = false;
                canPickUp               = false;
                break;
        }
    }
}
