using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UltimateXR.Avatar.Controllers;
using UnityEngine.InputSystem.XR;

public class RefreshBodyIK : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UxrStandardAvatarController VRController;
    [SerializeField]
    private TrackedPoseDriver inputVR;

    // Start is called before the first frame update
    void Start()
    {
        inputVR = gameObject.GetComponent<TrackedPoseDriver>();
        Debug.Log("inputVR "+ inputVR);
        if (inputVR != null && !photonView.IsMine)
        {
            inputVR.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (VRController != null && !photonView.IsMine)
        {
            Debug.Log("boyreload");
            VRController.SolveBodyIK();
        }
    }
}
