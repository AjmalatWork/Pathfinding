using System;
using UnityEngine;
using UnityEngine.UI;

public class SetSource : BaseUIButton, IClickableUI
{
    public GridLayoutGroup gridLayout;
    [NonSerialized] public bool isSetSourceClicked = false;
    [NonSerialized] public Node sourceNode;

    public static SetSource Instance { get; private set; }

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
        if(!GenerateRandomGraph.Instance.isGraphGenerated)
        {
            Debug.LogWarning("Generate graph first!");
            return;
        }
        isSetSourceClicked = true;
        foreach ( Transform child in gridLayout.transform)
        {
            GameObject gameObject = child.gameObject;
            Button button = gameObject.GetComponent<Button>();
            Node node = gameObject.GetComponent<Node>();
            button.onClick.AddListener(node.NodeClicked);
        }        
    }
}
