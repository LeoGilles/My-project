using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Reset();
            SceneManager.LoadScene("Credits");
        }
    }

    private void Reset()
    {
        foreach(GameObject perso in GameManager.Instance.playerPrefab)
        {
            Destroy(perso);
        }
    }
}
