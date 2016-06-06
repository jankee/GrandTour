using UnityEngine;
using System.Collections;

//아이템 타입을 정해 enum으로 정한다.
public enum ItemType 
{
    MANA,
    HEALTH,
};

public class Item : MonoBehaviour
{
    //아이템타임을 type변수 정한다.
    public ItemType type;
    //사용할 sprite를 사용 할 변수
    public Sprite spriteNeutral;
    //선택할때 sprite를 사용 할 변수
    public Sprite spriteHighlighted;
    //아이템의 최대치를 정할 변수
    public int maxSize;

    public void Use()
    {
        switch (type)
        {
            case ItemType.MANA:
                print("I used a Mana Potion");
                break;
            case ItemType.HEALTH:
                print("I used a Health Potion");
                break;
        }
    }
}
