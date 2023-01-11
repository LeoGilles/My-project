using System.Collections;
using System.Collections.Generic;
using UltimateXR.Locomotion;
using UnityEngine;
using Photon.Pun;
using CartoonFX;

public class TrailTPVr : MonoBehaviourPunCallbacks
{   [SerializeField]
    private UxrTeleportLocomotionBase tpLeft;
    [SerializeField]
    private UxrTeleportLocomotionBase tpRight;
    [SerializeField]
    private GameObject prefabAnimation;
    // Start is called before the first frame update
    void Start()
    {
        tpLeft.Teleported += Tp_Teleported;
        tpRight.Teleported += Tp_Teleported;
    }

    private void Tp_Teleported(object sender, TeleportEventArgs e)
    {
        photonView.RPC("Trail", RpcTarget.Others, e.arg1.position, e.arg2);
    }

    [PunRPC]
    void Trail(Vector3 position, Vector3 newPosition)
    {
        StartCoroutine(Lerp(position,newPosition));
    }
    IEnumerator Lerp(Vector3 position, Vector3 newPosition)
    {
        float timeElapsed = 0;  
        float lerpDuration = 0.4f;
        position.y = 1;
        newPosition.y = 1;
        var cloneAnimation = Instantiate(prefabAnimation, position,Quaternion.identity);
        cloneAnimation.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        while (timeElapsed < lerpDuration)
        {
            cloneAnimation.transform.position = Vector3.Lerp(position, newPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cloneAnimation.transform.position = newPosition;
        Destroy(cloneAnimation);
    }
}
