using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UltimateXR.UI.Helpers.Keyboard;
using TMPro;
/// <summary>
/// Player name input field. Let the user input his name, will appear above the player in the game.
/// </summary>
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants

    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";
    [SerializeField]
    private TMP_InputField inputFieldPC;
    [SerializeField]
    private Text inputFieldVR;
    [SerializeField]
    private UxrKeyboardUI keyboard;
    #endregion

    #region MonoBehaviour CallBacks

    /// &lt;summary&gt;
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// &lt;/summary&gt;
    void Start()
    {

        string defaultName = string.Empty;
        if (inputFieldVR != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                keyboard.AddConsoleContent(defaultName);            
            }
        }
        else
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputFieldPC.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region Public Methods

    /// &lt;summary&gt;
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// &lt;/summary&gt;
    /// &lt;param name="value"&gt;The name of the Player&lt;/param&gt;
    public void SetPlayerName(Text text)
    {
        // #Important
        if (string.IsNullOrEmpty(text.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = text.text;

        PlayerPrefs.SetString(playerNamePrefKey, text.text);
    }
    public void SetPlayerNamePC(TMP_InputField text)
    {
        // #Important
        if (string.IsNullOrEmpty(text.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = text.text;

        PlayerPrefs.SetString(playerNamePrefKey, text.text);
    }
    #endregion
}

