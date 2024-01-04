using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapManager : SIngletonBehaviour<MapManager>
{
    [SerializeField]
    GameObject MapTileContainer;
    [SerializeField]
    GameObject ObjectContainer;
    [SerializeField]
    GameObject ttile;
    int mapWidth;
    int mapHeight;
    public int MapWidth
    {
        get { return mapWidth; }
    }
    public int MapHeight
    {
        get { return mapHeight; }
    }

    Dictionary<Vector2, TileBase> MapTilesPos = new Dictionary<Vector2, TileBase>();
    Dictionary<Vector2, GameObject> MapTileObjectsPos = new Dictionary<Vector2, GameObject>();
    Dictionary<string, List<TileBase>> MapTilesName = new Dictionary<string, List<TileBase>>();
    public delegate void MapCreat();
    public MapCreat MapCreatEvent;
    public MapCreat MapCreatEvent2;
    public event MapCreat PlantCreatEvent;


    public class Coord
    {
        public int tileX;
        public int tileY;
        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }
    void ProcessMap()
    {
        List<List<Coord>> roomRegions = GetRegions(1);

        int roomThresholdSize = 100000;
        Debug.Log("방개수 : "+ roomRegions.Count);
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    //Instantiate(ttile, new Vector2(tile.tileX, tile.tileY), Quaternion.identity);
                }
            }
            if(Random.Range(0, 100) > 50)
            foreach (Coord tile in roomRegion)
            {
                TileChange(new Vector2Int(tile.tileX, tile.tileY), TileDataManager.Instance.GetTileData("Mud"));
            }
        }
    }
    List<List<Coord>> GetRegions(int tileID)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[MapWidth, MapHeight];
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                TileBase tile = GetTiletoPosition(new Vector2Int(x, y));
                if (mapFlags[x, y] == 0 && tile.TileID == tileID)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);
                    //Debug.Log("Region 타일 개수 : " + newRegion.Count);
                    foreach (Coord Regiontile in newRegion)
                    {
                        mapFlags[Regiontile.tileX, Regiontile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[MapWidth, MapHeight];
        Vector2Int pos = new Vector2Int(startX, startY);
        int tileType = GetTiletoPosition(pos).TileID;

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);
            //Debug.Log("타일 위치 : " + tile.tileX +"  " +tile.tileY);
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (isInMapRange(x,y) && (y == tile.tileY || x ==tile.tileX))
                    {
                        TileBase mapTile = GetTiletoPosition(new Vector2Int(x, y));
                        if (mapFlags[x,y] == 0 && mapTile.TileID == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        Debug.Log("구역 타일 개수 : " + tiles.Count);
        return tiles;
    }
    
    bool isInMapRange(int x, int y)
    {
        return x >= 0 && MapWidth > x && y >= 0 && MapHeight > y;
    }
    public void CreatMap()
    {

    }
    public void MapChange(int XSize, int YSize, Dictionary<Vector2, TileBase> MapTilesData)
    {
        mapWidth = XSize;
        mapHeight = YSize;
        for (int X = 0; X < XSize; X++)
        {
            for (int Y = 0; Y < YSize; Y++)
            {
                Vector2Int Pos = new Vector2Int(X, Y);
                TileBase tile = MapTilesData[Pos];
                TileCreat(Pos, tile);
            }
        }
        MapTilesPos = MapTilesData;
        //ProcessMap();
        MapCreatEvent();
        MapCreatEvent2();
        PlantCreate(XSize, YSize);
        //PlantCreatEvent();
    }
    public GameObject Tree;
    public float radius;
    void PlantCreate(int XSize, int YSize)
    {
        List<Vector2> points = PoissonDiscSampling.GeneratePoints(radius, new Vector2(XSize, YSize), 30);
        int num = 0;
        for(int i = 0; i < points.Count; i++)
        {
            Vector2Int point = new Vector2Int((int)points[i].x, (int)points[i].y);
            if (MapTilesPos.ContainsKey(point))
            if(MapTilesPos[point].TileID == 1)
            {
                GameObject tree = Instantiate(Tree, (Vector2)point, Quaternion.identity);
                    tree.transform.parent = ObjectContainer.transform;
                    num++;
            }
        }
    }
    public void TileCreat(Vector2Int pos, TileBase tile)
    {
        if (!MapTilesPos.ContainsKey(pos))
        {
            GameObject tileObject = Instantiate(tile.gameObject, (Vector2)pos, Quaternion.identity);
            tileObject.transform.parent = MapTileContainer.transform;
            MapTilesPos.Add(pos, tile);
            MapTileObjectsPos.Add(pos, tileObject);
        }
        else
        {
            TileChange(pos, tile);
            //Debug.LogError(pos + "이 위치에는 이미 타일이 있습니다.");
        }
    }
    void TileDestroy(Vector2Int pos)
    {
        if (MapTilesPos.ContainsKey(pos))
        {
            //GameObject tile = MapTilesPos[pos].gameObject;
            //Debug.Log(MapTilesPos[pos].gameObject.transform.position);
            Destroy(MapTileObjectsPos[pos]);
            MapTilesPos.Remove(pos);
            MapTileObjectsPos.Remove(pos);
            //MapTilesName[tile.name].Remove(tile);
        }
    }
    public void TileChange(Vector2Int pos, TileBase tile)
    {
        if (MapTilesPos.ContainsKey(pos))
        {
            TileDestroy(pos);
            TileCreat(pos, tile);
        }

            //TileCreat(pos, tile);
    }
    public TileBase GetRandomTile(string TIleName)
    {
        if (MapTilesName.ContainsKey(TIleName))
        {
            int randomTile = Random.Range(0, MapTilesName[TIleName].Count);
            TileBase tile = MapTilesName[TIleName][randomTile];
            return tile;
        }
        else
        {
            Debug.LogError(TIleName + " 라는 이름의 타일은 없습니다.");
            return null;
        }
    }
    public TileBase GetTiletoPosition(Vector2Int pos)
    {
        if (MapTilesPos.ContainsKey(pos))
            return MapTilesPos[pos];
        else
            Debug.Log(pos + " 위치의 타일은 없습니다.");
            return null;
    }
    public int[,] GetMapSize()
    {
        int[,] mapSize = new int[MapWidth, MapHeight];
        return mapSize;
    }
}
