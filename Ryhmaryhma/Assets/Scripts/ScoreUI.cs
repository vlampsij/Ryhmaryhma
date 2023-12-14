using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;
    void Start()
    {

        var scores = scoreManager.GetHighScores().OrderByDescending(x => x.score).ToArray();

        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.playerScore.text = scores[i].score.ToString();
        }
    }

    public void Poistu()
    {
        SceneManager.LoadScene("Aloitus");
    }


}
