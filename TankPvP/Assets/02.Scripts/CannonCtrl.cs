using UnityEngine;
using System.Collections;

public class CannonCtrl : MonoBehaviour
{
    private Transform tran;

    public float rotSpeed = 100f;

    // Use this for initialization
    void Start()
    {
        tran = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;

        tran.Rotate(angle, 0, 0);
    }
}
