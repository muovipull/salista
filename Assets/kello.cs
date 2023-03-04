using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kello : MonoBehaviour



{
    
    public float timer = 0.0f;
    public bool istime = false;
    public int secund = 0;
    public int minutes = 0;
    public aloitus_jos_aloitusvalmis Aloitus_Jos_Aloitusvalmis;
    void Update()
    {
        if(istime)
        {
            timer += Time.deltaTime;
            distime();
            print(secund);
            print("toimmii");



        }
        
    }
    public void distime()
    {
        minutes = Mathf.FloorToInt(timer / 60.0f);
        secund = Mathf.FloorToInt(timer - minutes * 60 );
        print(secund);

    }
    public void StartTimer()
    {
        if (aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset <= 0)
        {
            istime = true;
        }
    }
    public void stop()
    {
        istime=false;
        
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


    }



}
