using UnityEngine;
using System.Collections;

public class TankAnim : MonoBehaviour
{
    //텍스쳐의 회전속도
    private float scrollSpeed = 2f;
    private float addTime = 0f;
    private Renderer _renderer;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") > 0.1f)
        {
            addTime += Time.fixedDeltaTime * scrollSpeed;
        }
        else if (Input.GetAxis("Vertical") < -0.1f)
        {
            addTime -= Time.fixedDeltaTime * scrollSpeed;
        }

        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, addTime));

        //float offset = Input.GetAxis("Vertical");

        //offset += (Time.fxidDeltaTime * scrollSpeed);

        //offset += Input.GetAxis("Vertical");

        //print("offset : " + offset);

        //print(_renderer.material.mainTexture.name);

        //기본 텍스쳐의 y 오프셋 값 변경
        //_renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));

        //_renderer.material.mainTextureOffset = new Vector2(0, offset);

        //노말 텍스쳐의 y 오프셋 값 변경
        //_renderer.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
    }
}
