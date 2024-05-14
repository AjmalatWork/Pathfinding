using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    Image image;

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
        }
        else if(SetDestination.Instance.isSetDestinationClicked)
        {
            sprite = Resources.Load<Sprite>("Node Target");
            if (sprite != null)
            {
                image.sprite = sprite;
            }
            SetDestination.Instance.isSetDestinationClicked = false;
        }
        foreach (Transform child in gridLayout.transform)
        {
            child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
