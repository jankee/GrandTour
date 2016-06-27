using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{
    //백그라운드 배열이다
    public Transform[] backgrounds;
    //카메라 움직임과 백그라운드 움직임의 비율
    private float[] parallaxScales;
    //
    public float smoothing = 1f;

    private Transform cam;
    private Vector3 previousCamPos;

    public void Awake()
    {
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallex = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundPosX = backgrounds[i].position.x + parallex;

            Vector3 backgroundTargetPos = new Vector3(backgroundPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
