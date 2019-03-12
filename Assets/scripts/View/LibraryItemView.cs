using System;
using UnityEngine;

namespace View
{
    public class LibraryItemView : MonoBehaviour
    {
        public LibraryItem info;
        public UnityEngine.UI.Text nameText;
        public UnityEngine.UI.Text priceText;
        public UnityEngine.UI.Text artistText;
        public UnityEngine.UI.Text locationText;
        public UnityEngine.UI.Text artistInitialsText;
        public UnityEngine.UI.Text followersText;
        public UnityEngine.UI.Text likesText;


        public void onPriceSelected()
        {

        }

        public void onProfileSelected()
        {

        }

        public void updateView()
        {
            nameText.text = info.name;
        }
    }
}