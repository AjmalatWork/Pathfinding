using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRandomGraph : BaseUIButton, IClickableUI
{
    public GridLayoutGroup gridLayout;
    public int edgeProbability = 20;

    const int numColumns = 20;
    const int numRows = 11;

    Transform[] randomTransformGraph = new Transform[numColumns * numRows];
    float[,] adjacencyMatrix = new float[numColumns * numRows, numColumns * numRows];

    int lineCount;
    RectTransform gridTransform;    
    LineRenderer lineRenderer;

    private void Start()
    {
        gridTransform = gridLayout.GetComponent<RectTransform>();
    }

    public new void OnClick()
    {        
        GenerateGraph();
    }

    void GenerateGraph()
    {        
        GetNodesInArray();
        CreateEdges();
        //RemoveUnconnectedNodes();
    }

    void GetNodesInArray()
    {
        int noOfNodes = 0;

        foreach (Transform child in gridLayout.transform)
        {
            randomTransformGraph[noOfNodes] = child;
            if(!randomTransformGraph[noOfNodes].gameObject.activeSelf)
            {
                randomTransformGraph[noOfNodes].gameObject.SetActive(true);
            }
            noOfNodes++;
        }
    }

    //  *   *   *   *   *
    //  *   *   *   *   *
    //  *   *   *   *   *
    //  *   *   *   *   *
    //  *   *   *   *   *
    // For each node in above graph, we need to connect nodes with only 3 other nodes that are neighbouring to it
    // One to the right, One down and One diagonally right down
    void CreateEdges()
    {
        int noOfNodes = randomTransformGraph.Length;        

        for (int i = 0; i < noOfNodes; i++)
        {
            lineCount = 0;
            lineRenderer = randomTransformGraph[i].GetComponent<LineRenderer>();
            lineRenderer.positionCount = 8;
            float neighbourDistance;

            bool isLastColumn = ((i + 1) % numColumns == 0) ? true : false;

            if (i + 1 < noOfNodes && !isLastColumn)
            {
                neighbourDistance = ConnectNodes(randomTransformGraph[i], randomTransformGraph[i + 1]);
                adjacencyMatrix[i, i + 1] = neighbourDistance;
                adjacencyMatrix[i + 1, i] = neighbourDistance;
            }

            if (i + numColumns < noOfNodes)
            {
                neighbourDistance = ConnectNodes(randomTransformGraph[i], randomTransformGraph[i + numColumns]);
                adjacencyMatrix[i, i + numColumns] = neighbourDistance;
                adjacencyMatrix[i + numColumns, i] = neighbourDistance;
            }

            if (i + numColumns + 1 < noOfNodes && !isLastColumn)
            {
                neighbourDistance = ConnectNodes(randomTransformGraph[i], randomTransformGraph[i + numColumns + 1]);
                adjacencyMatrix[i, i + numColumns + 1] = neighbourDistance;
                adjacencyMatrix[i + numColumns + 1, i] = neighbourDistance;
            }

            if (i - numColumns + 1 > 0 && !isLastColumn)
            {
                neighbourDistance = ConnectNodes(randomTransformGraph[i], randomTransformGraph[i - numColumns + 1]);
                adjacencyMatrix[i, i - numColumns + 1] = neighbourDistance;
                adjacencyMatrix[i - numColumns + 1, i] = neighbourDistance;
            }

            lineRenderer.positionCount = lineCount;
        }
    }

    float ConnectNodes(Transform node1, Transform node2)
    {
        if (UnityEngine.Random.Range(1, 101) <= edgeProbability)
        {
            DrawEdge(node1, node2);            
            return Vector2.Distance(node1.transform.position, node2.transform.position);            
        }
        else
        {
            return 0f;
        }
    }

    void DrawEdge(Transform node1, Transform node2) 
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridTransform, node1.transform.position, Camera.main, out Vector3 worldStartPosition);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridTransform, node2.transform.position, Camera.main, out Vector3 worldEndPosition);
        worldStartPosition = new Vector3(worldStartPosition.x, worldStartPosition.y, worldStartPosition.z);
        worldEndPosition = new Vector3(worldEndPosition.x, worldEndPosition.y, worldEndPosition.z);
        
        lineRenderer.SetPosition(lineCount, worldStartPosition);
        lineRenderer.SetPosition(lineCount + 1, worldEndPosition);
        lineCount += 2;

        //Debug.Log(node1.name + worldStartPosition + " to " + worldEndPosition);
    }

    void RemoveUnconnectedNodes()
    {
        bool keepNode;
        List<int> nodesToRemove = new();

        for (int i = 0; i < numColumns  *numRows; i++)
        {
            keepNode = false;
            for (int j = 0; j < numColumns * numRows; j++)
            {
                if (adjacencyMatrix[i, j] != 0)
                {
                    Debug.Log($"(I,J): ({i},{j}) and Value: {adjacencyMatrix[i, j]}");
                    keepNode = true;
                    break;
                }
            }
            if (!keepNode)
            {
                randomTransformGraph[i].gameObject.SetActive(false);
            }
        }
    }
}
