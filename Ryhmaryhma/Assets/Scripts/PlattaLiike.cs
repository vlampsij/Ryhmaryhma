using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattaLiike : MonoBehaviour
{
    [SerializeField]
    private PlattaPolku _polku;

    [SerializeField]
    private float _nopeus;

    private int _kohdePisteIndex;

    private Transform _edellinen;
    private Transform _kohde;

    private float _aikaPisteeseen;
    private float _kulunutAika;
    // Start is called before the first frame update
    void Start()
    {
        SeuraavaKohdePiste();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _kulunutAika += Time.deltaTime;

        float kulunutProsentti = _kulunutAika / _aikaPisteeseen;
        //Tällä funktiolla platta hidastaa matkan alussa ja lopussa
        kulunutProsentti = Mathf.SmoothStep(0, 1, kulunutProsentti);
        transform.position = Vector3.Lerp(_edellinen.position, _kohde.position, kulunutProsentti);

        if (kulunutProsentti >= 1 )
        {
            SeuraavaKohdePiste();
        }
    }

    private void SeuraavaKohdePiste()
    {
        //Hakee uuden kohtee Plattapolun funktiolla ja ottaa sen kohteeksi
        _edellinen = _polku.HaePiste(_kohdePisteIndex);
        _kohdePisteIndex = _polku.HaeSeuraavaIndex(_kohdePisteIndex);
        _kohde = _polku.HaePiste(_kohdePisteIndex);

        _kulunutAika = 0;

        //Ottaa pisteiden välisen etäisyyden ja laskee matkaan tarvittavan ajan
        float aikaKohteeseen = Vector3.Distance(_edellinen.position, _kohde.position);
        _aikaPisteeseen = aikaKohteeseen / _nopeus;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ottaa pelaajan platformin lapsiobjektiksi kun osuu triggeriin
        //Näin pelaaja liikkuu platan mukana
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        //Poistaa pelaajan platan lapsiobjekteista
        other.transform.SetParent(null);
    }
}
