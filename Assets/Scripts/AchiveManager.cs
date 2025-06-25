using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class AchiveEntry
{
    public string key;
    public int value;
}

[System.Serializable]
public class AchiveData
{
    public List<AchiveEntry> achives = new List<AchiveEntry>();

    public int GetValue(string key)
    {
        var entry = achives.Find(e => e.key == key);
        return entry != null ? entry.value : 0;
    }

    public void SetValue(string key, int value)
    {
        var entry = achives.Find(e => e.key == key);
        if (entry != null)
            entry.value = value;
        else
            achives.Add(new AchiveEntry { key = key, value = value });
    }

    public bool ContainsKey(string key)
    {
        return achives.Exists(e => e.key == key);
    }
}

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharater;
    public GameObject[] unlockCharater;
    public GameObject uiNotice;

    enum Achive { unlockPotato, unlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    string savePath;
    AchiveData achiveData;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        savePath = Path.Combine(Application.persistentDataPath, "achiveData.json");

        Load();
    }

    void Start()
    {
        UnlockCharater();
    }

    void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            achiveData = JsonUtility.FromJson<AchiveData>(json);
        }
        else
        {
            Init();
        }
    }

    void Init()
    {
        achiveData = new AchiveData();

        foreach (Achive achive in achives)
        {
            achiveData.SetValue(achive.ToString(), 0);
        }

        Save();
    }


    void Save()
    {
        string json = JsonUtility.ToJson(achiveData, true);
        File.WriteAllText(savePath, json);
    }

    void UnlockCharater()
    {
        for (int index = 0; index < lockCharater.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = achiveData.ContainsKey(achiveName) && achiveData.GetValue(achiveName) == 1;

            lockCharater[index].SetActive(!isUnlock);
            unlockCharater[index].SetActive(isUnlock);
        }
    }


    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.unlockPotato:
                isAchive = GameManager.instance.kill >= 100;
                break;
            case Achive.unlockBean:
                isAchive = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        string key = achive.ToString();

        if (isAchive && achiveData.GetValue(key) == 0)
        {
            achiveData.SetValue(key, 1);
            Save();

            UnlockCharater(); // 🎯 해금 즉시 반영

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }


        IEnumerator NoticeRoutine()
        {
            uiNotice.SetActive(true);
            yield return wait;
            uiNotice.SetActive(false);
        }
    }
}

