using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateXR.Mechanics.Weapons;
using UnityEngine;
using UnityEngine.UI;
public class BulletTarget : MonoBehaviourPunCallbacks
{
    public float health;
    public float maxHealth;
    public GameObject healthBarUi;
    public Slider slider;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TextMeshProUGUI textLife;
    [SerializeField]
    private CapsuleCollider capsCollider;
    [SerializeField]
    private UxrActor actor;
    [SerializeField]
    private AudioSource dmgTaken;
    [SerializeField]
    private AudioSource died;

    public bool isVR;
    [SerializeField] Transform racine;
    [SerializeField] BulletTarget ownedShield;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.minValue = 0;
            slider.value = maxHealth;
        }

        if (healthBarUi != null)
        {
            healthBarUi.SetActive(false);
        }

        actor.DamageReceived += LoseHealth;
    }

    private void LoseHealth(object sender, UxrDamageEventArgs e)
    {
        var photonTemp = e.ActorSource.gameObject.GetComponent<PhotonView>();
        if (photonTemp.IsMine)
        {
            if (!isVR)
            {
                photonTemp.RPC("looseLife", RpcTarget.Others, e.Damage, GetComponent<PhotonView>().ViewID);
                actor.Life -= e.Damage;
                health -= e.Damage;
                           
                if (health <= 0)
                {
                    if (gameObject.tag == "Shield")
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                    Debug.Log($"{e.ActorSource.name} killed {this.name}");
                   
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health = 0;
        }


        if (slider != null)
        {
            slider.value = health;
        }

        if (textLife != null)
        {
            textLife.text = $"{(int)health}/{(int)maxHealth}";
        }

        if (health < maxHealth)
        {
            if (healthBarUi != null)
            {
                healthBarUi.SetActive(true);
            }
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            if (gameObject.tag == "Shield")
            {
                gameObject.SetActive(false);
                return;
            }
            if (gameObject.tag == "NPC")
            {
                if (animator != null && gameObject.tag == "NPC")
                {
                    animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 4f));
                }
                if (healthBarUi != null && gameObject.tag == "NPC")
                {
                    healthBarUi.SetActive(false);
                }
                if (capsCollider != null && gameObject.tag == "NPC")
                {
                    capsCollider.enabled = false;
                }
                Destroy(gameObject, 3f);
            }
            if (photonView != null && photonView.IsMine && gameObject.tag != "NPC")
            {
                Vector3 newpos = Vector3.zero;
                if (isVR)
                {
                    newpos = RespawnManager.instance.OnVrPlayerDeath();
                }
                else
                {
                    newpos = RespawnManager.instance.OnPcPlayerDeath();
                }
                photonView.RPC("respawn", RpcTarget.AllViaServer, newpos);
            }
        }
    }
    public void Hurt()
    {
       
        if (dmgTaken != null)
        {
            dmgTaken.Play();
        }
    }
    public void Died()
    {
        if (died != null)
        {
            died.Play();
        }
    }

    [PunRPC]
    void respawn(Vector3 pos)
    {
        racine.SetPositionAndRotation(pos, racine.rotation);
        health = maxHealth;
        if (ownedShield != null)
        {
            ownedShield.health = ownedShield.maxHealth;
        }
    }

    [PunRPC]
    void looseLife(float dmg, int photonId)
    {
       
        var target = PhotonView.Find(photonId).gameObject.GetComponent<BulletTarget>();
        target.health -= dmg; 
        Debug.Log("looseheRPC");
        dmgTaken.Play();
        if (target.health <= 0)
        {
            died.Play();
        }
            /*if (target.health <= 0)
            {
                Debug.Log($"{target.name} died");
                target.animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 4f));
                if (healthBarUi != null)
                {
                    target.healthBarUi.SetActive(false);
                }
                if (capsCollider != null)
                {
                    target.capsCollider.enabled = false;
                }

                Destroy(target, 3f);
            }*/
        }
}
