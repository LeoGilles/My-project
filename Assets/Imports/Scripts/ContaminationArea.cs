﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContaminationArea : MonoBehaviour
{
    [System.Serializable]
    public struct BelongToProperties
    {
        public Color mainColor;
        public Color secondColor;
            
    }

    public BelongToProperties nobody;
    public BelongToProperties virus;
    public BelongToProperties scientist;

    public float cullRadius = 5f;

    private ParticleSystem pSystem;
    
    public float inTimer = 0f;
    private float maxInTimer = 3f;

    private CullingGroup cullGroup;

    void Start()
    {
        populateParticleSystemCache();
        setupCullingGroup();

        BelongsToNobody();
    }

    private void populateParticleSystemCache()
    {
        pSystem = this.GetComponentInChildren<ParticleSystem>();
    }


    /// <summary>
    /// This manage visibility of particle for the camera to optimize the rendering.
    /// </summary>
    private void setupCullingGroup()
    {
        Debug.Log($"setupCullingGroup {Camera.main}");
        cullGroup = new CullingGroup();
        cullGroup.targetCamera = Camera.main;
        cullGroup.SetBoundingSpheres(new BoundingSphere[] { new BoundingSphere(transform.position, cullRadius) });
        cullGroup.SetBoundingSphereCount(1);
        cullGroup.onStateChanged += OnStateChanged;
    }

    void OnStateChanged(CullingGroupEvent cullEvent)
    {
        Debug.Log($"cullEvent {cullEvent.isVisible}");
        if (cullEvent.isVisible)
        {
            pSystem.Play(true);
        }
        else
        {
            pSystem.Pause();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTimer += Time.deltaTime;
            if (inTimer > maxInTimer)
            {
                if (GameManager.Instance.pcPlayer.Contains(other.gameObject))
                {
                    BelongsToScientists();
                }
                else if (GameManager.Instance.vrPlayer.Contains(other.gameObject))
                {
                    BelongsToVirus();
                }
                
            }
        }
    }


    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            inTimer = 0;
        }        
    }

    void Update()
    {
    
    }

    private void ColorParticle(ParticleSystem pSys, Color mainColor, Color accentColor)
    {
        // TODO: Solution to color particle 
        var main = pSys.main;
        main.startColor = new ParticleSystem.MinMaxGradient(mainColor,accentColor);

    }

    public void BelongsToNobody()
    {
        ColorParticle(pSystem, nobody.mainColor, nobody.secondColor);
    }

    public void BelongsToVirus()
    {
        ColorParticle(pSystem, virus.mainColor, virus.secondColor);
    }

    public void BelongsToScientists()
    {
        ColorParticle(pSystem, scientist.mainColor, scientist.secondColor);
    }

    void OnDestroy()
    {
        if (cullGroup != null)
            cullGroup.Dispose();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, cullRadius);
    }
}