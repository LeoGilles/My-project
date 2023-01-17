using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pcKillScoreText;
    [SerializeField] private TextMeshProUGUI vrKillScoreText;

    private int pcKillScore=0;
    private int vrKillScore=0;

    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de ScoreManager");
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangePcKillScore(int point)
    {
        pcKillScore += point;
        pcKillScoreText.SetText(pcKillScore.ToString());
    }

    public void ChangeVrKillScore(int point)
    {
        vrKillScore += point;
        vrKillScoreText.SetText(pcKillScore.ToString());
    }
}
