using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
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
        private List<Controller.MainController.LibraryItem> lastLibraryItems = null;
        private List<GameObject> lastAddedGameObjects = null;

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

        public void populateLibrary(List<Controller.MainController.LibraryItem> items, bool append)
        {
            lastAddedGameObjects = libraryContent.populate<Controller.MainController.LibraryItem>(items, append, ItemPopulation.PopulationMode.Grid, false);

            lastLibraryItems = items;

            for (int i = 0; i < lastAddedGameObjects.Count; ++i)
            {
                lastAddedGameObjects[i].GetComponent<LibraryItemView>().info = items[i];
                lastAddedGameObjects[i].GetComponent<LibraryItemView>().updateView();

                Controller.MainController.instance.onLibraryItemLoaded(lastLibraryItems, i);
            }
        }

        private void repositionLibraryPopulation(List<Controller.MainController.LibraryItem> items, bool append)
        {
            libraryContent.repositionPopulation<Controller.MainController.LibraryItem>(items, append, ItemPopulation.PopulationMode.Grid);
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
                        repositionLibraryPopulation(lastLibraryItems, false);
                    }
                    break;
            }
        }

        public void updateLibraryItemTexture(Texture2D picture, int i)
        {
            lastAddedGameObjects[i].GetComponent<LibraryItemView>().updatePicture(picture);
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