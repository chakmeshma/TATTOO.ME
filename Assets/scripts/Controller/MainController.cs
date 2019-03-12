using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class MainController : MonoBehaviour
    {
        public static MainController instance = null;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                List<View.LibraryItem> items = new List<View.LibraryItem>();



                for (int i = 0; i < 10; ++i)
                {
                    View.LibraryItem info = new View.LibraryItem();

                    info.name = " " + Random.Range(1000, 900000000000).ToString();
                    info.followers = 100;
                    info.likes = 200;
                    info.location = "Berlin, Germany";
                    info.price = 123.55f;
                    info.seller = "Majid Motallebikashani";
                    info.sellerInitials = "MM";
                    info.pictureURL = "https://cdn.ebaumsworld.com/mediaFiles/picture/2447743/852504"+ Random.Range(31, 88).ToString() +".jpg";

                    items.Add(info);
                }

                View.MainView.instance.populateLibrary(items, false);
            }
        }

        public void onLibraryItemLoaded(List<View.LibraryItem> items, int i)
        {
            Model.MainModel.instance.getTattooPicture(i, items[i].pictureURL);
        }

        public void onLibraryItemPictureDownloaded(Texture2D image, int i)
        {
            View.MainView.instance.updateLibraryItemTexture(image, i);
        }
    }
}