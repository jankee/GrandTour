﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr: MonoBehaviour 
{
    //출현할 위치 배열
    public Transform[] points;
    //몬스터 프리팹 할당
    public GameObject monsterPrefab;

    //몬스터 발생 주기
    public float createTime = 2f;
    //몬스터 발생 수
    public int maxMonster = 10;
    //게임 종료 여부 변수
    public bool isGameOver = false;

    public static GameMgr instance = null;

    public List<GameObject> monsterPool = new List<GameObject>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }



	// Use this for initialization
	void Start () 
    {
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        //몬스터를 생성해서 풀에 저장
        for (int i = 0; i < maxMonster; i++)
        {
            GameObject monster = (GameObject)Instantiate(monsterPrefab);
            //생성한 몬스터 이름 설정
            monster.name = "Monster_" + i.ToString();
            //생성한 몬스터 비활성화
            monster.SetActive(false);
            //생성한 몬스터를 오브젝트 풀에 저장
            monsterPool.Add(monster);
        }

        if (points.Length >0)
        {
            StartCoroutine(this.CreateMonster());
        }
	}
    
	IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            //몬스터 생성주기시간 만큼 메인루프에 양보
            yield return new WaitForSeconds(createTime);

            if (isGameOver)
            {
                yield break;
            }

            foreach (GameObject monster in monsterPool)
            {
                if (!monster.activeSelf)
                {
                    int idx = Random.Range(1, points.Length);

                    monster.transform.position = points[idx].position;

                    monster.SetActive(true);

                    break;
                }
            }


            //현재 생성된 몬스터 갯수 산출
            //int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;

            //if (maxMonster > monsterCount)
            //{
                

            //    int idx = Random.Range(1, points.Length);

            //    Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            //}
            //else
            //{
            //    yield return null;
            //}
        }
    }
}
