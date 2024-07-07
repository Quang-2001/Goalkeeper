using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSeachNode;

    Queue<Node> frontier = new ();
    Dictionary<Vector2Int, Node> reached = new();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    Dictionary<Vector2Int, Node> grid = new();
    GridManager gridManager;
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
            
        }

        
    }
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
    void ExploreNeighbors()
    {
        List<Node> neighbors = new ();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSeachNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
                
            }
        }
        foreach(Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.iswalkable)
            {
                neighbor.connectedTo = currentSeachNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }


    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.iswalkable = true;
        destinationNode.iswalkable = true;
        frontier.Clear();
        reached.Clear();

        bool isRunning = true;
        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates,grid[coordinates]);
        while(frontier.Count > 0 && isRunning)
        {
            currentSeachNode = frontier.Dequeue();
            currentSeachNode.isExplored = true;
            ExploreNeighbors();
            if (currentSeachNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].iswalkable;
            grid[coordinates].iswalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].iswalkable = previousState;
            if (newPath.Count  <= 1)
            {
                GetNewPath();
                return true;
            }
            
        }
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath",false, SendMessageOptions.DontRequireReceiver);
    }
}
