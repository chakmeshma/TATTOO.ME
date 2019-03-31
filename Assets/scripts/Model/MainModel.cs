using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

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
            string[] tattooPictureURL =
            {
                "https://oddstuffmagazine.com/wp-content/uploads/2013/10/Creative-Hand-Tattoo-Designs-in-Vogue-2-610x612.jpg",
                "http://nextluxury.com/wp-content/uploads/geometric-heart-tattoo-designs-for-men.jpg",
                "https://www.inkme.tattoo/wp-content/uploads/2016/05/Couple-Tattoo-Designs-46.jpg",
                "https://customtattoodesign.ca/wp-content/uploads/2016/10/sleeve1-1.jpg",
                "https://www.askideas.com/media/73/Cool-Tribal-Tattoo-Design.jpg",
                "https://images.designtrends.com/wp-content/uploads/2016/09/27172727/Girly-Crown-Tattoo-Design.jpg",
                "https://www.inkme.tattoo/wp-content/uploads/2016/10/forearm-tattoo-design-58.jpg",
                "http://tattoo-journal.com/wp-content/uploads/2015/09/awesome-tattoo-6.jpg",
                "https://assets.shopfront.envato-static.com/page-content/images/related-posts/post-images/200-free-vectors-tribal-graphics--tattoo-designs.jpg",
                "http://drawdoo.com/wp-content/themes/blogfolio/themify/img.php?src=http://drawdoo.com/wp-content/uploads/tutorials/TattooDesigns/lesson07/step_00.png&w=665&h=&zc=1&q=60&a=t"
            };

            for (int i = 0; i < 8; ++i)
            {
                Controller.MainController.LibraryItem info = new Controller.MainController.LibraryItem();

                info.name = " " + Random.Range(1000, 900000000000).ToString();
                info.followers = 100;
                info.likes = 200;
                info.location = "Berlin, Germany";
                info.price = 123.55f;
                info.seller = "Majid Motallebikashani";
                info.sellerInitials = "MM";
                //info.pictureURL = "http://localhost/Tattoos/" + Random.Range(1, 15).ToString() + ".png";
                //info.pictureURL = "https://upload.wikimedia.org/wikipedia/commons/e/e0/Large_Scaled_Forest_Lizard.jpg";
                info.pictureURL = tattooPictureURL[Random.Range(0, 10)];

                items.Add(info);
            }

            numLoadedSet++;
        }

        public void getTattooPicture(int index, string url)
        {
            StartCoroutine(fetchTattooPicture(index, url));
        }

        private int numLoadedSet = 0;

        public void getNewLibraryItems(List<MainController.LibraryItem> newItems)
        {
            if (numLoadedSet <= 3)
            {
                for (int i = 0; i < 10; ++i)
                {
                    Controller.MainController.LibraryItem info = new Controller.MainController.LibraryItem();

                    info.name = " " + Random.Range(1000, 900000000000).ToString();
                    info.followers = 536;
                    info.likes = 350;
                    info.location = "Tehran, Iran";
                    info.price = 898.55f;
                    info.seller = "Shahrokh Shahinfar";
                    info.sellerInitials = "MM";
                    info.pictureURL = "https://static1.squarespace.com/static/55d770c0e4b0b0f70c93dab9/t/5615cb3ae4b0141a0f975cd7/1444268799934/unnamed-3.jpg?format=300w";

                    newItems.Add(info);
                }

                numLoadedSet++;
            }
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