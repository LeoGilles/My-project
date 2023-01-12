using System.Collections;
using System.Collections.Generic;
using UltimateXR.Avatar;
using UltimateXR.Core;
using UltimateXR.Devices;
using UltimateXR.Devices.Integrations.Oculus;
using UltimateXR.Manipulation;
using UnityEngine;

public class MiniMapControllerActivation : MonoBehaviour
{
    [SerializeField]
    private UxrAvatar avatar;
    [SerializeField]
    private UxrGrabbableObject grabObj;
    [SerializeField]
    private GameObject CanvasVR;
    bool PressingLeft;
    bool PressingRight;
    bool GrabLeft;
    bool GrabRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        GrabLeft = UxrGrabManager.Instance.GetObjectBeingGrabbed(avatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject);
        GrabRight = UxrGrabManager.Instance.GetObjectBeingGrabbed(avatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject2);
        PressingLeft = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Button1, UxrButtonEventType.Pressing);
        PressingRight = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Right, UxrInputButtons.Button1, UxrButtonEventType.Pressing);

        if(GrabLeft && grabbableObject == grabObj && PressingLeft)
        {
            CanvasVR.SetActive(true);
        }
        else if (GrabRight && grabbableObject2 == grabObj && PressingRight)
        {
            CanvasVR.SetActive(true);
        }
        else
        {
            CanvasVR.SetActive(false);
        }
    }
}
