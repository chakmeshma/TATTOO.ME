using Battlehub.UIControls.DockPanels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace View
{
    public class MainView : MonoBehaviour
    {
        public DockPanel dockPanel;
        public static MainView instance = null;
        public enum UIPage
        {
            MainMenu,
            Library
        }
        public GameObject mainMenu;
        public GameObject library;
        public ItemPopulation libraryContent;
        private RectTransform rectTransform;
        private List<Controller.MainController.LibraryItem> libraryLastItems = null;
        private int libraryNumberTextured = 0;
        private bool libraryInited = false;
        private bool libraryEnabled = false;
        private RegionEventHandler<Transform> lastLibraryCloseHandler = null;

        private void closeHandler(Region region, Transform arg)
        {
            switch (arg.name)
            {
                case "Library":
                    libraryEnabled = false;
                    libraryToggleAct();
                    break;
            }
        }

        private void libraryToggleAct()
        {
            if (libraryEnabled)
            {
                library.SetActive(true);

                dockPanel.RootRegion.Add(null, "Library", library.transform);

                if (lastLibraryCloseHandler != null)
                {
                    dockPanel.TabClosed -= lastLibraryCloseHandler;
                }

                dockPanel.TabClosed += (lastLibraryCloseHandler = new RegionEventHandler<Transform>(closeHandler));

                StartCoroutine(resizeWatch(UIPage.Library));
            }
            else
            {

                StopAllCoroutines();

                GameObject newLibrary = Instantiate(library, transform);

                if (lastLibraryCloseHandler != null)
                {
                    dockPanel.TabClosed -= lastLibraryCloseHandler;
                }

                Region libraryRegion = GetComponentsInChildren<Transform>().Where(r => r.name == "Library").First().GetComponentInParent<Region>();

                libraryRegion.IsSelected = true;

                libraryRegion.RemoveAt(libraryRegion.ActiveTabIndex);

                newLibrary.name = "Library";
                library = newLibrary;

                libraryContent = library.GetComponentInChildren<ItemPopulation>();

                library.SetActive(false);
            }
        }

        public void onLibraryToggle()
        {
            libraryEnabled = !libraryEnabled;
            libraryToggleAct();
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

            onResize(UIPage.Library);
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

        public void libraryItemTextureReady(Texture2D picture, int index)
        {
            if (!libraryEnabled)
            {
                return;
            }

            libraryContent.lastAddedGameObjects[index].GetComponent<LibraryItemView>().updatePicture(picture);
            libraryNumberTextured++;

            if (libraryNumberTextured == libraryLastItems.Count)
            {
                //libraryNumberTextured = 0;
                onResize(UIPage.Library);
            }
        }

        private void repositionLibraryPopulation(List<Controller.MainController.LibraryItem> items)
        {
            libraryContent.repositionPopulation<Controller.MainController.LibraryItem>(items, ItemPopulation.PopulationMode.Grid);
        }

        //private void disableAllPages()
        //{
        //    mainMenu.SetActive(false);
        //    library.SetActive(false);
        //}

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }

            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            StopAllCoroutines();
            //StartCoroutine(resizeWatch());
        }

        private void onResize(UIPage page)
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


        IEnumerator resizeWatch(UIPage page)
        {
            RectTransform rectTransform = null;

            switch (page)
            {
                case UIPage.Library:

                    rectTransform = library.GetComponent<RectTransform>();
                    break;
            }

            if (rectTransform)
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
                        onResize(page);
                    }

                    yield return new WaitForSecondsRealtime(0.4f);
                }
            }
        }
    }
}