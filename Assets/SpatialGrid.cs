using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid : MonoBehaviour
{
    public static SpatialGrid Instance;
    public int[] startIndices;
    public Entry[] spatialLookup;
    public Vector2[] points;
    public int gridSize = 10;
    public float radius;

    public float offsetX;
    public float offsetY;

    //Vector2Int[] cellOffsets = { 
    //    new Vector2Int(0, 0), 

    //    new Vector2Int(1, 0),
    //    new Vector2Int(1, 1),
    //    new Vector2Int(0, 1),

    //    new Vector2Int(-1, 0),
    //    new Vector2Int(-1, -1),
    //    new Vector2Int(0, -1),

    //    new Vector2Int(1, -1),
    //    new Vector2Int(-1, 1),
    //};

    Vector2Int[] cellOffsets = {
        new Vector2Int(0, 0),

        //new Vector2Int(1, 0),
        //new Vector2Int(1, 1),
        //new Vector2Int(0, 1),

        //new Vector2Int(-1, 0),
        //new Vector2Int(-1, -1),
        //new Vector2Int(0, -1),

        //new Vector2Int(1, -1),
        //new Vector2Int(-1, 1),
    };

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        spatialLookup = new Entry[gridSize];
        startIndices = new int[spatialLookup.Length];
    }

    private void Update()
    {        
        UpdateSpatialLookup(EntityManager.Instance.positions.ToArray(), 80);
    }

    public (int x, int y) PositionToCellCoord(Vector2 point, float radius)
    {
        int cellX = (int)Mathf.Ceil(point.x / radius);
        int cellY = (int)Mathf.Ceil(point.y / radius);
        return (cellX, cellY);
    }

    public uint HashCell(int cellX, int cellY)
    {
        uint a = (uint)cellX * 15823;
        uint b = (uint)cellY * 9737333;
        return a + b;
    }

    public uint GetKeyFromHash(uint hash)
    {
        return hash % (uint)spatialLookup.Length;
    }

    public void UpdateSpatialLookup(Vector2[] points, float radius)
    {
        this.points = points;
        this.radius = radius;

        for (int i = 0; i < points.Length; i++)
        {
            (int cellX, int cellY) = PositionToCellCoord(points[i], radius);

            //Debug.Log($"index: {i} -- cell: {cellX},{cellY}");

            uint cellKey= GetKeyFromHash(HashCell(cellX, cellY));
            spatialLookup[i] = new Entry(i, cellKey);
            startIndices[i] = int.MaxValue;
        }

        Array.Sort(spatialLookup);

        for (int i = 0;i < points.Length; i++)
        {
            if (spatialLookup[i] != null)
            {
                uint key = spatialLookup[i].cellKey;
                uint prevKey = i == 0 ? uint.MaxValue : spatialLookup[i - 1].cellKey;
                if (prevKey != key)
                {
                    startIndices[key] = i;
                }
            }
        }
    }

    public void ForEachPointInSample(Vector2 samplePoint, Action<int> OnSuccess)
    {
        (int centerX, int centerY) = PositionToCellCoord(samplePoint, radius);
        float sqrRadius = radius * radius;

        foreach(Vector2Int offset in cellOffsets)
        {
            uint key = GetKeyFromHash(HashCell(centerX + offset.x, centerY + offset.y));
            int cellStartIndex = startIndices[key];

            for (int i = cellStartIndex; i <startIndices.Length; i++)
            {
                if (spatialLookup!= null)
                {
                    if (spatialLookup[i]?.cellKey != key)
                        break;

                    int index = spatialLookup[i].index;
                    float sqrDistance = (points[index] - samplePoint).sqrMagnitude;

                    if (EntityManager.Instance.entities.Count > 0 && EntityManager.Instance.entities.Count > index)
                    {
                        var transformToAvoid = EntityManager.Instance.entities[index].transform;

                        Vector3 offsetdist = transformToAvoid.position - new Vector3(samplePoint.x, 0, samplePoint.y);

                        if (sqrDistance <= sqrRadius)
                        {
                            OnSuccess?.Invoke(index);
                        }
                    }
                }
            }
        }
    }

    void DrawGrid()
    {
        Vector3 center = transform.position;
        float diamter = radius;
        center = center + new Vector3(radius/2, 0, radius/2);

        Vector3 drawPos;

        for (int i = 0; i < gridSize / 2; i++)
        {

            for (int j = 0; j < gridSize / 2; j++)
            {
                drawPos = center + new Vector3(diamter * i, 0, diamter * j);
                Gizmos.DrawWireCube(drawPos, new Vector3(diamter, 1, diamter));

                drawPos = center - new Vector3(diamter * i, 0, diamter * j);
                Gizmos.DrawWireCube(drawPos, new Vector3(diamter, 1, diamter));

                drawPos = center + new Vector3(diamter * i, 0, -diamter * j);
                Gizmos.DrawWireCube(drawPos, new Vector3(diamter, 1, diamter));

                drawPos = center + new Vector3(-diamter * i, 0, diamter * j);
                Gizmos.DrawWireCube(drawPos, new Vector3(diamter, 1, diamter));
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    private void OnDrawGizmosSelected()
    {
        DrawGrid();
    }
}

[Serializable]
public class Entry : IComparable<Entry>
{
    public int index;
    public uint cellKey;

    public Entry(int index, uint cellKey)
    {
        this.index = index;
        this.cellKey = cellKey;
    }

    public int CompareTo(Entry other)
    {
        return cellKey.CompareTo(other.cellKey);
    }
}
