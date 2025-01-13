using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XCharts.Runtime;

public class ChartTest : MonoBehaviour
{
    [SerializeField] private LineChart lineChart;

    private void Start()
    {
        lineChart.ClearData();
        for (int i = 0; i < 5; i++)
        {
            var time = DateTime.Now.AddDays(i);
            var beginTime = new DateTime(1970, 1, 1);
            var differenceInMinutes = (time - beginTime).TotalSeconds;
            lineChart.AddData(0, differenceInMinutes, UnityEngine.Random.Range(5, 90));
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 5; i++)
            {
                var time = DateTime.Now.AddDays(i);
                var beginTime = new DateTime(1970, 1, 1);
                var differenceInMinutes = (time - beginTime).TotalSeconds;
                lineChart.AddData(0, differenceInMinutes, UnityEngine.Random.Range(5, 90));
            }
        }
    }


}
