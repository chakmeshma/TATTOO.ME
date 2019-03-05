using UnityEngine;

namespace View
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Clickable : MonoBehaviour
    {
        private void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(SharedViewResources.instance.clickableCursorTexture, new Vector2(28f, 5f), CursorMode.Auto);
        }
    }
}