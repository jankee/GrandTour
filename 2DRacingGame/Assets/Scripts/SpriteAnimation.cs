using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour
{

    public int colums           = 8;
    public int rows             = 8;

    private Renderer myRenderer;

    //애니메이션 컨트롤 변수
    public int currentFrame     = 1;
    public int currentAnim      = 0;
    public float animTime       = 0.0f;
    public float fps            = 10.0f;

    public bool explode         = false;

    private Vector2 framePosition;
    private Vector2 frameSize;
    private Vector2 frameOffset;
    private int i;

    private float carVelocity = 0;

    //애니메이션 프레임(최소, 최대) 변수
    private int idle            = 17;
    private int idleLeft        = 1;
    private int idleRight       = 2;
    private int driveMin        = 3;
    private int driveMax        = 4;
    private int driveLeftMin    = 5;
    private int driveLeftMax    = 6;
    private int driveRightMin   = 7;
    private int driveRightMax   = 8;
    private int Spin            = 9;
    private int explosionMin    = 10;
    private int explosionMax    = 16;

    //에니메이션 ID 변수
    private int animIdle        = 0;
    private int animIdleLeft    = 1;
    private int animIdleRight   = 2;
    private int animDrive       = 3;
    private int animDriveLeft   = 4;
    private int animDriveRight  = 5;
    private int animSpin        = 6;
    private int animExplosion   = 7;



    // Use this for initialization
    void Start()
    {
        myRenderer = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleAnimation();
	}

    void HandleAnimation()
    {
        FindAnimation();
        PlayAnimation();
    }

    void FindAnimation()
    {
        PlayerMovement playerMovement = this.transform.parent.GetComponent<PlayerMovement>();

        carVelocity = playerMovement.currentSpeed;

        if (carVelocity > 0.1f)
        {
            currentAnim = animDrive;

            if (Input.GetAxis("Horizontal") < 0.0f)
            {
                currentAnim = animDriveLeft;
            }
            if (Input.GetAxis("Horizontal") > 0.0f)
            {
                currentAnim = animDriveRight;
            }
        }
        if (carVelocity < 0.1f)
        {
            currentAnim = animIdle;
        }

        if (explode)
        {
            currentAnim = animExplosion;
        }
    }

    void PlayAnimation()
    {
        animTime -= Time.deltaTime;

        if (animTime <= 0)
        {
            currentFrame += 1;
            animTime += 1f / fps;
        }

        //One - Off Animations
        if (currentAnim == animExplosion)
        {
            currentFrame = Mathf.Clamp(currentFrame, explosionMin, explosionMax + 1);
            if (currentFrame > explosionMax)
            {
                explode = false;
            }
        }


        //루핑 애니메이션
        if (currentAnim == animIdle)
        {
            currentFrame = Mathf.Clamp(currentFrame, idle, idle);
        }
        if (currentAnim == animDrive)
        {
            currentFrame = Mathf.Clamp(currentFrame, driveMin, driveMax + 1);
            if (currentFrame > driveMax)
            {
                currentFrame = driveMin;
            }
        }
        if (currentAnim == animDriveLeft)
        {
            currentFrame = Mathf.Clamp(currentFrame, driveLeftMin, driveLeftMax + 1);
            if (currentFrame > driveLeftMax)
            {
                currentFrame = driveLeftMin;
            }
        }
        if (currentAnim == animDriveRight)
        {
            currentFrame = Mathf.Clamp(currentFrame, driveRightMin, driveRightMax + 1);
            if (currentFrame > driveRightMax)
            {
                currentFrame = driveRightMin;
            }
        }


        framePosition.y = 1;

        for (i = currentFrame; i > colums; i -= rows)
        {
            framePosition.y += 1;
        }

        framePosition.x = i - 1;


        frameSize = new Vector2(1f / colums, 1f / rows);

        print(framePosition.x / colums);
        print(1f - (framePosition.y / rows));

        frameOffset = new Vector2(framePosition.x / colums, 1f - (framePosition.y / rows));

        myRenderer.material.SetTextureScale("_MainTex", frameSize);
        myRenderer.material.SetTextureOffset("_MainTex", frameOffset);
    }
}
