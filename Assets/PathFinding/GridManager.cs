using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int girdSize;

    [Tooltip("Unity Grip Size - Should match UnityEditor snap settings.")]
    [SerializeField] int unityGridSize = 10;
    public int UnityGripSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } } 
    void Awake()
    {
        CreatGrid();   
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
        
    }
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].iswalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach(KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x /unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z /unityGridSize);

        return coordinates;
    }
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        return position;
    }

    void CreatGrid()
    {
        for(int x = 0; x <girdSize.x; x++)
        {
            for(int y= 0; y<girdSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
                
            }       
        }
    }
}
