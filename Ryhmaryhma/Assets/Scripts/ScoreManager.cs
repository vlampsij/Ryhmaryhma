using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    void Awake()
    {
        string json = PlayerPrefs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);

        if (sd == null || sd.scores == null)
        { 
            sd = new ScoreData();
        }
    }

    public void ClearScore()
    {
        sd.scores.Clear();
        SaveScore();
    }
    //void TyhjennaScoret()
    //{
    //    ScoreManager scoreManager = GetComponent<ScoreManager>();

    //    if (scoreManager != null)
    //    {
    //        scoreManager.ClearScore();
    //    }
    //    else
    //    {
    //        Debug.LogError("ScoreManager reference is missing.");
    //    }
    //}
    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score); 
    }

    public void AddScore(int aika)
    {
        sd.scores.Add(new Score(aika));
        SaveScore();
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        string json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }
}
