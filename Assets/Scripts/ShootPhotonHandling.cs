using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UltimateXR.Core.Components;
using UltimateXR.Mechanics.Weapons;
using UltimateXR.Manipulation;
using UltimateXR.Haptics.Helpers;

public class ShootPhotonHandling : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UxrFirearmWeapon weapon;
    private UxrGrabbableObject grab;
    private UxrGrabber grabber;
    private UxrGrabManager manager;
    private UxrManipulationHapticFeedback haptic;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("abonné shoot local");
        weapon.ProjectileShot += Weapon_ProjectileShot;
      
    }

    private void Weapon_ProjectileShot(int triggerIndex)
    {
        photonView.RPC("ShootUxr", RpcTarget.Others,triggerIndex);
    }


    [PunRPC]
    void ShootUxr(int triggerIndex)
    {
        weapon.TryToShootRound(triggerIndex);
         
    }


}
