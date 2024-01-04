using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static void CreatMesh(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Vector3[] vertexs = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[(width * height) * 6];
        Mesh mesh;
        int vert = 0;
        int tris = 0;
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                vertexs[x + y] = new Vector3(x, noiseMap[x, y], y);
            }
        }
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;
                tris += 6;
                vert++;
            }
            vert++;
        }
    }
}
