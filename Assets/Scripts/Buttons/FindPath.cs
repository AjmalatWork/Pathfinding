using System;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : BaseUIButton, IClickableUI
{
    public static FindPath Instance { get; private set; }
    [NonSerialized] public LineRenderer lineRenderer;
    [NonSerialized] public List<Node> nodes = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        lineRenderer = GetComponent<LineRenderer>();
    }

    public new void OnClick()
    {
        List<Node> path = AStar(GenerateRandomGraph.Instance.adjacencyMatrix, SetSource.Instance.sourceNode, SetDestination.Instance.destinationNode);

        if (path.Count == 0) 
        {
            Debug.LogWarning("No path found");
            return;
        }

        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count - 1; i++) 
        {
            Debug.Log(path[i].Index + "," + path[i+1].Index);            
            DrawEdge(path[i], path[i+1], i);
        }        
    }

    void DrawEdge(Node node1, Node node2, int linePosition)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GenerateRandomGraph.Instance.gridTransform, node1.transform.position, Camera.main, out Vector3 worldStartPosition);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GenerateRandomGraph.Instance.gridTransform, node2.transform.position, Camera.main, out Vector3 worldEndPosition);
        worldStartPosition = new Vector3(worldStartPosition.x, worldStartPosition.y, worldStartPosition.z);
        worldEndPosition = new Vector3(worldEndPosition.x, worldEndPosition.y, worldEndPosition.z);

        lineRenderer.SetPosition(linePosition, worldStartPosition);
        lineRenderer.SetPosition(linePosition + 1, worldEndPosition);        
    }

    List<Node> AStar(float[,] adjacencyMatrix, Node sourceNode, Node destinationNode)
    {
        int nodeCount = adjacencyMatrix.GetLength(0);

        // Set up source node
        sourceNode.GCost = 0;
        sourceNode.HCost = heuristic(sourceNode, destinationNode);

        List<Node> openSet = new() { sourceNode };
        HashSet<Node> closedSet = new();

        while (openSet.Count > 0)
        {
            // Get the node with the lowest FCost
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            if (currentNode == destinationNode)
            {
                return RetracePath(sourceNode, destinationNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            for (int i = 0; i < nodeCount; i++)
            {
                if (adjacencyMatrix[currentNode.Index, i] == 0 || closedSet.Contains(nodes[i]))
                {
                    continue;
                }

                Node neighbor = nodes[i];
                float tentativeGCost = currentNode.GCost + adjacencyMatrix[currentNode.Index, i];

                if (tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = heuristic(neighbor, destinationNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return new List<Node>(); // No path found
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        // Add start node too
        path.Add(currentNode);

        path.Reverse();
        return path;
    }

    float heuristic(Node currnetNode, Node destinationNode)
    {
        float D = GenerateRandomGraph.Instance.distanceStraight;
        float D2 = GenerateRandomGraph.Instance.distanceDiagonal;
        float dx = Math.Abs(currnetNode.X - destinationNode.X);
        float dy = Math.Abs(currnetNode.Y - destinationNode.Y);
        return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
    }
}
