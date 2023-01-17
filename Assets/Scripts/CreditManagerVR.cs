using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class CreditManagerVR : MonoBehaviour
{

    [SerializeField] private GameObject htcPrefab;

    private GameObject LocalVR;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject canvasVR;
    [SerializeField] private GameObject canvasPC;
    [SerializeField] private InputSystemUIInputModule inputsystem;

    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            LocalVR = Instantiate(htcPrefab, new Vector3(0f, 0f, 0f), new Quaternion());
            mainCamera.enabled = false;
            canvasVR.SetActive(true);
            canvasPC.SetActive(false);
        }
        else
        {
            inputsystem.enabled = true;
        }
    }
}
