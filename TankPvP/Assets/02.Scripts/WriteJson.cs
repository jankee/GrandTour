using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class WriteJson : MonoBehaviour
{
    public Driver[] driver;
    JsonData playerJson;

    // Use this for initialization
    void Start()
    {


        playerJson = JsonMapper.ToJson(playerJson);

        print(playerJson);

        File.WriteAllText(Application.dataPath + "/Player.json", playerJson.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}