using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class MainController : MonoBehaviour
    {
        public struct LibraryItem
        {
            public float price;
            public string name;
            public string seller;
            public string sellerInitials;
            public int followers;
            public int likes;
            public string location;
            public string pictureURL;
        }
        public static MainController instance = null;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
        }


        private void loadLibraryItems()
        {
            List<Controller.MainController.LibraryItem> items = new List<Controller.MainController.LibraryItem>();

            Model.MainModel.instance.getLibraryItems(items);

            View.MainView.instance.populateLibrary(items);
        }

        private void nextLoadLibraryItems()
        {
            List<Controller.MainController.LibraryItem> newItems = new List<Controller.MainController.LibraryItem>();

            Model.MainModel.instance.getNewLibraryItems(newItems);

            if (newItems.Count > 0)
            {
                View.MainView.instance.appendLibrary(newItems);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                loadLibraryItems();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                nextLoadLibraryItems();
            }
        }

        public void scrolledDown()
        {
            nextLoadLibraryItems();
        }

        public void onLibraryItemLoaded(List<Controller.MainController.LibraryItem> items, int i)
        {
            Model.MainModel.instance.getTattooPicture(i, items[i].pictureURL);
        }

        public void onLibraryItemPictureDownloaded(Texture2D image, int i)
        {
            if (image)
            {
                View.MainView.instance.libraryItemTextureReady(image, i);
            }
            else
            {
                View.MainView.instance.libraryItemTextureReady(View.SharedViewResources.instance.webRequestErrorTexture, i);
            }
        }
    }
}