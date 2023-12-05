using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Liikkuminen : MonoBehaviour
{
    public CharacterController ctrl;

    public float nopeus;
    public float kaantymisAika;
    float kaantymisNopeus;

    private float hyppyViive;
    private float yNopeus;
    private float painovoima = 9.81f;
    public float hypynKorkeus;

    // Update is called once per frame
    void Update()
    {
        bool maassa = ctrl.isGrounded;

        if (maassa)
        {
            //viive, jotta pelaaja voi hyp‰t‰ rampeilla alas tultaessa
            hyppyViive = 0.2f;
        }
        if (hyppyViive > 0)
        {
            hyppyViive -= Time.deltaTime;
        }
        //Maahan osuessa tappaa y-akselin liikkeen
        if (maassa && yNopeus < 0)
        {
            yNopeus = 0f;
        }
        //painovoima
        yNopeus -= painovoima * Time.deltaTime;

        //x-ja y-akselin input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 suunta = new Vector3(horizontal*nopeus, 0, vertical*nopeus);

        if (suunta.magnitude >= 0.1f)
        {
            //K‰‰nt‰‰ hahmomallin inputin suuntaan (smoothing ei toimi atm)
            float target = Mathf.Atan2(suunta.x, suunta.z) * Mathf.Rad2Deg;
            float kulma = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref kaantymisNopeus, kaantymisAika);
            transform.rotation = Quaternion.Euler(0f, kulma, 0f);

            //Liikkuminen
            gameObject.transform.forward = suunta;
        }
        //Hyppy
        if (Input.GetButtonDown("Jump"))
        {
            //Jos ollaan oltu maassa hetki sitten (Jos hyppyviiveell‰ on arvo)
            if (hyppyViive > 0)
            {
                print("hyp‰tty");
                hyppyViive = 0;
                //Hypp‰‰
                yNopeus += Mathf.Sqrt(hypynKorkeus * 2 * painovoima);
            }
        }
        //Antaa suunnalle asianmukaisen y-akselin nopeuden, sitten tehd‰‰n Move call
        //Vain yksi Move call voidaan tehd‰ per frame
        suunta.y = yNopeus;
        ctrl.Move(suunta*Time.deltaTime);
    }
}
