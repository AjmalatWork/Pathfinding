using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : BaseUIButton, IClickableUI
{
    public static FindPath Instance { get; private set; }

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
    }

    public new void OnClick()
    {        
        AStar(GenerateRandomGraph.Instance.adjacencyMatrix, SetSource.Instance.sourceNode, SetDestination.Instance.destinationNode);
    }

    void AStar(float[,] adjacencyMatrix, Node sourceNode, Node destinationNode)
    {

    }

}
