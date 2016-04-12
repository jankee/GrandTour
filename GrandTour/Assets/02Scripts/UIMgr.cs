using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour 
{
    public void OnClickStartBtn(RectTransform rt)
    {
        print("Click Buttton");

        Application.LoadLevel("scLevel01");
        Application.LoadLevelAdditive("scPlay");
    }
}
