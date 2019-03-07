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
        private UIPage _page = UIPage.MainMenu;
        public GameObject[] mainMenuElements;
        public GameObject[] libraryElements;

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
                            foreach(GameObject go in mainMenuElements)
                            {
                                go.SetActive(true);
                            }
                            break;
                        case UIPage.Library:
                            foreach (GameObject go in libraryElements)
                            {
                                go.SetActive(true);
                            }
                            break;
                    }

                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        private void disableAll()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
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
    }
}