using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour 
{
    public void OnClickStartBtn(RectTransform rt)
    {
        print("Scale X : " + rt.localScale.x);
    }
}
