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
