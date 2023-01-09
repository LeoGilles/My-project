using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UltimateXR.Mechanics.Weapons;
using UnityEngine;
using UnityEngine.UI;
public class BulletTarget : MonoBehaviourPunCallbacks
{
    public float health;
    public float maxHealth;
    public GameObject healthBarUi;
    public Slider slider;
    private Animator animator;
    private CapsuleCollider capsCollider;
    [SerializeField]
    private UxrActor actor;
    // Start is called before the first frame update
    void Start()
    {

        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = maxHealth;
        if(healthBarUi != null)
        {
            healthBarUi.SetActive(false);
        }

        animator = GetComponent<Animator>();
        capsCollider = GetComponent<CapsuleCollider>();
        actor.DamageReceived += LoseHealth;
    }

    private void LoseHealth(object sender, UxrDamageEventArgs e)
    {
        Debug.Log($"{e.ActorSource.name} a tiré sur {e.ActorTarget.name} en infligeant {e.Damage} dégats de type {e.DamageType}");
        Debug.Log(health / maxHealth);
        actor.Life -= e.Damage;
        health -= e.Damage;
        if (health <= 0)
        {
            Debug.Log(e.ActorTarget.name + " was killed by " + e.ActorSource.name);
        }
    }

        // Update is called once per frame
        void Update()
    {
        slider.value = health;

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
            
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 4f));
            if (healthBarUi != null)
            {
                healthBarUi.SetActive(false);
            }
            if (capsCollider != null)
            {
                capsCollider.enabled = false;    
            }
            if(photonView != null && photonView.IsMine)
            {
                GameManager.Instance.LeaveRoom();
            }
            Destroy(this.gameObject,3f);
        }


    }
    public void LogDeath(string shooter)
    {

    }

}
