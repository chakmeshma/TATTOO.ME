using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class LikeHeartAction : MonoBehaviour
    {
        private Image thisImage;
        private bool selected = false;

        private void Awake()
        {
            thisImage = GetComponent<Image>();
        }

        private void OnMouseDown()
        {
            selected = !selected;

            updateView();
        }

        private void updateView()
        {
            if (selected)
            {
                thisImage.sprite = SharedViewResources.instance.likeHeartFullSprite;
            }
            else
            {
                thisImage.sprite = SharedViewResources.instance.likeHeartEmptySprite;
            }
        }
    }
}