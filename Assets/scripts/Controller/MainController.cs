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
                    View.LibraryItem info = new View.LibraryItem();

                    info.name = " " + Random.Range(1000, 900000000000).ToString();

                    items.Add(info);
                }

                View.MainView.instance.populateLibrary(items, false);
            }
        }
    }
}