using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct Data
{
    public string[] data;
}
public static class DataParser
{
    public static Data[] Parse(string path)
    {
        List<Data> DataList = new List<Data>();
        TextAsset text = Resources.Load<TextAsset>(path);
        Debug.Log(text != null);
        string[] CsvData = text.text.Split('\n');
        Data data = new Data();
        for (int i = 1; i < CsvData.Length; i++)
        {
            string[] row = CsvData[i].Split(',');
            data.data = new string[row.Length + 1];
            for (int k = 0; k < row.Length; k++)
            {
                data.data[k] = row[k];
                Debug.Log(row[k]);
            }
            DataList.Add(data);
        }
        return DataList.ToArray();
    }
}
