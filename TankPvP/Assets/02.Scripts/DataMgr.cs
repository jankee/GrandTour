using UnityEngine;
using System.Collections;

public class DataMgr : MonoBehaviour
{
    //싱글턴 선언
    public static DataMgr instance = null;

    //MySQL 데이터베이스 사용할 고유번호
    private const string seqNo = "u101113475_hoon";

    //점수 저장 php 주소
    private string urlSave = "http://jankee.esy.es/save_score.php";

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public IEnumerator SaveScore(string user_name, int killCount)
    {
        //POST 방식으로 인자를 전달 하기 위한 FORM선언
        WWWForm form = new WWWForm();

        //전달 파라미터 설정
        form.AddField("user_name", user_name);
        form.AddField("kill_count", killCount);
        form.AddField("seq_no", seqNo);

        var www = new WWW(urlSave, form);
        //완료 시점까지 대기
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print(www.text);
        }
        else
        {
            print("Error : " + www.error);
        }
    }
}
