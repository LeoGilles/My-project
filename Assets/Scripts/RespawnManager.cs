using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{

    [SerializeField] private Transform[] pcRespawn;
    [SerializeField] private Transform[] vrRespawn;


    public static RespawnManager instance;
    void Start()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plusieurs instance de RespawnManager");
        }
        else
        {
            instance = this;
        }
    }

    public Vector3 OnPcPlayerDeath()
    {
        ScoreManager.Instance.ChangeVrKillScore(1);
        int index = Random.Range(0, pcRespawn.Length);
        return pcRespawn[index].position;
    }

    public Vector3 OnVrPlayerDeath()
    {
        ScoreManager.Instance.ChangePcKillScore(1);
        int index = Random.Range(0, vrRespawn.Length);
        return vrRespawn[index].position;
    }

}
