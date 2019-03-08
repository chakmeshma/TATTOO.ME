using UnityEngine;


namespace View
{
    public class SharedViewResources : MonoBehaviour
    {
        public static SharedViewResources instance;

        public Texture2D clickableCursorTexture;
        public Sprite likeHeartEmptySprite;
        public Sprite likeHeartFullSprite;

        private void Awake()
        {
            instance = this;
        }
    }
}