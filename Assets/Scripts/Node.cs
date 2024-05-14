using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    Image image;
    bool isSourceSet = false;
    bool isDestinationSet = false;

    private void Awake()
    {
        gridLayout = GetComponentInParent<GridLayoutGroup>();
        image = GetComponent<Image>();
    }

    public void NodeClicked()
    {
        Sprite sprite;

        // If Set Source is clicked
        if(SetSource.Instance.isSetSourceClicked)
        {
            sprite = Resources.Load<Sprite>(ImageConstants.nodeSource);
            if(sprite != null ) 
            {
                image.sprite = sprite;
            }
            SetSource.Instance.isSetSourceClicked = false;
            SetSource.Instance.sourceNode = this;
            isSourceSet = true;
        }
        // If Set Destination is clicked
        else if(SetDestination.Instance.isSetDestinationClicked)
        {
            sprite = Resources.Load<Sprite>(ImageConstants.nodeTarget);
            if (sprite != null)
            {
                image.sprite = sprite;
            }
            SetDestination.Instance.isSetDestinationClicked = false;
            SetDestination.Instance.destinationNode = this;
            isDestinationSet = true;
        }

        
        foreach (Transform child in gridLayout.transform)
        {   
            // There can always be only one source and one destination
            if (child.gameObject != gameObject) 
            {
                Image childImage = child.GetComponent<Image>();

                // If any other node has source node image, change it with normal node
                if (childImage.sprite.name == ImageConstants.nodeSource && isSourceSet)
                {
                    childImage.sprite = Resources.Load<Sprite>(ImageConstants.node);
                }

                // If any other node has destination node image, change it with normal node
                if (childImage.sprite.name == ImageConstants.nodeTarget && isDestinationSet)
                {
                    childImage.sprite = Resources.Load<Sprite>(ImageConstants.node);
                }
            }

            // Remove onclick event listener from all nodes
            child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
