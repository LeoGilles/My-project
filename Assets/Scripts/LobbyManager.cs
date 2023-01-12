using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject pcPrefab;
    [Tooltip("The prefab to use for representing the VRplayer")]
    public GameObject vrPrefab;

    [SerializeField] private GameObject startText;


    private List<GameObject> players = new List<GameObject>();

    private float inTimer=0f;
    private bool isInside = false;

    #region Photon Callbacks

    public enum UserDeviceType
    {
        HTC,
        PC
    }
    /// &lt;summary&gt;
    /// Called when the local player left the room. We need to load the launcher scene.
    /// &lt;/summary&gt;
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }


    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }


    }
    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods
    void Start()
    {
        Instance = this;
        if (pcPrefab == null || vrPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Prefabs Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (GetDeviceUsed() == "HTCPrefab")
                {
                    players.Add(PhotonNetwork.Instantiate("HTCPrefab", new Vector3(0f, 0.2f, 0f), new Quaternion(0, 1, 0, 0), 0));
                }
                else
                {
                    players.Add(PhotonNetwork.Instantiate("pcPrefab", new Vector3(-30f, 0.2f, 0f), Quaternion.identity, 0));
                }

            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }


    private void Update()
    {
        if (isInside)
        {
            inTimer+= Time.deltaTime;
        }
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);


        foreach(GameObject player in players)
        {
            Destroy(player);
        }
        PhotonNetwork.LoadLevel("Playground");
    }

    void LoadNextArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);


        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        PhotonNetwork.LoadLevel("MainMapScene");
    }
    static string GetDeviceUsed()
    {
        // Server execution

        string deviceUsed = "auto";

        switch (deviceUsed)
        {
            case "htc":
                // Si l'app config demande du HTC mais que le casque n'est pas branché
                Debug.LogWarning("AppConfig asked for HTC, but not active, so use PC version");
                return UnityEngine.XR.XRSettings.isDeviceActive ? "HTCPrefab" : "pcPrefab";

            case "pc":
                return "pcPrefab";

            default: // "auto" and others
                return UnityEngine.XR.XRSettings.isDeviceActive ? "HTCPrefab" : "pcPrefab";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {

            if (PhotonNetwork.IsMasterClient)
            {
                isInside = true;
                
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            startText.transform.rotation = other.transform.rotation;

            if (PhotonNetwork.IsMasterClient && inTimer > 3f)
            {
                LoadNextArena();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTimer = 0f;
            if (PhotonNetwork.IsMasterClient)
            {
                isInside = false;

            }
        }
    }

    #endregion
}


