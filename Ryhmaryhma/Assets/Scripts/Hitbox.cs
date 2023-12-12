using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject hitbox;
    public AudioSource hamis;
    //public AudioClip hooki;

    private void Awake()
    {
        hitbox.GetComponent<Collider>().enabled = false;
    }
    public void AktivoiHitbox()
    {
        hamis.PlayOneShot(hamis.clip, 0.2f);
        hitbox.GetComponent<Collider>().enabled = true;
    }
    public void DeaktivoiHitbox()
    {
        hitbox.GetComponent<Collider>().enabled = false;
    }
}
