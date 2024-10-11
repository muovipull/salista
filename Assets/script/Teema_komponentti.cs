using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Teema_komponentti : MonoBehaviour
{
    private Image kuva;
    private Text teksti;
    private TextMeshProUGUI teksti_tmp;
    public Color tumma_vari;
    private Color alku_vari;
    
    // Start is called before the first frame update
    void Start()
    {
        kuva= GetComponent<Image>();
        teksti=GetComponent<Text>();
        teksti_tmp =GetComponent<TextMeshProUGUI>();

        if ( kuva != null )
        {
            alku_vari = kuva.color;



        }
        if( teksti != null )
        {
            alku_vari= teksti.color;





        }
        if( teksti_tmp != null )
        {
            alku_vari = teksti_tmp.color;


        }
        teema_vaihtunut(Teema.tumma_teema_p‰‰ll‰);
        Teema.teema_vaihtunut += teema_vaihtunut;
    }
    public void teema_vaihtunut(bool tumma_teema)
    {
        if(kuva != null)  kuva.color = tumma_teema ? tumma_vari : alku_vari;
        if( teksti != null ) teksti.color = tumma_teema ? tumma_vari : alku_vari;
        if ( teksti_tmp != null ) teksti_tmp.color = tumma_teema ? tumma_vari : alku_vari;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
