using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class StaticControl : MonoBehaviour
{
    private SaveFile saveFile;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Dropdown dateSelectionDropdown; // son 1 hafta, son 2 hafta, son 1 ay, total time
    [SerializeField] private TMP_Dropdown typeSelectionDropdown; // accuratytable, reactiontime, totalworktime, dailyworkcount
    [SerializeField] private LineChart accuractyTable, reactiontimeTable, TotalWorkTime;
    [SerializeField] private BarChart dailyWorkCount;

    private void Start()
    {
        dateSelectionDropdown.onValueChanged.AddListener(UpdateCharts);
        typeSelectionDropdown.onValueChanged.AddListener(UpdateCharts);
    }

    public void OpenPanel()
    {
        canvasGroup.gameObject.SetActive(true);
        saveFile = SaveSystem.Instance.saveFile;
        canvasGroup.DOFade(1f, 0.2f);
        CloseAllCharts();
        UpdateCharts(0);
    }

    public void ClosePanel()
    {
        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
        });
    }

    public void CloseAllCharts()
    {
        accuractyTable.gameObject.SetActive(false);
        reactiontimeTable.gameObject.SetActive(false);
        TotalWorkTime.gameObject.SetActive(false);
        dailyWorkCount.gameObject.SetActive(false);
    }

    private void UpdateCharts(int _)
    {
        if (saveFile == null) return;

        CloseAllCharts();

        // Zaman aralığını al
        DateTime startDate = DateTime.Now;
        switch (dateSelectionDropdown.value)
        {
            case 0: // Son 1 hafta
                startDate = DateTime.Now.AddDays(-7);
                break;
            case 1: // Son 2 hafta
                startDate = DateTime.Now.AddDays(-14);
                break;
            case 2: // Son 1 ay
                startDate = DateTime.Now.AddMonths(-1);
                break;
            case 3: // Total time
                if (saveFile.trainingAndDates.Count > 0)
                {
                    startDate = saveFile.trainingAndDates[0].dateTime.Date;
                }
                break;
        }

        // Tablo tipine göre verileri göster
        switch (typeSelectionDropdown.value)
        {
            case 0: // Accuracy Table
                accuractyTable.gameObject.SetActive(true);
                accuractyTable.ClearData();
                var dailyAccuracy = saveFile.trainingAndDates
                    .Where(entry => entry.dateTime >= startDate)
                    .GroupBy(entry => entry.dateTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        AverageAccuracy = g.Average(entry => entry.trainInformation.accuracy)
                    });

                foreach (var entry in dailyAccuracy)
                {
                    var timeStamp = (entry.Date - new DateTime(1970, 1, 1)).TotalSeconds;
                    accuractyTable.AddData(0, timeStamp, entry.AverageAccuracy);
                }
                break;

            case 1: // Reaction Time
                reactiontimeTable.gameObject.SetActive(true);
                reactiontimeTable.ClearData();
                var dailyReactionTime = saveFile.trainingAndDates
                    .Where(entry => entry.dateTime >= startDate)
                    .GroupBy(entry => entry.dateTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        AverageReactionTime = g.Average(entry => entry.trainInformation.reactionTime)
                    });

                foreach (var entry in dailyReactionTime)
                {
                    var timeStamp = (entry.Date - new DateTime(1970, 1, 1)).TotalSeconds;
                    reactiontimeTable.AddData(0, timeStamp, entry.AverageReactionTime);
                }
                break;

            case 2: // Total Work Time
                TotalWorkTime.gameObject.SetActive(true);
                TotalWorkTime.ClearData();
                var cumulativeTotalTime = 0.0f;
                var dailyCumulativeTime = saveFile.trainingAndDates
                    .Where(entry => entry.dateTime >= startDate)
                    .GroupBy(entry => entry.dateTime.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new
                    {
                        Date = g.Key,
                        CumulativeTime = (cumulativeTotalTime += g.Sum(entry => entry.trainInformation.totalTime))
                    });

                foreach (var entry in dailyCumulativeTime)
                {
                    var timeStamp = (entry.Date - new DateTime(1970, 1, 1)).TotalSeconds;
                    TotalWorkTime.AddData(0, timeStamp, entry.CumulativeTime);
                }
                break;

            case 3: // Daily Work Count
                dailyWorkCount.gameObject.SetActive(true);
                dailyWorkCount.ClearData();

                var dailyCounts = saveFile.trainingAndDates
                    .Where(entry => entry.dateTime >= startDate)
                    .GroupBy(entry => entry.dateTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    });

                foreach (var entry in dailyCounts)
                {
                    var timeStamp = (entry.Date - new DateTime(1970, 1, 1)).TotalSeconds;
                    dailyWorkCount.AddData(0, timeStamp, entry.Count);
                }
                break;
        }
    }
}


