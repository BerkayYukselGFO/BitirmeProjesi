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
        for (int i = 0; i < 5; i++)
        {
            lineChart.AddData(0, UnityEngine.Random.Range(0, 10));
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 5; i++)
            {
                lineChart.AddData(0, UnityEngine.Random.Range(0, 10));
            }

        }
    }


}
