using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavedData : MonoBehaviour
{
    public UserData userData = new();
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField]
    private TextMeshProUGUI nameUi;
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        loadFromJson();
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.save)
        {
            SaveToJson();
        }
    }

    public void SaveToJson()
    {
        string path = Application.streamingAssetsPath + "/Weather.json";
        string data = JsonUtility.ToJson(userData);     
        System.IO.File.WriteAllText(path,data);
        loadFromJson();
    }
    public void loadFromJson()
    {
        string path = Application.streamingAssetsPath + "/UserData.json";
        string data = System.IO.File.ReadAllText(path);
        userData = JsonUtility.FromJson<UserData>(data);
        nameUi.SetText(userData.Name);
    }
}

[System.Serializable]
public class UserData
{
    public string Name;
    public int KillCount;
}