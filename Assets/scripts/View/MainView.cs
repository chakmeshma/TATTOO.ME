using System.Collections;
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
        public string pictureURL;
    }

    public class MainView : MonoBehaviour
    {
        public static MainView instance = null;
        public enum UIPage
        {
            MainMenu,
            Library
        }
        public GameObject topBar;
        public GameObject mainMenu;
        public GameObject library;
        public ItemPopulation libraryContent;
        private UIPage _page = UIPage.MainMenu;
        private RectTransform rectTransform;
        private List<LibraryItem> lastLibraryItems = null;

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

                    disableAllPages();

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

        private void disableAllPages()
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

            rectTransform = GetComponent<RectTransform>();
        }

        public void onLibrarySelected()
        {
            page = UIPage.Library;
        }

        public void populateLibrary(List<LibraryItem> items, bool append)
        {
            List<GameObject> addedGameObjects = libraryContent.populate<LibraryItem>(items, append, ItemPopulation.PopulationMode.Grid, false);

            lastLibraryItems = items;

            for (int i = 0; i < addedGameObjects.Count; ++i)
            {
                addedGameObjects[i].GetComponent<LibraryItemView>().info = items[i];
                addedGameObjects[i].GetComponent<LibraryItemView>().updateView();
            }
        }

        private void transformLibraryPopulation(List<LibraryItem> items, bool append)
        {
            libraryContent.transformPopulation<LibraryItem>(items, append, ItemPopulation.PopulationMode.Grid);
        }

        private void Start()
        {
            StopAllCoroutines();
            StartCoroutine(resizeWatch());
        }

        void onResize()
        {
            switch (page)
            {
                case UIPage.Library:
                    if (lastLibraryItems != null)
                    {
                        transformLibraryPopulation(lastLibraryItems, false);
                    }
                    break;
            }
        }



        IEnumerator resizeWatch()
        {
            float lastWidth = rectTransform.rect.width * rectTransform.localScale.x;
            float lastHeight = rectTransform.rect.height * rectTransform.localScale.y;

            bool resize = false;
            float since = Time.realtimeSinceStartup;

            while (true)
            {
                float currentWidth = rectTransform.rect.width * rectTransform.localScale.x;
                float currentHeight = rectTransform.rect.height * rectTransform.localScale.y;

                if (currentWidth != lastWidth || currentHeight != lastHeight)
                {
                    resize = true;
                    since = Time.realtimeSinceStartup;
                }

                lastWidth = currentWidth;
                lastHeight = currentHeight;

                if (resize && Time.realtimeSinceStartup - since >= 0.1f)
                {
                    resize = false;
                    onResize();
                }

                yield return new WaitForSecondsRealtime(0.4f);
            }
        }
    }
}