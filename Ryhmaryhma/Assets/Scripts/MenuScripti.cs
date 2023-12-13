using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripti : MonoBehaviour
{
    public AudioSource aanet;
    public AudioClip paallaAani;
    public AudioClip klikkausAani;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartBtnClicked()
    {
        SceneManager.LoadScene("Taso1");
    }
    public void TasotBtnClicked()
    {
        SceneManager.LoadScene("TasoValikko");
    }

    public void HuippuajatClicked()
    {
        SceneManager.LoadScene("Huippuajat");
    }

    public void Taso1BtnClicked()
    {
        SceneManager.LoadScene("Taso1");
    }

    public void EndBtnClicked()
    {
        Application.Quit();
    }

    public void PaallaNapin()
    {
        aanet.PlayOneShot(paallaAani);
    }
    public void KlikkaaNapin()
    {
        aanet.PlayOneShot(klikkausAani);
    }
}
