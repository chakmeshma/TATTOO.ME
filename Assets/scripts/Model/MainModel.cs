using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

        public void getLibraryItems(List<Controller.MainController.LibraryItem> items)
        {
            for (int i = 0; i < 10; ++i)
            {
                Controller.MainController.LibraryItem info = new Controller.MainController.LibraryItem();

                info.name = " " + Random.Range(1000, 900000000000).ToString();
                info.followers = 100;
                info.likes = 200;
                info.location = "Berlin, Germany";
                info.price = 123.55f;
                info.seller = "Majid Motallebikashani";
                info.sellerInitials = "MM";
                info.pictureURL = "http://localhost/pics/852504" + Random.Range(31, 70).ToString() + ".jpg";

                items.Add(info);
            }
        }

        public void getTattooPicture(int index, string url)
        {
            StartCoroutine(fetchTattooPicture(index, url));
        }

        private IEnumerator fetchTattooPicture(int index, string url)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Controller.MainController.instance.onLibraryItemPictureDownloaded(null, index);
                }
                else
                {
                    Controller.MainController.instance.onLibraryItemPictureDownloaded(DownloadHandlerTexture.GetContent(uwr), index);
                }
            }
        }

    }
}