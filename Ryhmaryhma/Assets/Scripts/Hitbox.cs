using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject hitbox;

    private void Awake()
    {
        hitbox.GetComponent<Collider>().enabled = false;
    }
    public void AktivoiHitbox()
    {
        hitbox.GetComponent<Collider>().enabled = true;
    }
    public void DeaktivoiHitbox()
    {
        hitbox.GetComponent<Collider>().enabled = false;
    }
}
