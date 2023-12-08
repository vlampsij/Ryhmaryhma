using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattaPolku : MonoBehaviour
{
    public Transform HaePiste(int pisteIndex)
    {
        return transform.GetChild(pisteIndex);
    }

    public int HaeSeuraavaIndex(int nykyIndex)
    {
        //Käy polun lapsiobjektit järjestyksessä läpi
        int seuraavaIndex = nykyIndex + 1;
        
        //Jos objektit loppuu, aloittaa alusta
        if (seuraavaIndex == transform.childCount)
        {
            seuraavaIndex = 0;
        }

        return seuraavaIndex;
    }
}
