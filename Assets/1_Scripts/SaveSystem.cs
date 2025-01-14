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
    [SerializeField] private bool test;
    public void Start()
    {
        GetCurrentData();
        if (test)
        {
            AddTestData();
        }
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

    private void AddTestData()
    {
        saveFile = new SaveFile();
        System.Random random = new System.Random();
        DateTime startDate = DateTime.Now.AddDays(-45);
        DateTime currentDate = startDate;

        while (currentDate <= DateTime.Now)
        {
            // %75 olasılıkla antreman yapma
            if (random.NextDouble() <= 0.75)
            {
                // Günlük rastgele 1-4 antreman
                int dailyTrainCount = random.Next(1, 5);

                for (int i = 0; i < dailyTrainCount; i++)
                {
                    // Reaksiyon süresi: Doğal düşüş ve rastgelelik
                    float reactionTime = Mathf.Clamp(
                        10f - (float)(currentDate - startDate).TotalDays / 45f * 5f + (float)(random.NextDouble() - 0.5f) * 2f,
                        5f, 12f
                    );

                    // Accuracy: Doğal artış ve rastgelelik
                    float accuracy = Mathf.Clamp(
                        30f + (float)(currentDate - startDate).TotalDays / 45f * 60f + (float)(random.NextDouble() - 0.5f) * 10f,
                        20f, 90f
                    );

                    // Total Time: Doğal düşüş ve rastgelelik
                    float totalTime = Mathf.Clamp(
                        45f - (float)(currentDate - startDate).TotalDays / 45f * 20f + (float)(random.NextDouble() - 0.5f) * 5f,
                        25f, 45f
                    );

                    // Yeni antreman verisi oluştur ve ekle
                    TrainInformation trainInfo = new TrainInformation(reactionTime, accuracy, totalTime);
                    saveFile.trainingAndDates.Add(new TrainingAndDate(currentDate, trainInfo));
                }
            }

            // Bir sonraki güne geç
            currentDate = currentDate.AddDays(1);
        }

        // Verileri kaydet
        Save();
    }

}
