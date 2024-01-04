using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NoiseMapGenerator : MonoBehaviour
{
    //public enum DrawMode { NoiseMapGenerator, ColourMap, Mesh, Falloff};
    //public DrawMode drawMode;

    public Renderer render;
    public int Width;
    public int Height;
    public float scale;
    [Range(0,1)]
    public float Persistance;
    public float Lacunarity;
    public int Octaves;

    //[Range(0,1)]
    //public float IslandScale;
    public int Seed;
    public bool RandomSeed;

    public int ChuckSize;
    public bool UseFalloffMap;
    float[,] falloffMap;

    public bool a;
    public Vector2 Offset;
    Dictionary<Vector2Int, TileBase> MapTiles = new Dictionary<Vector2Int, TileBase>();
    float[,] noiseMap;
    public List<tileTexture> tileTextureList = new List<tileTexture>();
    [System.Serializable]
    public struct tileTexture
    {
        public string tileName;
        public float tileHeight;
        public Color tileColor;
    }
    private void Start()
    {
        CreatNoiseMapAndTexture();
        CreatMap();
    }
    public void DrawMapInEditor()
    {
       if(UseFalloffMap)
        {
            falloffMap = FallOffMapGenerator.GenerateFalloffMap(ChuckSize);
        }
    }
    public void CreatNoiseMapAndTexture()
    {
        if (UseFalloffMap)
            falloffMap = FallOffMapGenerator.GenerateFalloffMap(ChuckSize);
        noiseMap = GenerateNoiseMap(Width, Height, Seed, scale , Octaves, Persistance, Lacunarity, Offset);
        NoiseMapTexture();
    }
    public void CreatMap()
    {
        Dictionary<Vector2, TileBase> mapTiles = new Dictionary<Vector2, TileBase>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                TileBase tile = null;
                for (int i = 0; i < tileTextureList.Count; i++)
                {
                    if (tileTextureList[i].tileHeight > noiseMap[x, y])
                    {
                        tile = TileDataManager.Instance.GetTileData(tileTextureList[i].tileName);
                        break;
                    }
                    tile = TileDataManager.Instance.GetTileData(tileTextureList[i].tileName);
                }
                if (tile == null)
                    tile = TileDataManager.Instance.GetTileData("DeepWater");
                mapTiles.Add(pos, tile);
            }
        }
        MapManager.Instance.MapChange(Width, Height, mapTiles);
    }

    public float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        if (RandomSeed)
            Seed = Random.Range(0, 1000000);

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

                for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // inverseLerp(a,b 값) = 비율
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public void NoiseMapTexture()
    {
        Texture2D noiseMapTexture = new Texture2D(Width, Height);
        Color[] colors = new Color[Width * Height];
        Debug.Log("asd");
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                if (UseFalloffMap)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                for (int i = 0; i < tileTextureList.Count; i++)
                {
                    if (tileTextureList[i].tileHeight > noiseMap[x, y])
                    {
                        colors[Width * y + x] = tileTextureList[i].tileColor;
                        break;
                    }
                    colors[Width * y + x] = tileTextureList[i].tileColor;
                    //colors[Width * y + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
                }
            }
        noiseMapTexture.SetPixels(colors);
        noiseMapTexture.filterMode = FilterMode.Point;
        noiseMapTexture.Apply();
        render.sharedMaterial.mainTexture = noiseMapTexture;
        render.transform.localScale = new Vector3(Width, 1, Height);
    }
    private void OnValidate()
    {
        //falloffMap = FallOffMapGenerator.GenerateFalloffMap(Width);
    }
    private void OnDrawGizmos()
    {
    }
}
[CustomEditor(typeof(NoiseMapGenerator), true), CanEditMultipleObjects]
public class NoiseMapGeneratorEditor : Editor
{

    NoiseMapGenerator Script;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(Script.a)
        {
            Script.CreatNoiseMapAndTexture();
        }
        if(GUILayout.Button("Generate Map"))
        {
            Script.CreatMap();
        }
    }

    private void OnEnable()
    {
        Script = (NoiseMapGenerator)target;
    }
}

