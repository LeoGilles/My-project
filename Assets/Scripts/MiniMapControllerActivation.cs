using System.Collections;
using System.Collections.Generic;
using UltimateXR.Avatar;
using UltimateXR.Core;
using UltimateXR.Devices;
using UltimateXR.Devices.Integrations.Oculus;
using UltimateXR.Manipulation;
using UnityEngine;
using Photon.Pun;
public class MiniMapControllerActivation : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UxrAvatar avatar;
    [SerializeField]
    private UxrGrabbableObject grabObj;
    [SerializeField]
    private GameObject CanvasVR;
    [SerializeField]
    private GameObject Shield;
    bool LeftMap;
    bool RightMap;
    bool LeftShield;
    bool RightShield;
    bool GrabLeft;
    bool GrabRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if(photonView.IsMine)
        {

       
        GrabLeft = UxrGrabManager.Instance.GetObjectBeingGrabbed(avatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject);
        GrabRight = UxrGrabManager.Instance.GetObjectBeingGrabbed(avatar, UxrHandSide.Right, out UxrGrabbableObject grabbableObject2);
        LeftMap = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Button1, UxrButtonEventType.Pressing);
        RightMap = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Right, UxrInputButtons.Button1, UxrButtonEventType.Pressing);
        LeftShield = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Button2, UxrButtonEventType.Pressing);
        RightShield = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Right, UxrInputButtons.Button2, UxrButtonEventType.Pressing);

        if (GrabLeft && grabbableObject == grabObj && LeftMap)
        {
            CanvasVR.SetActive(true);
        }
        else if (GrabRight && grabbableObject2 == grabObj && RightMap)
        {
            CanvasVR.SetActive(true);
        }
        else
        {
            CanvasVR.SetActive(false);
        }

        if (GrabLeft && grabbableObject == grabObj && LeftShield)
        {
            Shield.SetActive(true);
        }
        else if (GrabRight && grabbableObject2 == grabObj && RightShield)
        {
            Shield.SetActive(true);
        }
        else
        {
            Shield.SetActive(false);
        }
        }
    }
}
