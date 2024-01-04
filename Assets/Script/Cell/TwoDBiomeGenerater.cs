using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeTye
{
    Forest, Mud
}
public class TwoDBiomeGenerater : MonoBehaviour
{
    public static TwoDBiomeGenerater instance;
    public List<TileBase> TileSOList;
    public int XSize;
    public int YSize;

    public float Seed;
    public bool RandomSeed;
    [Range(0,100)]
    public float FillPercent;
    public int SmoothNum;
    Dictionary<Vector2, TileBase> MapTiles = new Dictionary<Vector2, TileBase>();

    private void Awake()
    {
        instance = this;

        if (RandomSeed)
            Seed = Random.Range(0, 100000f);
        CreatRandomMap();
        for (int i = 0; i < SmoothNum; i++)
            SmoothMap();
        MapManager.Instance.MapChange(XSize,YSize,MapTiles);
    }

    public void CreatRandomMap()
    {
        System.Random prng = new System.Random(Seed.GetHashCode());
        for (int X = 0; X < XSize; X++)
        {
            for(int Y = 0; Y < YSize; Y++)
            {
                Vector2Int Pos = new Vector2Int(X, Y);

                // 1 바다 0 땅
                int d = prng.Next(0, 100) < FillPercent ? 1 : 0;
                if (d == 1)
                {
                    MapTiles.Add(Pos, TileSOList[0]);
                }
                else
                {
                    MapTiles.Add(Pos, TileSOList[1]);
                }
            }
        }
    }
    public void SmoothMap()
    {
        for (int X = 0; X < XSize; X++)
        {
            for (int Y = 0; Y < YSize; Y++)
            {
                int WaterCount = GetSurroundingWallCount(X, Y);
                Vector2Int Pos = new Vector2Int(X, Y);
                if (WaterCount > 4)
                {
                    MapTiles[Pos] = TileSOList[0];
                }
                else if (WaterCount < 4)
                    MapTiles[Pos] = TileSOList[1];
            }
        }
    }
    public int GetSurroundingWallCount(int x, int y)
    {
        int WaterCount = 0;
        for (int X = x - 1; X <= x + 1; X++)
        {
            for (int Y = y - 1; Y <= y + 1; Y++)
            {
                Vector2 pos = new Vector2(X, Y);
                //Debug.Log(pos);
                if (X == x && Y == y)
                {
                    //Debug.Log("넘겨" + pos);
                    continue;
                }
                Vector2Int Pos = new Vector2Int(X, Y);
                if(MapTiles.ContainsKey(Pos))
                {
                    if(MapTiles[Pos].TileID == 0)
                    {
                        WaterCount++;
                    }
                }
                else
                {
                    WaterCount++;
                }
            }
        }
        return WaterCount;
    }
}
