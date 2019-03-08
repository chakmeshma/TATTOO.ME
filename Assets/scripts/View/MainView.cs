using System.Collections.Generic;
using UnityEngine;

namespace View
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
        public Sprite shape;
    }

    public class MainView : MonoBehaviour
    {
        public RectTransform libraryContent;
        public static MainView instance = null;
        public enum UIPage
        {
            MainMenu,
            Library
        }
        private UIPage _page = UIPage.MainMenu;
        public GameObject topBar;
        public GameObject mainMenu;
        public GameObject library;

        public UIPage page
        {
            get
            {
                return _page;
            }

            set
            {
                if (_page != value)
                {
                    _page = value;

                    disableAll();

                    switch (_page)
                    {
                        case UIPage.MainMenu:
                            mainMenu.SetActive(true);
                            break;
                        case UIPage.Library:
                            library.SetActive(true);
                            break;
                    }

                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        private void disableAll()
        {
            mainMenu.SetActive(false);
            library.SetActive(false);
        }

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
        }

        public void onLibrarySelected()
        {
            page = UIPage.Library;
        }

        public void populate(List<LibraryItem> items, bool append)
        {
            if(!append)
            {

            }
        }
    }
}