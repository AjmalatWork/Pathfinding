using System;
using UnityEngine;
using UnityEngine.UI;

public class SetDestination : BaseUIButton, IClickableUI
{
    public GridLayoutGroup gridLayout;
    [NonSerialized] public bool isSetDestinationClicked = false;
    [NonSerialized] public Node destinationNode;

    public static SetDestination Instance { get; private set; }

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
        if (!GenerateRandomGraph.Instance.isGraphGenerated)
        {
            Debug.LogWarning("Generate graph first!");
            return;
        }
        isSetDestinationClicked = true;
        foreach (Transform child in gridLayout.transform)
        {
            GameObject gameObject = child.gameObject;
            Button button = gameObject.GetComponent<Button>();
            Node node = gameObject.GetComponent<Node>();
            button.onClick.AddListener(node.NodeClicked);
        }
    }
}
