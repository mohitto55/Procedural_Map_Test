using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileBase : MonoBehaviour
{
    public string TileName;
    public int TileID;
    public int Height;
    public GameObject OnObject;
    public virtual void Awake()
    {
        MapManager.Instance.MapCreatEvent += SearchSurroundTileOnCreatMap;
        MapManager.Instance.MapCreatEvent2 += SearchSurroundTileOnCreatMap2;
    }
    public virtual void SearchSurroundTileOnCreatMap()
    {

    }
    public virtual void SearchSurroundTileOnCreatMap2()
    {

    }
}
