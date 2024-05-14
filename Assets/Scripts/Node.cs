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

        if(SetSource.Instance.isSetSourceClicked)
        {
            sprite = Resources.Load<Sprite>("Node Source");
            if(sprite != null ) 
            {
                image.sprite = sprite;
            }
            SetSource.Instance.isSetSourceClicked = false;
            isSourceSet = true;
        }
        else if(SetDestination.Instance.isSetDestinationClicked)
        {
            sprite = Resources.Load<Sprite>("Node Target");
            if (sprite != null)
            {
                image.sprite = sprite;
            }
            SetDestination.Instance.isSetDestinationClicked = false;
            isDestinationSet = true;
        }
        foreach (Transform child in gridLayout.transform)
        {            
            if (child.gameObject != gameObject) 
            {
                Image childImage = child.GetComponent<Image>();
                if (childImage.sprite.name == "Node Source" && isSourceSet)
                {
                    childImage.sprite = Resources.Load<Sprite>("Node");
                }
                if (childImage.sprite.name == "Node Target" && isDestinationSet)
                {
                    childImage.sprite = Resources.Load<Sprite>("Node");
                }
            }            
            child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
