using System.Collections;
using UnityEngine;
using TMPro;

public class Ajastin2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float ajastin = 0f;
    private bool JuokseekoAjastin = false;

    void Start()
    {
        
        StartTimer();
    }

    void Update()
    {
        if (JuokseekoAjastin)
        {
            ajastin += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        JuokseekoAjastin = true;
        StartCoroutine(UpdateTimerCoroutine());
    }

    private IEnumerator UpdateTimerCoroutine()
    {
        while (JuokseekoAjastin)
        {
            yield return new WaitForSeconds(1f); // Update the timer every 1 second.
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        // Format the timer value into minutes and seconds.
        float minutes = Mathf.FloorToInt(ajastin / 60);
        float seconds = Mathf.FloorToInt(ajastin % 60);

        // Update your timer display UI (e.g., TextMeshProUGUI).
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
