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

    //Hy�kk�yksen muuttujat
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
            //Hy�kk�� jos on pelaajan vieress�
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
        //jos et�isyys on minimaalinen, nollaa muuttujan, jotta koodi hakee uuden kohteen
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

        //Tarkistaa, ett� pisteelle voi k�vell�
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
        //Hy�kk�� pelaajan suuntaan, jos muuttuja sallii
        if (!hyokatty)
        {
            //Hy�kk�� eteenp�in. Periaatteessa projektiili voisi olla helpompi mut menin t�ll�
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
        //N�ill� voi visualisoida vihollisen aggro rangen ja kantaman editorissa
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nakoEtaisyys);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hyokkaysEtaisyys);
    }
}
