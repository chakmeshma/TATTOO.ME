using UnityEngine;

namespace View
{

    public class LibraryItemPriceView : MonoBehaviour
    {

        private void OnMouseDown()
        {
            GetComponentInParent<LibraryItemView>().onPriceSelected();
        }
    }
}