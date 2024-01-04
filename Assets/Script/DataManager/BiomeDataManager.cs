using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BiomeData
{
    public string BiomeName;
    public int BiomeID;
    public Color BiomeColor;
    public float BiomeTemperature;
}
public class BiomeDataManager : SIngletonBehaviour<BiomeDataManager>
{
    Dictionary<int, BiomeData> BiomeDatas = new Dictionary<int, BiomeData>();
    void Start()
    {
        Biomeparser();
    }
    void Biomeparser()
    {
        string path = "CSV/BiomeData";
        Data[] BiomeData = DataParser.Parse(path);
        Debug.Log("길이 : " + BiomeData.Length);
        for (int i = 0; i < BiomeData.Length; i++)
        {
            BiomeData biomeData = new BiomeData();
            biomeData.BiomeName = BiomeData[i].data[0];
            int BiomeID = Int32.Parse(BiomeData[i].data[1]);
            biomeData.BiomeID = BiomeID;
            Color color;
            ColorUtility.TryParseHtmlString(BiomeData[i].data[2], out color);
            biomeData.BiomeColor = color;
            biomeData.BiomeTemperature = Int32.Parse(BiomeData[i].data[3]);

            BiomeDatas.Add(BiomeID, biomeData);
        }
    }
}
