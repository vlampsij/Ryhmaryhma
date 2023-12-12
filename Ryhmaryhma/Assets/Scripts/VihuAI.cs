using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VihuAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform pelaaja;
    public Rigidbody rb;
    private Animator animaatio;

    public LayerMask tamaOnMaata, tamaOnPelaaja;

    //Partioinnin muuttujat
    public Vector3 kohde;
    bool onKohde;
    public float kohdeEtaisyys;

    //Hyökkäyksen muuttujat
    public float attackRate;
    bool hyokatty;
    //Aggro range
    public float nakoEtaisyys, hyokkaysEtaisyys;
    public bool pelaajaNakopiirissa, pelaajaKantamalla, aggro;

    private void Awake()
    {
        //Ottaa pelaajan muuttujaan
        pelaaja = GameObject.Find("PlayerPlacehold").transform;
        agent = GetComponent<NavMeshAgent>();
        animaatio = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        //Aggro range ja attack range
        pelaajaNakopiirissa = Physics.CheckSphere(transform.position, nakoEtaisyys, tamaOnPelaaja);
        pelaajaKantamalla = Physics.CheckSphere(transform.position, hyokkaysEtaisyys, tamaOnPelaaja);

        if (!pelaajaNakopiirissa && !pelaajaKantamalla && !aggro)
        {
            //Partioi jos pelaaja ei rangella
            Patrol();
        }
        else if (pelaajaNakopiirissa && !pelaajaKantamalla || aggro)
        {
            //Aggroo ja ajaa pelaajaa takaa kun osuu kantamalle
            AjaPelaajaa();
            aggro = true;
            //Hyökkää jos on pelaajan vieressä
            if (pelaajaNakopiirissa && pelaajaKantamalla)
            {
                Hyokkaa();
            }
        }

    }

    private void Patrol()
    {
        //Hakee kohdepisteen, jota kohti kulkee
        if (!onKohde)
        {
            HaeKohde();
        }
        if (onKohde)
        {
            agent.SetDestination(kohde);
        }
        
        Vector3 etaisyysKohteeseen = transform.position - kohde;
        //jos etäisyys on minimaalinen, nollaa muuttujan, jotta koodi hakee uuden kohteen
        if (etaisyysKohteeseen.magnitude < 1f)
        {
            onKohde = false;
        }
    }
    private void HaeKohde()
    {
        float randomZ = Random.Range(-kohdeEtaisyys, kohdeEtaisyys);
        float randomX = Random.Range(-kohdeEtaisyys, kohdeEtaisyys);
        //Kohde on random X ja Z pisteen tulos
        kohde = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Tarkistaa, että pisteelle voi kävellä
        if (Physics.Raycast(kohde, -transform.up, 2f, tamaOnMaata))
        {
            onKohde = true;
        }
    }
    private void AjaPelaajaa()
    {
        agent.SetDestination(pelaaja.position);
    }
    private void Hyokkaa()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(pelaaja);
        //Hyökkää pelaajan suuntaan, jos muuttuja sallii
        if (!hyokatty)
        {
            //Hyökkää eteenpäin. Periaatteessa projektiili voisi olla helpompi mut menin tällä
            rb.AddForce(transform.forward, ForceMode.Impulse);
            animaatio.SetTrigger("Hyokkays");
            

            hyokatty = true;
            //Resettaa muuttujan attackRaten kuluttua loppuun
            Invoke("HyokkaysReset", attackRate);
        }
    }
    private void HyokkaysReset()
    {
        hyokatty = false;
    }
    private void TuhoaVihu()
    {
        animaatio.SetBool("Kuolema", true);
        Destroy(gameObject, 1f);
    }
    private void OnDrawGizmosSelected()
    {
        //Näillä voi visualisoida vihollisen aggro rangen ja kantaman editorissa
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nakoEtaisyys);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hyokkaysEtaisyys);
    }
}
