using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using Photon.Pun;

public class ThirdPersonShooter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private float normalSensitivity;
    [SerializeField]
    private float aimSensitivity;
    [SerializeField]
    private LayerMask aimColliderLayerMask;
    [SerializeField]
    private Transform debugTransform;
    [SerializeField]
    private GameObject bulletProjectile;
    [SerializeField]
    private Transform spawnBulletPosition;
    [SerializeField]
    private AudioSource audioBullet;
    [SerializeField]
    private float fireRate = 1.5f;

    private bool canShoot = true;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;


    private void Awake()
    {

        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();

    }


    private void Update()
    {
        if (!photonView.IsMine) return;
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.shoot && !starterAssetsInputs.menu && canShoot)
        {
            canShoot = false;
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            thirdPersonController.SetRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 100f);
            thirdPersonController.SetRotateOnMove(true);

            //Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            photonView.RPC("Shoot", RpcTarget.AllViaServer, spawnBulletPosition.position, aimDir, PhotonNetwork.NickName);
            if (audioBullet != null)
            {
                audioBullet.Play();
            }
            StartCoroutine(CoolDown(fireRate));
            starterAssetsInputs.shoot = false;
        }

    }
    IEnumerator CoolDown(float CD)
    {
        yield return new WaitForSeconds(CD);
        Debug.Log("shootcc");
        canShoot = true;
    }
    [PunRPC]
    void Shoot(Vector3 position, Vector3 aimDir, string nickname, PhotonMessageInfo info)
    {

        // Tips for Photon lag compensation. Il faut compenser le temps de lag pour l'envoi du message.
        // donc décaler la position de départ de la balle dans la direction
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        // Instantiate the Snowball from the Snowball Prefab at the position of the Spawner
        var temp = Instantiate(bulletProjectile, position, Quaternion.LookRotation(aimDir, Vector3.up));
        temp.GetComponent<BulletProjectiles>().shooter = nickname;
        // Set velocity to the snowballRigidBody direction and speed
        //...

        // Instantiate the snow ball
        //...

        // Destroy the Snowball after 5 seconds
        //...
    }
}
