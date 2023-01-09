using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PhotonTransformChildView : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool SynchronizePosition = true;
    public bool SynchronizeRotation = true;
    public bool SynchronizeScale = false;

    public List<Transform> SynchronizedChildTransform;
    private List<Vector3> localPositionList;
    private List<Quaternion> localRotationList;
    private List<Vector3> localScaleList;

    // Start is called before the first frame update
    void Awake()
    {
        localPositionList = new(SynchronizedChildTransform.Count);
        localRotationList = new(SynchronizedChildTransform.Count);
        localScaleList = new(SynchronizedChildTransform.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            foreach (var trans in SynchronizedChildTransform.Select((data, index) => (data, index)))
            {
                if (SynchronizePosition)
                {
                    trans.data.localPosition = localPositionList[trans.index];
                }
                if (SynchronizeRotation)
                {
                    trans.data.localRotation = localRotationList[trans.index];
                }
                if (SynchronizeScale)
                {
                    trans.data.localScale = localScaleList[trans.index];
                }

            }
        }
    }


    #region IPUnObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Code for sending data to synchronize
            stream.SendNext(SynchronizedChildTransform[0]);
        }
        else
        {
            // Code for reading data to synchronize
            localPositionList = (List<Vector3>)stream.ReceiveNext();

            localRotationList = (List<Quaternion>)stream.ReceiveNext();
        }

    }
    #endregion
}


