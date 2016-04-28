using UnityEngine;
using System.Collections;

public class TankDamage : MonoBehaviour
{
    //탱크폭파후 투명처리를 위한 MeshRenderer 컴포넌트 배열
    private MeshRenderer[] renderers;
    //탱크 폭발 이펙트
    private GameObject expEffect = null;

    //탱크 초기 생명치
    private int initHp = 100;
    //탱크의 현재 생명치
    private int currHp = 0;

    // Use this for initialization
    void Awake()
    {
        //탱크 모델의 모든 MeshRenderer 컴포넌트를 배열에 할당
        renderers = GetComponentsInChildren<MeshRenderer>();

        //현재 생명치를 초기 생명치로 설정
        currHp = initHp;
        //탱크 폭발시 생성시킬 폭발 효과 로드
        expEffect = Resources.Load<GameObject>("Large Explosion");
    }

    void OnTriggerEnter(Collider other)
    {
        if (currHp > 0 && other.tag == "CANNON")
        {
            currHp -= 20;
            if (currHp <= 0)
            {
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    IEnumerator ExplosionTank()
    {
        yield return new WaitForSeconds(0);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
