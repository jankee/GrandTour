using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed                  = 10f;
    public float reverseSpeed           = 5f;
    public float turnSpeed              = 0.6f;

    private float moveDirection         = 0f;
    private float turnDirection         = 0f;

    public float currentSpeed           = 0f;

    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentSpeed = Mathf.Abs(transform.InverseTransformDirection(rb.velocity).z);

        float maxAngularDrag = 2.5f;
        float currentAngularDrag = 1f;
        float aDragLeftTime = currentSpeed * 0.1f;

        float maxDrag = 1.0f;
        float currentDrag = 2.5f;
        float dragLerpTime = currentSpeed * 0.1f;

        float myAngularDrag = Mathf.Lerp(currentAngularDrag, maxAngularDrag, aDragLeftTime);
        float myDrag = Mathf.Lerp(currentDrag, maxDrag, dragLerpTime);

        if (Input.GetAxis("Vertical") > 0f)
        {
            moveDirection = Input.GetAxis("Vertical") * speed;
            rb.AddRelativeForce(0, 0, moveDirection);

            if (currentSpeed > 0.05f)
            {
                turnDirection = Input.GetAxis("Horizontal") * turnSpeed;
                rb.AddRelativeTorque(0, turnDirection, 0);    
            }
        }
        if (Input.GetAxis("Vertical") < 0f)
        {
            moveDirection = Input.GetAxis("Vertical") * reverseSpeed;
            rb.AddRelativeForce(0, 0, moveDirection);

            if (currentSpeed > 0.05f)
            {
                turnDirection = Input.GetAxis("Horizontal") * turnSpeed;
                rb.AddRelativeTorque(0, -turnDirection, 0);    
            }
        }

        rb.angularDrag = myAngularDrag;
        rb.drag = myDrag;
    }
}
