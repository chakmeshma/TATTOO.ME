using UnityEngine;
using UnityEngine.UI;

public class FilterItemAction : MonoBehaviour
{
    private Image thisImage;
    private Text thisText;
    private bool selected = false;

    private void Awake()
    {
        thisImage = GetComponentInChildren<Image>();
        thisText = GetComponentInChildren<Text>();
    }

    private void OnMouseDown()
    {
        selected = !selected;

        updateView();
    }

    private void updateView()
    {
        if(selected)
        {
            thisImage.color = Color.black;
            thisText.color = Color.white;
        }
        else
        {
            thisImage.color = Color.white;
            thisText.color = Color.black;
        }
    }
}
