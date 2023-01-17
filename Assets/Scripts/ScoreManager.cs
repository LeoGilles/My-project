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

    private int winScore = 3;

    [SerializeField] private TextMeshProUGUI endText;

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

        if (pcKillScore >= winScore)
        {
            OnWin("Congratulation PC TEAM WIN");
        }

    }

    public void ChangeVrKillScore(int point)
    {
        vrKillScore += point;
        vrKillScoreText.SetText(vrKillScore.ToString());

        if(vrKillScore >= winScore)
        {
            OnWin("Congratulation VR TEAM WIN");
        }
    }

    private void OnWin(string winningTeamName)
    {
        Debug.Log(winningTeamName);
        endText.text = winningTeamName;
        endText.gameObject.SetActive(true);
        StartCoroutine(GoToCredit());
    }

    private IEnumerator GoToCredit()
    {
        yield return new WaitForSeconds(3f);
        endText.gameObject.SetActive(false);
        GameOverManager.Instance.StartCredit();
    }
}
