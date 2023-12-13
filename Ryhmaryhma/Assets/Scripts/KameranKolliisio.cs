using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameranKolliisio : MonoBehaviour
{
    public Transform referenssiPaikka; //kameran pa
    

    public float kolliisioEtaisyys = 0.3f; // est‰‰ kameran tˆrm‰‰misen objekteihin
    public float kameraNopeus = 15f; // kuinka nopeasti kameran pit‰isi siirty‰ paikalleen, jos esteit‰ ei en‰‰n ole 

    Vector3 oletusSijainti;
    Vector3 normaaliSuunta;
    Transform parentTransform;
    float oletusEtaisyys;

    void Start()
    {
        AsetaOletusPaikat();
    }

    void LateUpdate()
    {
        KolliisionKasittely();
    }

    void AsetaOletusPaikat()
    {
        oletusSijainti = transform.localPosition;
        normaaliSuunta = oletusSijainti.normalized;
        parentTransform = transform.parent;
        oletusEtaisyys = Vector3.Distance(oletusSijainti, Vector3.zero);

        // lukitse kursori
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void KolliisionKasittely()
    {
        Vector3 nykyinenSijainti = oletusSijainti;
        RaycastHit osuma;

        // laskee suunnan kamerasta tiettyyn pisteeseen kohdeobjektissa.
        Vector3 kohdeSuunta = parentTransform.TransformPoint(oletusSijainti) - referenssiPaikka.position;

        // tarkistaa, onko esteit‰ tai osuu jonnekkin
        if (Physics.SphereCast(referenssiPaikka.position, kolliisioEtaisyys, kohdeSuunta, out osuma, oletusEtaisyys))
        {
            // jos on osuma, p‰ivitt‰‰ nykyisen sijainnin osumapisteeseen minus kollisioetaisyys
            // Lerppaa kameran liikkeen ettei hypp‰‰ koko ajan pelaajan jalkoihin
            Vector3 tulevaSijainti;
            tulevaSijainti = (normaaliSuunta * (osuma.distance - kolliisioEtaisyys));
            nykyinenSijainti = Vector3.Lerp(transform.localPosition, tulevaSijainti, Time.deltaTime * kameraNopeus / 3);
        }
        else
        {
            // Jos ei ole osumaa, k‰yt‰ Vector3.Lerp:ia sulavasti liikkumaan nykyisen ja tavoitesijainnin v‰lill‰
            nykyinenSijainti = Vector3.Lerp(transform.localPosition, oletusSijainti, Time.deltaTime * kameraNopeus);
        }

        // asettaa kameran sijainnin
        transform.localPosition = nykyinenSijainti;
    }
}