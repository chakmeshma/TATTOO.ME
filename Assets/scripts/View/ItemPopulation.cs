using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public float verticalPadding;
        public float horizontalPadding;
        public RectTransform body;
        public Object itemPrefab;
        private int nColumns = 0;
        private float itemWidth;
        private float itemHeight;
        private float contentWidth;
        private RectTransform rectTransform;
        private List<GameObject> lastAddedGameObjects = new List<GameObject>();
        private float[] accs = null;
        float accLargestDiff = 0.0f;

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

        public void contentReset()
        {
            clearAll();
            nColumns = 0;
        }

        public void repositionPopulation<T>(List<T> items, PopulationMode populationMode)
        {
            nColumns = 0;
            populate<T>(items, populationMode, true);
        }

        public List<GameObject> populate<T>(List<T> items, PopulationMode populationMode, bool resize)
        {
            if (accs == null)
            {
                accs = new float[items.Count];

                for (int i = 0; i < accs.Length; ++i)
                {
                    accs[i] = 0.0f;
                }
            }

            switch (populationMode)
            {
                case PopulationMode.Grid:

                    if (items.Count == 0)
                    {
                        return null;
                    }

                    if (!resize)
                    {
                        clearAll();
                    }

                    if (nColumns == 0)
                    {
                        GameObject probeItem = Instantiate(itemPrefab, transform) as GameObject;

                        itemWidth = probeItem.GetComponent<RectTransform>().rect.width;
                        itemHeight = probeItem.GetComponent<RectTransform>().rect.height;
                        contentWidth = body.GetComponent<RectTransform>().rect.width;

                        Destroy(probeItem);

                        nColumns = Mathf.FloorToInt(contentWidth / itemWidth);

                        accLargestDiff = addPositionAllAcc(true);

                        return populate<T>(items, populationMode, resize);
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
                                GameObject go = null;

                                if (resize)
                                {
                                    go = transform.GetChild(index).gameObject;

                                }
                                else
                                {
                                    go = Instantiate(itemPrefab, transform) as GameObject;

                                    lastAddedGameObjects.Add(go);
                                }


                                //float remainderPadding = (contentWidth - (((nColumns - 1) * verticalPadding) + (nColumns * itemWidth))) / 2.0f;

                                //go.GetComponent<RectTransform>().anchoredPosition = new Vector2(j * (itemWidth + horizontalPadding) + remainderPadding, -i * (itemHeight + verticalPadding) - verticalPadding);

                                float emptySpace = contentWidth - nColumns * itemWidth;
                                float padding = emptySpace / (nColumns + 1);
                                if (resize)
                                {
                                    go.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding * (j + 1) + itemWidth * j, -i * (itemHeight + verticalPadding) - verticalPadding - accs[index]);
                                }
                                else
                                {
                                    go.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding * (j + 1) + itemWidth * j, -i * (itemHeight + verticalPadding) - verticalPadding);
                                }
                            }
                        }

                        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nRows * (itemHeight + verticalPadding)) + 2 * verticalPadding + accLargestDiff);
                    }

                    return lastAddedGameObjects;
            }

            return null;

        }

        public float addPositionAllAcc(bool clear)
        {
            if (clear)
            {
                for (int i = 0; i < lastAddedGameObjects.Count; ++i)
                {
                    accs[i] = 0.0f;
                }
            }

            float max = 0;

            for (int i = 0; i < lastAddedGameObjects.Count; ++i)
            {
                float innerMax = addPositionAcc(i);

                if (innerMax > max)
                {
                    max = innerMax;
                }
            }

            return max;
        }

        public float addPositionAcc(int index)
        {
            float max = 0;

            for (int i = index + nColumns; i < lastAddedGameObjects.Count; i += nColumns)
            {
                float newHeight = lastAddedGameObjects[index].GetComponent<RectTransform>().rect.height;
                accs[i] += newHeight - itemHeight;

                if (accs[i] > max)
                {
                    max = accs[i];
                }
            }

            return max;
        }
    }
}