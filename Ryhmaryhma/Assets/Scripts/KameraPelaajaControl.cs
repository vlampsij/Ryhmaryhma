using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KameraPelaajaControl : MonoBehaviour
{
    public Transform kameraParent;
    private CharacterController ctrl; //voi olla private koska koodi on pelaajassa komponenttin‰
    private Animator anim;
    Ajastin ajastinSkripti;
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
    private float ilmanvastuksenKerroin = 0.3f;

    private AudioSource aani;
    //public AudioSource hyppy;
    public AudioClip hyppyAani;

    //Damagemuuttujat
    private float hidastus = 1;




    private void Start()
    {
        ctrl = GetComponent<CharacterController>(); // hakee CharacterController-komponentin pelaajasta johon koodi liitetty
        anim = gameObject.GetComponentInChildren<Animator>();

        aani = gameObject.GetComponent<AudioSource>();
        ajastinSkripti = gameObject.GetComponent<Ajastin>();
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
            hyppyViive = 0.1f;
            
            anim.SetBool("Laskeutuminen", true);
            anim.SetBool("Hyppy", false);
            anim.SetBool("Tippuminen", false);
        }
        else
        {
            anim.SetBool("Tippuminen", true);
            anim.SetBool("Laskeutuminen", false);
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
        yNopeus -= painovoima * Time.deltaTime * 2;

        //x-ja y-akselin input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // M‰‰ritt‰‰ liikkumissuunnan kameran eteenp‰in suuntautuvan akselin perusteella
        Vector3 kameraEteenpain = kameraParent.forward;
        kameraEteenpain.y = 0; // J‰tt‰‰ y-komponentin huomiotta kallistuksen v‰ltt‰miseksi
        Vector3 suunta = (kameraEteenpain * vertical + kameraParent.right * horizontal).normalized * nopeus * hidastus;

        if (suunta.magnitude >= 0.1f)
        {
            //K‰‰nt‰‰ hahmomallin inputin suuntaan
            float target = Mathf.Atan2(suunta.x, suunta.z) * Mathf.Rad2Deg;
            float kulma = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref kaantymisNopeus, kaantymisAika);
            transform.rotation = Quaternion.Euler(0f, kulma, 0f);
        }
        bool liikkuu;

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetInteger("AnimationPar", 1);
            liikkuu = true;
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
            liikkuu = false;
        }

        if (liikkuu && !aani.isPlaying && !Input.GetButtonDown("Jump") && maassa)
        {
            
            aani.Play();
        }
        else if (!liikkuu)
        {
            aani.Stop();
        }
        


        //Hyppy
        if (Input.GetButtonDown("Jump"))
        { 
            //Jos ollaan oltu maassa hetki sitten (Jos hyppyviiveell‰ on arvo)
            if (hyppyViive > 0)
            {
                hyppyViive = 0;
                
                aani.PlayOneShot(hyppyAani);
                anim.SetBool("Hyppy", true);

                //Hypp‰‰
                yNopeus += Mathf.Sqrt(hypynKorkeus * 2 * painovoima);

                //lis‰‰ realistisuutta hyppyyn eli v‰hent‰‰ ilmanvastuksen vaikutusta y-nopeuteen, ilmanvastuksen kerroin m‰‰r‰‰, kuinka voimakkaasti ilmanvastus vaikuttaa liikkeen hidastumiseen.
                yNopeus -= ilmanvastuksenKerroin* yNopeus * Time.deltaTime;

                
            }
        }
        //Antaa suunnalle asianmukaisen y-akselin nopeuden, sitten tehd‰‰n Move call
        //Vain yksi Move call voidaan tehd‰ per frame
        suunta.y = yNopeus;
        ctrl.Move(suunta * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Pause();
        //}
    }

    private void KameraRotaatio()
    {
        // P‰ivitt‰‰ kameran rotaatiota hiiren liikkeen perusteella
        rotaatio.y += Input.GetAxis("Mouse X") * kameranHerkkyys;
        rotaatio.x += -Input.GetAxis("Mouse Y") * kameranHerkkyys;
        rotaatio.x = Mathf.Clamp(rotaatio.x, -lookXLimit, lookXLimit);
        kameraParent.localRotation = Quaternion.Euler(rotaatio.x, rotaatio.y, 0);

        // p‰ivitt‰‰ kameran parentin sijainnin pelaajan sijainnin perusteella
        kameraParent.position = transform.position;

        //kameraParent.parent.eulerAngles = new Vector3(0, rotaatio.y, 0);
    }
    private void Menuun()
    {
        //Taso1 = 3, aloitusmenu = 0
        SceneManager.LoadScene(0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hitbox")
        {
            OtaDamagee();
        }
        if(other.gameObject.tag == "Finish")
        {
            Voita();

            ajastinSkripti.PelinLoppu();
        }
    }
    public void OtaDamagee()
    {
        //Osuman ottaminen hidastaa pelaajaa
        StartCoroutine(Hidastus());
    }
    IEnumerator Hidastus()
    {
        for (hidastus = 0.1f; hidastus < 1; hidastus += 0.003f)
        {
            yield return null;
        }
    }
    IEnumerator LoppuAjastin(int s)
    {
        yield return new WaitForSecondsRealtime(s);
        SceneManager.LoadScene(2);
    }
    public void Voita()
    {
        nopeus = 0f;
        anim.SetTrigger("Voitto");
        StartCoroutine(LoppuAjastin(5));
    }
}