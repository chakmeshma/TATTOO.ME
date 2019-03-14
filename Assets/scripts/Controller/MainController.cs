using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                List<Controller.MainController.LibraryItem> items = new List<Controller.MainController.LibraryItem>();

                Model.MainModel.instance.getLibraryItems(items);

                View.MainView.instance.populateLibrary(items);
            }
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