using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject crossAir;
    [SerializeField]
    private GameObject Player;

    private StarterAssetsInputs starterAssetsInputs;
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = Player.GetComponent<StarterAssetsInputs>();
        menuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (starterAssetsInputs.menu)
        {
            menuPanel.SetActive(true);
            crossAir.SetActive(false);
            Cursor.visible = true;
            starterAssetsInputs.cursorLocked = false;
        }
        else
        {
            menuPanel.SetActive(false);
            crossAir.SetActive(true);
            Cursor.visible = false;
            starterAssetsInputs.cursorLocked = true;
        }
    }
}
