using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class Ajastin : MonoBehaviour {

    private float ajanKesto = 3f * 60f;

    [SerializeField]
    private bool ajanLasku = true;

    private float ajastin;

    [SerializeField]
    private TextMeshProUGUI ekaMinuutti;
    [SerializeField]
    private TextMeshProUGUI tokaMinuutti;
    [SerializeField]
    private TextMeshProUGUI jakaja;
    [SerializeField]
    private TextMeshProUGUI ekaSekunti;
    [SerializeField]
    private TextMeshProUGUI tokaSekunti;

    private float flashAjastin;
    private float flashKesto;

    public ScoreManager scoreManager;

    void Start()
    {
        ResetTimer();
        scoreManager = FindObjectOfType<ScoreManager>(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ajanLasku && ajastin > 0)
        {
            ajastin -= Time.deltaTime;
            UpdateTimerDisplay(ajastin);
        } else if (!ajanLasku && ajastin < ajanKesto)
        {
            ajastin += Time.deltaTime;
            UpdateTimerDisplay(ajastin);
        }
        else if (ajanLasku && ajastin <= 0)
        {
            LisaaHuippuaika(ajastin);
        } else if (!ajanLasku && ajastin >= ajanKesto)
        {
            LisaaHuippuaika(ajastin);
        }
        else
        {
           Flash();
        }
    }

    private void LisaaHuippuaika(float aika)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore((int)aika);
        }
        else 
        {
            Debug.LogError("Scoremanager referenssi kateissa ajastin scriptissä");
        }
    }
    private void ResetTimer()
    {
        if(ajanLasku)
        {
            ajastin = ajanKesto;
        } else
        {
            ajastin = 0;
        }
        SetTextDisplay(true);
    }
    private void UpdateTimerDisplay(float time)
    {
        float minuutit = Mathf.FloorToInt(time / 60);
        float sekunnit = Mathf.FloorToInt(time % 60);

        string tamaAika = string.Format("{00:00}{1:00}", minuutit, sekunnit);
        ekaMinuutti.text = tamaAika[0].ToString();
        tokaMinuutti.text = tamaAika[1].ToString();
        ekaSekunti.text = tamaAika[2].ToString();
        tokaSekunti.text = tamaAika[3].ToString();
    }
    private void Flash()
    {
        if(ajanLasku && ajastin != 0) 
        {
            ajastin = 0;
            UpdateTimerDisplay(ajastin);
        }
        if(!ajanLasku && ajastin != ajanKesto)
        {
            ajastin = 0;
            UpdateTimerDisplay(ajastin);
        }
        if(flashAjastin <= 0)
        {
            flashAjastin = flashKesto;
        } else if (flashAjastin >= flashKesto / 2)
        {
            flashAjastin -= Time.deltaTime;
            SetTextDisplay(true);
        }
    }
    private void SetTextDisplay(bool enabled)
    {
        ekaMinuutti.enabled = enabled;
        tokaMinuutti.enabled = enabled;
        ekaSekunti.enabled = enabled;
        tokaSekunti.enabled = enabled;
        jakaja.enabled = enabled;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("maaalii");
            PelinLoppu();
        }
    }
    public void PelinLoppu()
    {
        LisaaHuippuaika(ajastin);
    }
}
