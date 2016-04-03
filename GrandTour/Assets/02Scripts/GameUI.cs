using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour 
{
    //Text UI 항목 연견을 위한 변수
    public Text txtScore;
    //누적 점수를 기록하기 위한 변수
    private int totScore = 0;

	// Use this for initialization
	void Start () 
    {
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DispScore(0);
	}
    
	public void DispScore(int score)
    {
        totScore += score;
        txtScore.text = "SCORE  <color=#ff0000>" + totScore.ToString() + "</color>";

        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}
