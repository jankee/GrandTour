using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{

    SceneManager SManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
	
	}

    public void ClickNext()
    {
        Application.LoadLevel("Main_001");
    }
}
