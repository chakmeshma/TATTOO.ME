using UnityEngine;


namespace View
{
    public class SharedViewResources : MonoBehaviour
    {
        public static SharedViewResources instance;

        public Texture2D clickableCursorTexture;

        private void Awake()
        {
            instance = this;
        }
    }
}