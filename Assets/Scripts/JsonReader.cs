using System.Collections;
using System.Collections.Generic;
using UltimateXR.Mechanics.Weapons;
using UnityEngine;

public class JsonReader : MonoBehaviour
{

    public TextAsset textJSON;

    [System.Serializable]
    public class Settings
    {
        public int InitialHealth;
        public float FireDelay;
        public float TeleportDelay;
        public string VirusShotColor;
        public string PcShotColor;
        public int NbContaminatedPlayerToWin;
        public float RadiusExplosion;
        public int TimeToContaminate;
    }

    private Settings settings = new Settings();

    void Awake()
    {
        settings = JsonUtility.FromJson<Settings>(textJSON.text);
        var test = GetComponent<UxrGrenadeWeapon>();
        if (test != null)
        {
            test._damageRadius = settings.RadiusExplosion;
        }
        var tp = GetComponent<TrailTPVr>();
        if (tp != null)
        {
            tp.cooldown = settings.TeleportDelay;
        }
    }

    public Settings GetSettings()
    {
        return settings;
    }


}