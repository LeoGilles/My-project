using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI pcKillScoreText;
    [SerializeField] private TextMeshProUGUI vrKillScoreText;

    private int pcKillScore=0;
    private int vrKillScore=0;

    private int winScore = 3;


    [SerializeField] private TextMeshProUGUI pcCaptureScoreText;
    [SerializeField] private TextMeshProUGUI vrCaptureScoreText;

    private float pcCaptureScore = 0f;
    private float vrCaptureScore = 0f;

    private float winCaptureScore = 20f;
    private bool isGameOver = false;

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
        if (!isGameOver)
        {
            pcKillScore += point;
            photonView.RPC("RPC_ChangePcKillScore", RpcTarget.All, pcKillScore);
        }
        

    }
    [PunRPC]
    private void RPC_ChangePcKillScore(int point)
    {
        pcKillScore = point;
        pcKillScoreText.SetText(pcKillScore.ToString());

        if (pcKillScore >= winScore)
        {
            OnWin("Congratulation PC TEAM WIN");
        }
    }

    
    public void ChangeVrKillScore(int point)
    {
        if (!isGameOver)
        {
            vrKillScore += point;
            photonView.RPC("RPC_ChangeVrKillScore", RpcTarget.All, vrKillScore);
        }
    }

    [PunRPC]
    public void RPC_ChangeVrKillScore(int point)
    {
        vrKillScore = point;
        vrKillScoreText.SetText(vrKillScore.ToString());

        if (vrKillScore >= winScore)
        {
            OnWin("Congratulation VR TEAM WIN");
        }
    }

    public void ChangeCaptureScore(float pcScore, float vrScore)
    {
        if (!isGameOver)
        {
            pcCaptureScore += pcScore;
            vrCaptureScore += vrScore;
            photonView.RPC("RPC_ChangeCaptureScore", RpcTarget.All, pcCaptureScore, vrCaptureScore);
        }
    }

    [PunRPC]
    public void RPC_ChangeCaptureScore(float pcScore, float vrScore)
    {
        pcCaptureScore = pcScore;
        vrCaptureScore = vrScore;

        pcCaptureScoreText.SetText(Mathf.Round(pcCaptureScore).ToString());
        vrCaptureScoreText.SetText(Mathf.Round(vrCaptureScore).ToString());

        if (pcCaptureScore >= winCaptureScore)
        {
            if (pcCaptureScore > vrCaptureScore)
            {
                OnWin("Congratulation PC TEAM WIN");
            }
            else
            {
                OnWin("Congratulation VR TEAM WIN");
            }
        }
        else if (vrCaptureScore >= winCaptureScore)
        {
            OnWin("Congratulation VR TEAM WIN");
        }
    }

    private void OnWin(string winningTeamName)
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log(winningTeamName);
            endText.text = winningTeamName;
            endText.gameObject.SetActive(true);
            StartCoroutine(GoToCredit());
        }
    }

    private IEnumerator GoToCredit()
    {
        yield return new WaitForSeconds(3f);
        endText.gameObject.SetActive(false);
        GameOverManager.Instance.StartCredit();
    }

    public int GetPcKillScore()
    {
        return pcKillScore;
    }

    public int GetVrKillScore()
    {
        return vrKillScore;
    }
}
