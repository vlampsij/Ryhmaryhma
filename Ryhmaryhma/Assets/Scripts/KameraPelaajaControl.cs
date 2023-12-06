using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraPelaajaControl : MonoBehaviour
{
    public Transform kameraParent;
    private CharacterController ctrl; //voi olla private koska koodi on pelaajassa komponenttin‰

    public float kameranHerkkyys = 2.0f; // voi olla ett‰ lis‰t‰‰n peliin kohta jossa t‰t‰ muuttaa?

    // rajoittaa kameran pystysuuntaista kulmaa, est‰‰ liian suuren liikkeen ylˆs tai alas
    // samalla maksimi- ja minimikulmat pystysuuntaiselle kierrolle
    float lookXLimit = 60.0f;

    //pelaajan liikkumiseen ja k‰‰ntymiseen
    public float nopeus = 4f;
    public float kaantymisAika = 0.07f;
    float kaantymisNopeus;
    
    private Vector3 rotaatio = Vector3.zero; //liikkumissuunta

    //hypyn muuttuujat
    private float hyppyViive;
    private float yNopeus;
    private float painovoima = 9.81f;
    public float hypynKorkeus = 1f;

    

    private void Start()
    {
        ctrl = GetComponent<CharacterController>(); // hakee CharacterController-komponentin pelaajasta johon koodi liitetty
        
    }

    private void Update()
    {
        Liikunta();
        KameraRotaatio();
    }

    private void Liikunta()
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // M‰‰ritt‰‰ liikkumissuunnan kameran eteenp‰in suuntautuvan akselin perusteella
        Vector3 kameraEteenpain = kameraParent.forward;
        kameraEteenpain.y = 0; // J‰tt‰‰ y-komponentin huomiotta kallistuksen v‰ltt‰miseksi
        Vector3 suunta = (kameraEteenpain * vertical + kameraParent.right * horizontal).normalized * nopeus;

        if (suunta.magnitude >= 0.1f)
        {
            //K‰‰nt‰‰ hahmomallin inputin suuntaan (smoothing ei toimi atm)
            float target = Mathf.Atan2(suunta.x, suunta.z) * Mathf.Rad2Deg;
            float kulma = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref kaantymisNopeus, kaantymisAika);
            transform.rotation = Quaternion.Euler(0f, kulma, 0f);
        }
        //Hyppy
        if (Input.GetButtonDown("Jump"))
        {
            //Jos ollaan oltu maassa hetki sitten (Jos hyppyviiveell‰ on arvo)
            if (hyppyViive > 0)
            {
                hyppyViive = 0;
                //Hypp‰‰
                yNopeus += Mathf.Sqrt(hypynKorkeus * 2 * painovoima);
            }
        }
        //Antaa suunnalle asianmukaisen y-akselin nopeuden, sitten tehd‰‰n Move call
        //Vain yksi Move call voidaan tehd‰ per frame
        suunta.y = yNopeus;
        ctrl.Move(suunta * Time.deltaTime);
    }

    private void KameraRotaatio()
    {
        // P‰ivitt‰‰ kameran rotaatiota hiiren liikkeen perusteella
        rotaatio.y += Input.GetAxis("Mouse X") * kameranHerkkyys;
        rotaatio.x += -Input.GetAxis("Mouse Y") * kameranHerkkyys;
        rotaatio.x = Mathf.Clamp(rotaatio.x, -lookXLimit, lookXLimit);
        kameraParent.localRotation = Quaternion.Euler(rotaatio.x, 0, 0);

        // p‰ivitt‰‰ kameran parentin sijainnin pelaajan sijainnin perusteella
        kameraParent.position = transform.position;

        kameraParent.parent.eulerAngles = new Vector3(0, rotaatio.y, 0);
    }
}