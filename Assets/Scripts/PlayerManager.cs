using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;
using UltimateXR.Avatar;
using UltimateXR.Avatar.Controllers;
using UltimateXR.Core;
using UnityEngine.InputSystem.XR;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    [SerializeField]
    private CinemachineBrain cameraController;
    [SerializeField]
    private Camera cameraObj;
    [SerializeField]
    private CinemachineVirtualCamera cameraFollow;
    [SerializeField]
    private CinemachineVirtualCamera cameraAim;
    [SerializeField]
    private ThirdPersonShooter thirdPersonShooter;
    [SerializeField]
    private ThirdPersonController thirdPersonController;
    [SerializeField]
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private AudioListener audio;
    [SerializeField]
    private GrabManager grabManager;
    [SerializeField]
    private UxrAvatar VRAvatar;
    [SerializeField]
    private UxrStandardAvatarController VRController;
    [SerializeField]
    private Camera cameraMap;
    [SerializeField]
    private GameObject CameraControllerVR;
    [SerializeField]
    private MonoBehaviour inputVR;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /* if (stream.IsWriting)
         {
             // We own this player: send the others our data
             stream.SendNext(starterAssetsInputs.shoot);
             stream.SendNext(bulletTarget.health);
         }
         else
         {
             // Network player, receive data
             this.starterAssetsInputs.shoot = (bool)stream.ReceiveNext();
             this.bulletTarget.health = (int)stream.ReceiveNext();
         }*/
      /*  if(VRController != null && !photonView.IsMine)
        {
            Debug.Log("bodyreload");
            VRController.SolveBodyIK();
        }*/
      
    }
  
    // Start is called before the first frame update
    void Awake()
    {
        
        if (thirdPersonShooter != null)
        {
            thirdPersonShooter.enabled = photonView.IsMine;
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = photonView.IsMine;
        }
        if (playerInput != null)
        {
            playerInput.enabled = photonView.IsMine;
        }
        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.enabled = photonView.IsMine;
        }
        if (audio != null)
        {
            audio.enabled = photonView.IsMine;
        }
        if(cameraController != null)
        {
            cameraController.enabled = photonView.IsMine;
        }
        if (cameraObj != null)
        {
            cameraObj.enabled = photonView.IsMine;
        }
        if (cameraFollow != null)
        {
            cameraFollow.enabled = photonView.IsMine;
        }
        if (cameraAim != null)
        {
            cameraAim.enabled = photonView.IsMine;
        }
       /* if (grabManager != null)
        {
            grabManager.enabled = photonView.IsMine;
        }*/
       if(VRAvatar != null)
        {
            if(photonView.IsMine)
            {
                VRAvatar.AvatarMode = UxrAvatarMode.Local;
            }
            else
            {
                VRAvatar.AvatarMode = UxrAvatarMode.UpdateExternally;
            }
        }
        if (cameraMap != null)
        {
            cameraMap.enabled = photonView.IsMine;
        }
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

        //this.starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        //this.bulletTarget = GetComponent<BulletTarget>();
    }
    private void Start()
    {
        if (CameraControllerVR != null)
        {
            CameraControllerVR.SetActive(true);
        }
        if(inputVR != null)
        {
            inputVR.enabled = photonView.IsMine;
        }
    }
}
