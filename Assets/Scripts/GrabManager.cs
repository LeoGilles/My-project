using System.Collections;
using System.Collections.Generic;
using UltimateXR.Manipulation;
using UnityEngine;
using Photon.Pun;
using UltimateXR.Core.Components;
using UltimateXR.Mechanics.Weapons;
using UltimateXR.Avatar;
using UltimateXR.Haptics.Helpers;

public class GrabManager : MonoBehaviourPunCallbacks
{
    public UxrGrabber grabberLeft;
    public UxrGrabber grabberRight;
    public UxrAvatar VRAvatar;
    // Start is called before the first frame update
    void Awake()
    {
        UxrGrabManager.Instance.ObjectPlacing += GrabManager_ObjectPlacing;
        UxrGrabManager.Instance.ObjectGrabbing += GrabManager_ObjectGrabbing;
        UxrGrabManager.Instance.ObjectReleasing += GrabManager_ObjectReleasing;
    }

    private void GrabManager_ObjectPlacing(object sender, UxrManipulationEventArgs e)
    {
        PhotonView obj = e.GrabbableObject.gameObject.GetComponent<PhotonView>();
        e.GrabbableObject.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = false;
        PhotonView anchor = e.GrabbableAnchor.gameObject.GetComponent<PhotonView>();
        if (e.PlacementType == UxrPlacementType.Immediate)
        {
            photonView.RPC("PlaceUxr", RpcTarget.OthersBuffered, obj.ViewID,0,anchor.ViewID);
        }
        else
        {
            photonView.RPC("PlaceUxr", RpcTarget.OthersBuffered, obj.ViewID,1, anchor.ViewID);
        }  
    }

    private void GrabManager_ObjectReleasing(object sender, UxrManipulationEventArgs e)
    {
        PhotonView obj = e.GrabbableObject.gameObject.GetComponent<PhotonView>();
        e.GrabbableObject.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = false;

        if (e.Grabber == grabberLeft)
        {
            photonView.RPC("ReleaseUxr", RpcTarget.Others, "left", obj.ViewID);
        }
        else if (e.Grabber == grabberRight)
        {
            photonView.RPC("ReleaseUxr", RpcTarget.Others, "right", obj.ViewID);
        }       
        LeaveOwner(obj);
    }

    private void GrabManager_ObjectGrabbing(object sender, UxrManipulationEventArgs e)
    {
        PhotonView obj = e.GrabbableObject.gameObject.GetComponent<PhotonView>();
        
        e.GrabbableObject.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = photonView.IsMine;
        ChangeOwner(obj);
        if (e.Grabber == grabberLeft)
        {
            photonView.RPC("GrabUxr", RpcTarget.Others, "left", obj.ViewID, e.GrabPointIndex);
        }
        else if(e.Grabber == grabberRight)
        {
            photonView.RPC("GrabUxr", RpcTarget.Others, "right", obj.ViewID, e.GrabPointIndex);
        }
    }

    public void ChangeOwner(PhotonView view)
    {
        Debug.Log("obj :" + view.ViewID);
        view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
    }
    public void LeaveOwner(PhotonView view)
    {
        Debug.Log("owner 0");
        view.TransferOwnership(0);
    }

    [PunRPC]
    void GrabUxr(string hand, int grabObjId , int index)
    {
        UxrGrabber tempGrabber; 
        if(hand == "left")
        {
            tempGrabber = grabberLeft;
        }
        else
        {
            tempGrabber = grabberRight;
        }
        UxrGrabbableObject tempgrabObj = GetUxrGrabbableObjectByPhotonId(grabObjId);

        tempgrabObj.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = photonView.IsMine;
        UxrGrabManager.Instance.GrabObject(tempGrabber, tempgrabObj, index, false);
    }

    [PunRPC]
    void ReleaseUxr(string hand, int grabObjId)
    {
        UxrGrabber tempGrabber;
        if (hand == "left")
        {
            tempGrabber = grabberLeft;
        }
        else
        {
            tempGrabber = grabberRight;
        }

        UxrGrabbableObject tempgrabObj = GetUxrGrabbableObjectByPhotonId(grabObjId);
        tempgrabObj.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = false;

        UxrGrabManager.Instance.ReleaseObject(tempGrabber,tempgrabObj, false);
    }

    [PunRPC]
    void PlaceUxr(int grabObjId,int type,int anchorId)
    {
        UxrGrabbableObject tempgrabObj = GetUxrGrabbableObjectByPhotonId(grabObjId);
        tempgrabObj.gameObject.GetComponent<UxrManipulationHapticFeedback>().enabled = false;
        UxrGrabbableObjectAnchor tempAnchor = GetUxrGrabbableAnchorByPhotonId(anchorId);
        if(type == 0)
        {
            UxrGrabManager.Instance.PlaceObject(tempgrabObj, tempAnchor, UxrPlacementType.Immediate, false);
        }
        else
        {
            UxrGrabManager.Instance.PlaceObject(tempgrabObj, tempAnchor, UxrPlacementType.Smooth, false);
        }
    }
    private UxrGrabbableObjectAnchor GetUxrGrabbableAnchorByPhotonId(int photonId)
    {
        UxrGrabbableObjectAnchor temp = PhotonView.Find(photonId).gameObject.GetComponent<UxrGrabbableObjectAnchor>();
        return temp;
    }
    private UxrGrabbableObject GetUxrGrabbableObjectByPhotonId(int photonId)
    {
        return PhotonView.Find(photonId).gameObject.GetComponent<UxrGrabbableObject>();
    }
}
