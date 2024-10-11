using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEditor;
using System;

public class Teema : MonoBehaviour
{
    public static bool tumma_teema_p‰‰ll‰ = false;
    public static Action<bool> teema_vaihtunut;

    public string tema = "tumma" ;
    public Image tausta;
    public Image tausta1;
    public Image tausta2;
    public Image tausta3;
    public Image tausta4;
    public Image tausta5;
    public Image tausta7;
    public Image tausta8;
    public Image tausta9;
    public Image tausta10;
    public Image tausta11;
    public Image tausta12;
    public Image tausta13;
    public Image tausta14;
    public Image tausta15;
    public Image tausta16;
    public Image tausta17;
    public Image tausta18;
    public Image tausta19;
    public Image tausta20;
    public Image tausta21;
    public Image tausta22;
    public Image tausta23;
    public Image tausta24;
    public Image tausta25;
    public Image tausta26;
    public Image tausta27;
    public Image tausta28;
    public Image tausta29;
    public Image tausta30;
    public Text teksti;
    public Text teksti1;
    public Text teksti2;
    public Text teksti3;
    public Text teksti4;
    public Text teksti5;
    public Text teksti6;
    public Text teksti7;
    public Text teksti8;
    public Text teksti9;
    public Text teksti10;
    public TMP_Text Tmp_teksti2;
    public TMP_Text tmp_teksti;
    public TMP_Text tmp_teksti1;
    public TMP_Text tmp_teksti3;
    public TMP_Text tmp_teksti4;




    public void vaihto()
    {

        //tumma_teema_p‰‰ll‰ = !tumma_teema_p‰‰ll‰;
        //teema_vaihtunut.Invoke(tumma_teema_p‰‰ll‰);
        //0,return;
        if ( tema == "tumma")
        {
            tema = "vaalea";
            aseta_teema(Color.white, Color.black);
            print("vaihdettu valkoiseksi");
            PlayerPrefs.SetString("tema", tema);
        }
        else
        {
            tema = "tumma";
            aseta_teema(Color.black, Color.white);
            print("vaihdettu musta");
            PlayerPrefs.SetString("tema", tema);

        }
    }
    public void start_asetus()
    {
        if (tema == "vaalea")
        {
            tema = "vaalea";
            aseta_teema(Color.white, Color.black);
            print("vaihdettu valkoiseksi");

        }
        else
        {
            tema = "tumma";
            aseta_teema(Color.black, Color.white);
            print("vaihdettu musta");




        }
    }
    public void aseta_teema(Color vari, Color vasta)
    { 
        tausta.color = vari;
        tausta1.color = vasta;
        tausta2.color = vasta;
        tausta3.color = vasta;
        tausta4.color = vasta;
        tausta5.color = vasta;
        tausta7.color = vasta;
        tausta8.color = vasta;
        tausta9.color = vasta;
        tausta10.color = vasta;
        tausta11.color = vasta;
        tausta12.color = vasta;
        tausta13.color = vasta;
        tausta14.color = vasta;
        tausta15.color = vasta;
        tausta16.color = vasta;
        tausta17.color = vari;
        tausta18.color = vasta;
        

        teksti.color = vari;
        teksti1.color = vari;
        teksti2.color = vari;
        teksti3.color = vari;
        teksti4.color = vari;
        teksti5.color = vari;
        //teksti6.color = vari;
        //teksti7.color = vari;
        //teksti8.color = vari;
        //teksti9.color = vari;
        //teksti10.color = vari;
        Tmp_teksti2.color = vari;
        tmp_teksti.color = vari;
        tmp_teksti1.color = vari;
        tmp_teksti3.color = vasta;
        tmp_teksti4.color = vari;

        
            

    }
    
    // Start is called before the first frame update
    
    //void Start()
    //{
    //    tema = PlayerPrefs.GetString("tema", "tumma");
    //    start_asetus();
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
