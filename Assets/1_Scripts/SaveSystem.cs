using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class TrainInformation
{
    public float reactionTime;
    public float accuracy;
    public float totalTime;

    public TrainInformation(float reactionTime, float accuracy, float totalTime)
    {
        this.reactionTime = reactionTime;
        this.accuracy = accuracy;
        this.totalTime = totalTime;
    }
}

[System.Serializable]
public class TrainingAndDate
{
    public DateTime dateTime;
    public TrainInformation trainInformation;

    public TrainingAndDate(DateTime dateTime, TrainInformation trainInformation)
    {
        this.dateTime = dateTime;
        this.trainInformation = trainInformation;
    }
}

[System.Serializable]
public class SaveFile
{
    public List<TrainingAndDate> trainingAndDates = new();
}
public class SaveSystem : Singleton<SaveSystem>
{
    public SaveFile saveFile;

    public void Start()
    {
        GetCurrentData();
    }

    private void GetCurrentData()
    {
        if (PlayerPrefs.HasKey("SaveFile"))
        {
            var jsonData = PlayerPrefs.GetString("SaveFile");
            saveFile = JsonUtility.FromJson<SaveFile>(jsonData);
        }
        else
        {
            saveFile = new SaveFile();
            var emptyJson = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString("SaveFile", emptyJson);
        }
    }

    private void Save()
    {
        var jsonData = JsonUtility.ToJson(saveFile);
        PlayerPrefs.SetString("SaveFile", jsonData);
    }
    public void AddTraining(TrainInformation trainInformation)
    {
        saveFile.trainingAndDates.Add(new TrainingAndDate(DateTime.Now, trainInformation));
        Save();
    }
}
