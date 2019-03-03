using UnityEngine;

namespace View
{
    public class ModelRotation : MonoBehaviour
    {
        public float speed;
        private float angle;

        void Update()
        {
            angle = speed * Time.deltaTime;

            transform.Rotate(Vector3.up, angle);
        }
    }
}