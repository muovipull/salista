using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uusituote : MonoBehaviour
{
    public GameObject pois;
    public GameObject paalle;
    

    public void uusintuote()
    {
        pois.SetActive(false);
        paalle.SetActive(true);
    }




}
