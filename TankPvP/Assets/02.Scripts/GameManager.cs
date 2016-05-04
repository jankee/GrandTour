using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string user_name = "jankee";

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DataMgr.instance.SaveScore(user_name, 1));
        }
    }
}
