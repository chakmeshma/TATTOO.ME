using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    [RequireComponent(typeof(RectTransform))]
    public class ItemPopulation : MonoBehaviour
    {
        public enum PopulationMode
        {
            Grid,
            Stack
        }
        private int nColumns = 0;
        public RectTransform body;
        public UnityEngine.Object libraryItem;
        private float itemWidth;
        private float itemHeight;
        private float contentWidth;
        public float verticalPadding;
        public float horizontalPadding;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void clearAll()
        {
            foreach (Transform goTransform in transform)
            {
                Destroy(goTransform.gameObject);
            }
        }

        public void populate(List<LibraryItem> items, bool append, PopulationMode populationMode)
        {
            switch (populationMode)
            {
                case PopulationMode.Grid:
                    if (items.Count == 0)
                    {
                        return;
                    }
                    if (append)
                    {

                    }
                    else
                    {
                        clearAll();

                        if (nColumns == 0)
                        {
                            StopAllCoroutines();
                            StartCoroutine(calculatenColumns(items, append, PopulationMode.Grid));

                            return;
                        }
                        else
                        {
                            int nRows = items.Count / nColumns;
                            int remainder = items.Count % nColumns;
                            if (remainder != 0)
                            {
                                ++nRows;
                            }

                            int index = -1;

                            for (int i = 0; i < nRows; ++i)
                            {
                                for (int j = 0; j < nColumns && (index = (i * nColumns + j)) < items.Count; ++j)
                                {
                                    GameObject go = Instantiate(libraryItem, transform) as GameObject;

                                    float remainderPadding = (contentWidth - (((nColumns - 1) * verticalPadding) + (nColumns * itemWidth))) / 2.0f;

                                    go.GetComponent<RectTransform>().anchoredPosition = new Vector2(j * (itemWidth + horizontalPadding) + remainderPadding, -i * (itemHeight + verticalPadding) - verticalPadding);

                                }
                            }

                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nRows * (itemHeight + verticalPadding)) + verticalPadding);
                        }

                    }
                    break;
            }

        }

        private IEnumerator calculatenColumns(List<LibraryItem> items, bool append, PopulationMode populationMode)
        {
            yield return new WaitForEndOfFrame();

            GameObject probeItem = Instantiate(libraryItem, transform) as GameObject;

            itemWidth = probeItem.GetComponent<RectTransform>().rect.width;
            itemHeight = probeItem.GetComponent<RectTransform>().rect.height;
            contentWidth = body.GetComponent<RectTransform>().rect.width;

            Destroy(probeItem);

            nColumns = Mathf.FloorToInt(contentWidth / itemWidth);

            populate(items, append, populationMode);
        }
    }
}