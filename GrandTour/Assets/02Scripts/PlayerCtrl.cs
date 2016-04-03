using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackWard;
    public AnimationClip runRight;
    public AnimationClip rumLeft;
}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0f;
    private float v = 0f;

    private Transform tr;

    public float moveSpeed = 10f;

    public float rotSpeed = 100f;

    public int hp = 100;

    private int initHp;

    public Image imgHpbar;

    //private GameMgr gameMgr;


    //인스펙터뷰에 표시할 애니메이션 클레스 변수
    public Anim anim;
    //3D 모델의 Animation 컴포넌트에 접근하기 위한 변수
    public Animation _animation;

    //델리게이트 함수 선언
    public delegate void PlayerDieHandler();

    public static event PlayerDieHandler OnPlayerDie;



	// Use this for initialization
	void Start ()
    {
        initHp = hp;

        //Transform 컴포넌트 할당
        tr = this.GetComponent<Transform>();

        //자신하위의 Animation컴포넌트 찾아와 할당
        _animation = GetComponentInChildren<Animation>();

        _animation.clip = anim.idle;
        _animation.Play();

        //gameMgr = GameObject.Find("GameManager").GetComponent<GameMgr>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //Translate(이동방향 * 속도 * 변위값 * Time.deltaTime. 기준좌표(로컬좌표))
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("MouseX"));

        //키보드 입력값을 기준으로 애니메이션 실행
        if (v >= 0.1f)
        {
            //_animation.clip = anim.runForward;
            _animation.CrossFade(anim.runForward.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            _animation.CrossFade(anim.runBackWard.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            _animation.CrossFade(anim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            _animation.CrossFade(anim.rumLeft.name, 0.3f);
        }
        else
        {
            _animation.CrossFade(anim.idle.name, 0.3f);
        }

        //print("h = " + h + "\n" + "v = " + v);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            imgHpbar.fillAmount = (float)hp / (float)initHp;

            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        print("Player Die");

        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //foreach (GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        OnPlayerDie();

        GameMgr.instance.isGameOver = true;
    }
}
