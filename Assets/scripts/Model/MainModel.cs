using System.Collections;
using UnityEngine;

namespace Model
{

    public class MainModel : MonoBehaviour
    {
        public static MainModel instance = null;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
        }

        public void getTattooPicture(int index, string url)
        {
            StartCoroutine(fetchTattooPicture(index, url));
        }

        private IEnumerator fetchTattooPicture(int index, string url)
        {
            using (WWW www = new WWW(url))
            {
                yield return www;

                Controller.MainController.instance.onLibraryItemPictureDownloaded(www.texture, index);
            }
        }

    }
}