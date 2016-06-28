using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum PowerType
    {
        Projectile          = 0,
        Trap                = 1,
        Boost               = 2,
    }

    public PowerType powerType = PowerType.Projectile;

    private GameObject playerGameObject;


    // Use this for initialization
    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerProperties playerProperties = playerGameObject.GetComponent<PlayerProperties>();

        if (other.tag == "Player")
        {
            print("We have a powerup!");
            ApplyPowerUp(playerProperties);
        }
    }

    int ApplyPowerUp(PlayerProperties playerStatus)
    {
        switch (powerType)
        {
            case PowerType.Projectile:
                if (playerStatus.playerState == PlayerProperties.PlayerState.CarNormal)
                {
                    print("We have a projectile");
                    playerStatus.playerState = PlayerProperties.PlayerState.CarProjectile;
                    playerStatus.hasProjectile = true;
                    playerStatus.changeState = true;
                }
                break;
            case PowerType.Trap:
                if (playerStatus.playerState == PlayerProperties.PlayerState.CarNormal)
                {
                    print("We have a Trap");
                    playerStatus.playerState = PlayerProperties.PlayerState.CarTrap;
                    playerStatus.hasTrap = true;
                    playerStatus.changeState = true;
                }
                break;
            case PowerType.Boost:
                if (playerStatus.playerState == PlayerProperties.PlayerState.CarNormal)
                {
                    print("We have a Boost");
                    playerStatus.playerState = PlayerProperties.PlayerState.CarBoost;
                    playerStatus.hasBoost = true;
                    playerStatus.changeState = true;
                }
                break;
        }
        return (int)powerType;
    }
}
