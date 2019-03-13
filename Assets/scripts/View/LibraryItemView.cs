using UnityEngine;

namespace View
{
    public class LibraryItemView : MonoBehaviour
    {
        public Controller.MainController.LibraryItem info;
        public UnityEngine.UI.Text nameText;
        public UnityEngine.UI.Text priceText;
        public UnityEngine.UI.Text artistText;
        public UnityEngine.UI.Text locationText;
        public UnityEngine.UI.Text artistInitialsText;
        public UnityEngine.UI.Text followersText;
        public UnityEngine.UI.Text likesText;
        public UnityEngine.UI.RawImage picture;
        public GameObject loadingIcon;
        public RectTransform imageRect;


        public void onPriceSelected()
        {

        }

        public void onProfileSelected()
        {

        }

        public void updateView()
        {
            nameText.text = info.name;
            priceText.text = info.price.ToString();
            artistText.text = info.seller;
            locationText.text = info.location;
            artistInitialsText.text = info.sellerInitials;
            followersText.text = info.followers.ToString();
            likesText.text = info.likes.ToString();
        }

        public void updatePicture(Texture2D texture)
        {
            RectTransform thisRectTransform = GetComponent<RectTransform>();

            loadingIcon.SetActive(false);
            picture.texture = texture;

            float aspectRatio = (float)texture.height / (float)texture.width;

            float newHeight = imageRect.sizeDelta.x * aspectRatio;

            float heightDiff = newHeight - imageRect.sizeDelta.y;

            imageRect.sizeDelta = new Vector2(imageRect.sizeDelta.x, newHeight);

            thisRectTransform.sizeDelta = new Vector2(thisRectTransform.sizeDelta.x, thisRectTransform.sizeDelta.y + heightDiff);
        }
    }
}