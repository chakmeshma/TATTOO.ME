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
        public GameObject mainMenu;
        public GameObject library;
        public ItemPopulation libraryContent;
        private UIPage _page = UIPage.MainMenu;
        private RectTransform rectTransform;
        private List<Controller.MainController.LibraryItem> libraryLastItems = null;
        private int libraryNumberTextured = 0;
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

        public void appendLibrary(List<Controller.MainController.LibraryItem> newitems)
        {
            int newIndex = libraryContent.lastAddedGameObjects.Count;

            libraryContent.append<Controller.MainController.LibraryItem>(newitems, ItemPopulation.PopulationMode.Grid);

            libraryLastItems.AddRange(newitems);

            for (int i = newIndex; i < libraryContent.lastAddedGameObjects.Count; ++i)
            {
                libraryContent.lastAddedGameObjects[i].GetComponent<LibraryItemView>().info = libraryLastItems[i];
                libraryContent.lastAddedGameObjects[i].GetComponent<LibraryItemView>().updateView();

                Controller.MainController.instance.onLibraryItemLoaded(libraryLastItems, i);
            }

            onResize();
        }

        public void populateLibrary(List<Controller.MainController.LibraryItem> items)
        {
            libraryContent.lastAddedGameObjects = libraryContent.populate<Controller.MainController.LibraryItem>(items, ItemPopulation.PopulationMode.Grid, false);

            libraryLastItems = items;

            for (int i = 0; i < libraryContent.lastAddedGameObjects.Count; ++i)
            {
                libraryContent.lastAddedGameObjects[i].GetComponent<LibraryItemView>().info = items[i];
                libraryContent.lastAddedGameObjects[i].GetComponent<LibraryItemView>().updateView();

                Controller.MainController.instance.onLibraryItemLoaded(libraryLastItems, i);
            }
        }

        private void repositionLibraryPopulation(List<Controller.MainController.LibraryItem> items)
        {
            libraryContent.repositionPopulation<Controller.MainController.LibraryItem>(items, ItemPopulation.PopulationMode.Grid);
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
                    if (libraryLastItems != null)
                    {
                        repositionLibraryPopulation(libraryLastItems);
                    }
                    break;
            }
        }

        public void libraryItemTextureReady(Texture2D picture, int index)
        {
            libraryContent.lastAddedGameObjects[index].GetComponent<LibraryItemView>().updatePicture(picture);
            libraryNumberTextured++;

            if (libraryNumberTextured == libraryLastItems.Count)
            {
                //libraryNumberTextured = 0;
                onResize();
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