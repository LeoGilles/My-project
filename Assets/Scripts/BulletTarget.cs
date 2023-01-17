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

    [SerializeField] private bool isVR;
    [SerializeField] GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if(slider != null)
        {
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = maxHealth;
        }

        if(healthBarUi != null)
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
       
            photonTemp.RPC("looseLife", RpcTarget.Others, e.Damage, GetComponent<PhotonView>().ViewID);
            actor.Life -= e.Damage;
            health -= e.Damage;

            if (gameObject.tag == "Shield")
            {
                gameObject.SetActive(false);
                return;
            }
            if (health <= 0)
            {
                Debug.Log($"{e.ActorSource.name} killed {this.name}");
                Died();
            }
            else
            {
                Hurt();
            }
        }
    }

        // Update is called once per frame
    void Update()
<<<<<<< HEAD
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health = 0;
        }


        slider.value = health;
=======
    {   
        if(slider != null)
        {
            slider.value = health;
        }

>>>>>>> main
        if(textLife != null)
        {
            textLife.text = $"{(int)health}/{(int)maxHealth}";
        }

        if (health < maxHealth)
        {
            if(healthBarUi != null)
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
            if(gameObject.tag == "Shield")
            {
                gameObject.SetActive(false);
                return;
            }
            if(animator != null)
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 4f));
            }
            if (healthBarUi != null)
            {
                healthBarUi.SetActive(false);
            }
            if (capsCollider != null)
            {
                capsCollider.enabled = false;    
            }
            if(photonView != null && photonView.IsMine && gameObject.tag != "NPC")
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
                prefab.transform.position = newpos;
            }
            health = maxHealth;
        }
    }
    public void Hurt()
    {
        if(dmgTaken != null)
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
     void looseLife(float dmg,int photonId)
     { 
            var target =PhotonView.Find(photonId).gameObject.GetComponent<BulletTarget>();
            target.actor.Life -= dmg;
            target.health -= dmg;

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
