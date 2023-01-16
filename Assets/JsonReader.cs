using System.Collections;
using System.Collections.Generic;
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
        public string RadiusExplosion;
        public int TimeToContaminate;
    }

    public Settings settings = new Settings();

    void Start()
    {
        settings = JsonUtility.FromJson<Settings>(textJSON.text);
    }
}