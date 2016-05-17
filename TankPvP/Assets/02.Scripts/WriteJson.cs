using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class WriteJson : MonoBehaviour
{
    public Charactor player = new Charactor(0, "Austin The Wizard", 1337, false, new int[] { 3, 4, 5, 6, 7 });
    JsonData playerJson;

    // Use this for initialization
    void Start()
    {
        playerJson = JsonMapper.ToJson(player);

        print(playerJson);

        File.WriteAllText(Application.dataPath + "/Player.json", playerJson.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}


public class Charactor
{
    public int id;
    public string userName;
    public int health;
    public bool aggressive;
    public int[] stats;

    public Charactor(int id, string userName, int health, bool aggressive, int[] stats)
    {
        this.id = id;
        this.userName = userName;
        this.health = health;
        this.aggressive = aggressive;
        this.stats = stats;
    }
}