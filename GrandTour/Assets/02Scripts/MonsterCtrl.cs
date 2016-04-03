using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour 
{
    //몬스터의 상태 정보 enum 변수
    public enum MonsterState
    {
        idle,
        trace,
        attack,
        die,
    };

    //몬스터의 현재 상태 정보
    public MonsterState monsterState = MonsterState.idle;

    private Transform monsterTr;

    private Transform playerTr;

    private NavMeshAgent nvAgent;

    private Animator animator;

    //몬스터 추적거리
    public float traceDist = 10f;
    //몬스터 공격거리
    public float attackDist = 2f;
    //몬스터 사망여부
    private bool isDie = false;

    //혈흔 효과 
    public GameObject bloodEffect;
    //바닥 혈흔 효과
    public GameObject bloodDecal;

    public int hp = 100;

    private GameUI gameUI;

	// Use this for initialization
	void Start () 
    {
        monsterTr = GetComponent<Transform>();

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

        //nvAgent.destination = playerTr.position;

        //일정한 간격으로 몬스터의 행동 상태를 체크하는 코루틴 함수 실행
        StartCoroutine(CheckMonsterState());

        //일정한 간격으로 몬스터의 상태에 따라 애니 설정
        StartCoroutine(MonsterAction());
	}

    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
    }

    public void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;

    }
	



    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            //0.2초 후 다음으로 넘어감
            yield return new WaitForSeconds(0.2f);
            //몬스터와 플레이어의 거리를 측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    animator.SetBool("IsTrace", false);

                    nvAgent.Stop();
                    break;

                case MonsterState.trace:
                    animator.SetBool("IsTrace", true);
                    animator.SetBool("IsAttack", false);
                    //추적 대상의 위치를 넘겨 줌
                    nvAgent.destination = playerTr.position;
                    //추적을 재시작
                    nvAgent.Resume();
                    break;

                case MonsterState.attack:
                    animator.SetBool("IsAttack", true);
                    nvAgent.Stop();
                    break;
                case MonsterState.die:
                    break;
            }
            yield return null;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0)
            {
                MonsterDie();
            }

            CreateBloodEffect(collision.transform.position);

            Destroy(collision.gameObject);

            animator.SetTrigger("IsHit");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
    }

    void OnPlayerDie()
    {

        StopAllCoroutines();

        nvAgent.Stop();
        animator.SetTrigger("IsPlayerDie");
    }



    void CreateBloodEffect(Vector3 pos)
    {
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);

        Destroy(blood1, 2.0f);

        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);

        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));

        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);

        float scale = Random.Range(1.5f, 3.5f);

        blood2.transform.localScale = Vector3.one * scale;

        Destroy(blood2, 5f);
    }

    void MonsterDie()
    {
        gameObject.tag = "Untagged";
        //모든 coroutine을 정지
        StopAllCoroutines();

        isDie = true;

        monsterState = MonsterState.die;
        nvAgent.Stop();
        animator.SetTrigger("IsDie");

        //몬스터에 추가된 Collision을 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach(Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }


        gameUI.DispScore(50);

        gameObject.SetActive(false);

        //Destroy(this.gameObject, 10f);
    }
}
