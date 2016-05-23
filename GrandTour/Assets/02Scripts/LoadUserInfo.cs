using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class LoadUserInfo : MonoBehaviour
{
    public List<Driver> driver;

    public TextAsset jsonDataString = null;
    private JsonData N = null;

    // Use this for initialization
    void Start()
    {
        driver = new List<Driver>();
        
        //Resource 폴더의 JSON파일을 로드
        jsonDataString = Resources.Load<TextAsset>("data_info");
        N = JsonMapper.ToObject(jsonDataString.text);

        //"이름" 키에 저장된 키값을 축출
        string user_name = N["Driver"][3]["Name"].ToString();

        print(user_name);

        int pipi = Convert.ToInt16(N["Driver"][0]["Pedal"].ToString());

        print(pipi);

        int popo = Convert.ToInt32(pipi);

        print(popo);


        //print(N["Character"]["Job"]);

        ////"Ability" 중에 "Level"키 값을 축출
        //int level = N["Ability"]["Level"].AsInt;

        print(N["Driver"].Count);

        //Driver클레스에 driver 객체를 추가
        for (int i = 0; i < N["Driver"].Count; i++)
        {
            string name = N["Driver"][i]["Name"].ToString();
            int pedal = Convert.ToInt16(N["Driver"][i]["Pedal"].ToString());
            int shift = Convert.ToInt16(N["Driver"][i]["Shift"].ToString());
            int steer = Convert.ToInt16(N["Driver"][i]["Steer"].ToString());
            int appl = Convert.ToInt16(N["Driver"][i]["Appl"].ToString());
            int tech = Convert.ToInt16(N["Driver"][i]["Tech"].ToString());
            int anlys = Convert.ToInt16(N["Driver"][i]["Anlys"].ToString());
            int pss = Convert.ToInt16(N["Driver"][i]["PSS"].ToString());
            int ata = Convert.ToInt16(N["Driver"][i]["Shift"].ToString());
            int salary = Convert.ToInt16(N["Driver"][i]["Salary"].ToString());

            driver.Add(new Driver(name, pedal, shift, steer, appl, tech, anlys, pss, ata, salary));
        }

        print(driver[30].name);

        for (int i = 0; i < N["Mashine"].Count; i++)
        {
            string name = N["Mashine"][i]["Name"].ToString();
            int pedal = Convert.ToInt16(N["Mashine"][i]["Pedal"].ToString());
            int shift = Convert.ToInt16(N["Mashine"][i]["Shift"].ToString());
            int steer = Convert.ToInt16(N["Mashine"][i]["Steer"].ToString());
            int appl = Convert.ToInt16(N["Mashine"][i]["Appl"].ToString());
            int tech = Convert.ToInt16(N["Mashine"][i]["Tech"].ToString());
            int anlys = Convert.ToInt16(N["Mashine"][i]["Anlys"].ToString());
            int pss = Convert.ToInt16(N["Mashine"][i]["PSS"].ToString());
            int ata = Convert.ToInt16(N["Mashine"][i]["Shift"].ToString());
            int salary = Convert.ToInt16(N["Mashine"][i]["Salary"].ToString());

            //Mashine.Add(new Driver(name, pedal, shift, steer, appl, tech, anlys, pss, ata, salary));
        }
    }
}
