using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileDataManager : SIngletonBehaviour<TileDataManager>
{
    Dictionary<string, TileBase> TileDatas = new Dictionary<string, TileBase>();
    void Awake()
    {
        init();
    }
    void init()
    {
        TileBase[] tiles = Resources.LoadAll<TileBase>("Tile") ;
        Debug.Log("개수" + tiles.Length);
        for (int i = 0;i < tiles.Length; i++)
        {
            Debug.Log(tiles[i].TileName);
            TileDatas.Add(tiles[i].TileName, tiles[i]);
        }
    }
    public TileBase GetTileData(string tileName)
    {
        if (TileDatas[tileName] != null)
        {
            return TileDatas[tileName];
        }
        else
        {
            Debug.Log(tileName + " 이라는 타일은 없습니다.");
            return null;
        }
    }

}
