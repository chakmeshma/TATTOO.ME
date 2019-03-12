using UnityEngine;


namespace View
{
    public class LibraryItemProfileView : MonoBehaviour
    {
        private void OnMouseDown()
        {
            GetComponentInParent<LibraryItemView>().onProfileSelected();
        }
    }
}