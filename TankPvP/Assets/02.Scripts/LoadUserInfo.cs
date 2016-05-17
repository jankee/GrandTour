using SimpleJSON;
using System.Collections;
using UnityEngine;
using System;
using System.Text;

public class LoadUserInfo : MonoBehaviour
{

    public TextAsset jsonData = null;
    private string strJsonData = null;

    // Use this for initialization
    void Start()
    {

        //Resource 폴더의 JSON파일을 로드
        jsonData = Resources.Load<TextAsset>("driver");
        strJsonData = jsonData.text;

        //utf-8 인코딩
        //byte[] byteForEncoding = Encoding.UTF8.GetBytes(strJsonData);
        //string encodedString = Convert.ToBase64String(byteForEncoding);

        //uif-8 디코딩
        //byte[] decodededByte = Convert.FromBase64String(encodedString);
        //string decodedString = Encoding.UTF8.GetString(decodededByte);

        //print(decodedString);


        //TexAsset에서 string 타입의 텍스트를 추출


        //print(strJsonData);

        //JSON파일을 파싱
        var N = JSON.Parse(strJsonData);

        //"이름" 키에 저장된 키값을 축출
        string user_name = N["Character"][3]["Driver"].ToString();

        print(user_name);

        //print(N["Character"]["Job"]);

        ////"Ability" 중에 "Level"키 값을 축출
        //int level = N["Ability"]["Level"].AsInt;

        //print(level);

        //"Skill"배열값을 축출
        for (int i = 0; i < N["Character"].Count; i++)
        {
            print(N["Character"][i].ToString());
        }
    }
}
