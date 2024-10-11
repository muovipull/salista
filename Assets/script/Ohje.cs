using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ohje : MonoBehaviour
{
    public GameObject ohje_sivu;
    public GameObject ohje_avaus_nappi;
    
    
    public void avaa()
    {
        ohje_sivu.SetActive(true);
        ohje_avaus_nappi.SetActive(false);

    }
    public void sulje()
    {
        ohje_sivu.gameObject.SetActive(false);
        ohje_avaus_nappi.SetActive(true);

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
