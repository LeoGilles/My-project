using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanelPC;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabelPC;
    [SerializeField]
    private GameObject controlPanelVR;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabelVR;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject htcPrefab;
    [SerializeField]
    private AudioListener audio;
    [SerializeField]
    private GameObject canvasVR;
    [SerializeField]
    private GameObject canvasPC;
    [SerializeField]
    private InputSystemUIInputModule inputsystem;

    private GameObject LocalVR;
    #endregion

    #region Private Fields

    /// &lt;summary&gt;
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// &lt;/summary&gt;
    string gameVersion = "1";
    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    #endregion

    #region MonoBehaviour CallBacks

    /// &lt;summary&gt;
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// &lt;/summary&gt;
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        if(UnityEngine.XR.XRSettings.isDeviceActive)
        {
            LocalVR = Instantiate(htcPrefab,new Vector3(0f,0f,0f),new Quaternion());           
            mainCamera.enabled = false;
            audio.enabled = false;
            canvasVR.SetActive(true);
            canvasPC.SetActive(false);
        }
        else
        {
            inputsystem.enabled = true;
        }
    }

    private void Start()
    {
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            progressLabelVR.SetActive(false);
            controlPanelVR.SetActive(true);
        }
        else
        {
            progressLabelPC.SetActive(false);
            controlPanelPC.SetActive(true);
        }
    }
    #endregion


    #region Public Methods

    /// &lt;summary&gt;
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// &lt;/summary&gt;
    public void Connect()
    {
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            progressLabelVR.SetActive(true);
            controlPanelVR.SetActive(false);
        }
        else
        {
            progressLabelPC.SetActive(true);
            controlPanelPC.SetActive(false);
        }
        if (PhotonNetwork.IsConnected)
        {
            if (UnityEngine.XR.XRSettings.isDeviceActive)
            {
                Destroy(LocalVR);
            }
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion
    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            progressLabelVR.SetActive(false);
            controlPanelVR.SetActive(true);
        }
        else
        {
            progressLabelPC.SetActive(false);
            controlPanelPC.SetActive(true);
        }
        isConnecting = false;
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Playground' ");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Playground");

        }
    }
    #endregion
}


