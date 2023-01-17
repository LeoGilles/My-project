using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plusieurs instances de GameOverManager");
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCredit();
        }
    }

    private void Reset()
    {
        foreach(GameObject perso in GameManager.Instance.playerPrefab)
        {
            Destroy(perso);
        }
        foreach(GameObject prefab in GameManager.Instance.pcPlayer)
        {
            if (prefab != null)
            {
                Destroy(prefab);
            }
        }
        foreach (GameObject prefab in GameManager.Instance.vrPlayer)
        {
            if (prefab != null)
            {
                Destroy(prefab);
            }

        }

    }

    public void StartCredit()
    {
        Reset();
        SceneManager.LoadScene("Credits");
    }
}
