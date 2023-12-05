using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Liikkuminen : MonoBehaviour
{
    public CharacterController ctrl;

    public float nopeus;
    public float kaantymisAika;
    float kaantymisNopeus;

    private float painovoima = 9.81f;
    public float hypynKorkeus;

    // Update is called once per frame
    void Update()
    {

        //x-ja y-akselin input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 suunta = new Vector3(horizontal, 0f, vertical).normalized;

        if (suunta.magnitude >= 0.1f)
        {
            //K‰‰nt‰‰ hahmomallin inputin suuntaan
            float target = Mathf.Atan2(suunta.x, suunta.z) * Mathf.Rad2Deg;
            float kulma = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref kaantymisNopeus, kaantymisAika);
            transform.rotation = Quaternion.Euler(0f, kulma, 0f);

            //Liikkuminen
            ctrl.Move(suunta * nopeus * Time.deltaTime);

            //Hyppy
            if (Input.GetButtonDown("Jump") && ctrl.isGrounded)
            {

            }
        }

    }
}
