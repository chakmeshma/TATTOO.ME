using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class MainController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                List<View.LibraryItem> items = new List<View.LibraryItem>();

                for (int i = 0; i < 10; ++i)
                {
                    items.Add(new View.LibraryItem());
                }

                View.MainView.instance.populateLibrary(items, false);
            }
        }
    }
}