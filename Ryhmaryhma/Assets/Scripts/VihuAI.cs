using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VihuAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform pelaaja;
    public Rigidbody rb;

    public LayerMask tamaOnMaata, tamaOnPelaaja;

    //Patrol
    public Vector3 kohde;
    bool onKohde;
    public float kohdeEtaisyys;

    //Hyökkäys
    public float attackRate;
    bool hyokatty;

    public float nakoEtaisyys, hyokkaysEtaisyys;
    public bool pelaajaNakopiirissa, pelaajaKantamalla, aggro;

    private void Awake()
    {
        pelaaja = GameObject.Find("PlayerPlacehold").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        pelaajaNakopiirissa = Physics.CheckSphere(transform.position, nakoEtaisyys, tamaOnPelaaja);
        pelaajaKantamalla = Physics.CheckSphere(transform.position, hyokkaysEtaisyys, tamaOnPelaaja);

        if (!pelaajaNakopiirissa && !pelaajaKantamalla && !aggro)
        {
            Patrol();
        }
        else if (pelaajaNakopiirissa && !pelaajaKantamalla || aggro)
        {
            AjaPelaajaa();
            aggro = true;
            if (pelaajaNakopiirissa && pelaajaKantamalla)
            {
                Hyokkaa();
            }
        }

    }

    private void Patrol()
    {
        if (!onKohde)
        {
            HaeKohde();
        }
        if (onKohde)
        {
            agent.SetDestination(kohde);
        }
        Vector3 etaisyysKohteeseen = transform.position - kohde;

        if (etaisyysKohteeseen.magnitude < 1f)
        {
            onKohde = false;
        }
    }
    private void HaeKohde()
    {
        float randomZ = Random.Range(-kohdeEtaisyys, kohdeEtaisyys);
        float randomX = Random.Range(-kohdeEtaisyys, kohdeEtaisyys);

        kohde = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

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
        if (!hyokatty)
        {
            rb.AddForce(transform.forward, ForceMode.Impulse);
            

            hyokatty = true;
            
            Invoke("HyokkaysReset", attackRate);
        }
    }
    private void HyokkaysReset()
    {
        hyokatty = false;
    }
    private void TuhoaVihu()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nakoEtaisyys);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hyokkaysEtaisyys);
    }
}
