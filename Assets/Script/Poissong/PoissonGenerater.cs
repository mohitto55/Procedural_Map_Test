using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonGenerater : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float raidus = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    List<Vector2> points;

    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(raidus, regionSize, rejectionSamples);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionSize / 2, regionSize);
        if(points != null)
        {
            foreach(Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
