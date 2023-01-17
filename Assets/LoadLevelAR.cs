using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadLevelAR : MonoBehaviour
{
    public TextAsset PathJsonARVR;
    public GameObject Level;
    public List<GameObject> Prefabs;
    public List<string> PrefabNames;
    // Start is called before the first frame update
    void Start()
    {
        string ARjsonContent = PathJsonARVR.text;
        listPin jsonData = JsonUtility.FromJson<listPin>(ARjsonContent);
        foreach (var pin in jsonData.Pins)
        {
            try
            {
                GameObject instPin;
                if(Prefabs[PrefabNames.IndexOf(pin.name)].name == "Grenade")
                    instPin = PhotonNetwork.Instantiate(Prefabs[PrefabNames.IndexOf(pin.name)].name, Level.transform.position, Quaternion.identity);
                else
                    instPin = Instantiate(Prefabs[PrefabNames.IndexOf(pin.name)], Level.transform);
                instPin.transform.position = pin.position;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log(pin.name);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }




    [Serializable]
    public class listPin
    {
        public List<PinPrefab> Pins = new List<PinPrefab>();
    }

    [Serializable]
    public class PinPrefab
    {
        public Vector3 position;
        public Material material;
        public string name;
    }
}
