using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class kello : MonoBehaviour



{
    
    public float timer = 0.0f;
    public int istime = 0;
    public int secund = 0;
    public GameObject uudelleen;
    public Text rangaistus;
    public int rangaistusm = 20;
    public int minutes = 0;
    void Update()
    {
        StartTimer();
        if(istime==1)
        {
            rangaistus.text = "olet yritt‰nyt liian monta kertaa v‰‰rin siksi joudut odottamaan\n" + rangaistusm + "\n minuuttia ennen kuin voin taas kirjautua";
            timer += Time.deltaTime;
            distime();
            //print(secund);
            //print("toimmii");



        }
        
        if (rangaistusm == 0)
        {
            aloitus_jos_aloitusvalmis.j‰ljell‰_olevat_yritykset = 5;
            uudelleen.SetActive(true);

        }

        if(minutes==1)
        {
            rangaistus.text = "olet yritt‰nyt liian monta kertaa v‰‰rin siksi joudut odottamaan\n" + rangaistusm + "\n minuuttia ennen kuin voin taas kirjautua";
            rangaistusm -= minutes;
            minutes = 0;
            secund = 0;
            timer = 0;
            print(rangaistusm);
        }
        
        
    }
    public void distime()
    {
        minutes = Mathf.FloorToInt(timer / 60.0f);
        secund = Mathf.FloorToInt(timer - minutes * 60 );
        //print(secund);

    }
    public void StartTimer()
    {
        if (aloitus_jos_aloitusvalmis.kello == 1)
        {
            istime = 1;
        }
    }
    public void stop()
    {
        istime=0;
        
    }
    public void ResetTimer()
    {
        timer = 0.0f;
    }
    public void print()
    {
        print(secund);
        print(minutes);
        print(timer);
        print(istime);
        minutes = 1;



    }
    void Start()
    {
        rangaistusm = PlayerPrefs.GetInt("rangasitus", 20);
        istime = PlayerPrefs.GetInt("istime", istime); 
    }



    public void OnApplicationFocus(bool on)
    {
        if (on == false)
        {
            PlayerPrefs.SetInt("istime", istime);
            PlayerPrefs.SetInt("rangasitus", rangaistusm);


        }
    }


}
