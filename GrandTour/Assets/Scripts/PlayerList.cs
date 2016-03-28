using UnityEngine;
using System.Collections;

public enum PlayerType
{
    Sprinter,
    Climber,
    AllRounder,
    Rouleur,
    TimeTrilist,
}


public class PlayerList : MonoBehaviour
{
    public PlayerType type;

    public Sprite spriteNomal;

    public Sprite spriteHighlight;

    public int health;

    public int power;

    public int speed;

    public int technic;

    public int sprint;

    public int sprintPoint;

	public void Use()
    {
        switch (type)
        {
            case PlayerType.Sprinter:
                print("I am Sprinter");
                break;
            case PlayerType.Climber:
                print("I am Climber");
                break;
            case PlayerType.AllRounder:
                break;
            case PlayerType.Rouleur:
                break;
            case PlayerType.TimeTrilist:
                break;
        }
    }
}
