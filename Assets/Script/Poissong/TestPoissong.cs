using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoissong : MonoBehaviour
{
    public float radius;
    public Vector2 Size;
    public int Rejection = 30;
    List<Vector2> PointsList;
    void Start()
    {
        StartCoroutine(GeneratePoints(radius, Size, Rejection));
    }

    IEnumerator GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        PointsList = new List<Vector2>();
        float cellSize = radius / Mathf.Sqrt(2);
        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        Debug.Log("그리드 크기 : " + grid.GetLength(0) * grid.GetLength(1));
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int pointIndex = Random.Range(0, spawnPoints.Count);
            Vector2 center = spawnPoints[pointIndex];
            bool candidateAccepted = false;
            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float dir = Random.value * Mathf.PI * 2;
                Vector2 candidate = center + new Vector2(Mathf.Sin(dir),Mathf.Cos(dir)) * Random.Range(radius, 2 * radius);
                Debug.Log("캔디 : " + candidate);
                if (IsVaild(radius, cellSize, candidate, sampleRegionSize, points, grid))
                {
                    points.Add(candidate);
                    PointsList.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if(!candidateAccepted)
            spawnPoints.RemoveAt(pointIndex);
            yield return new WaitForSeconds(1);
            //yield return null;
        }
    }
    bool IsVaild(float radius, float cellSize, Vector2 candidate,Vector2 sampleRegionSize, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            Debug.Log(cellX + " " + cellY );
            int cellStartX = Mathf.Max(0, cellX - 2);
            int cellEndX = Mathf.Min(cellX + 2, (int)grid.GetLength(0) - 1);
            int cellStartY = Mathf.Max(0, cellY - 2);
            int cellEndY = Mathf.Min(cellY + 2, (int)grid.GetLength(1) - 1);
            for(int x = cellStartX; x <= cellEndX; x++)
            {
                for (int y = cellStartY; y <= cellEndY; y++)
                {
                    int Index = grid[x, y] - 1;
                    if(Index != -1)
                    {
                        //Debug.Log(x + " " + y + " Index : " + Index);
                        float sqrt = (candidate - points[Index]).sqrMagnitude;
                        if (sqrt < radius * radius)
                            return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        float cellSize = radius / Mathf.Sqrt(2);
        int MaxX = Mathf.CeilToInt(Size.x / cellSize);
        int MaxY = Mathf.CeilToInt(Size.y / cellSize);
        Gizmos.DrawWireCube(Size / 2, Size);
        //for(int x = 0; x < MaxX; x++)
        //{
        //    Gizmos.DrawLine(new Vector2(x * cellSize, 0), new Vector2(x * cellSize, MaxY));
        //}
        //for (int y = 0; y < MaxY; y++)
        //{
        //    Gizmos.DrawLine(new Vector2(0, y * cellSize), new Vector2(MaxX, y * cellSize));
        //}
        if (PointsList != null)
        {
            for (int i = 0; i < PointsList.Count; i++)
            {
                //Gizmos.DrawSphere(PointsList[i], 1);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(PointsList[i], radius);
            }
        }
    }
}
