﻿using System.Collections;
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
        public Object item;
        private int nColumns = 0;
        private float itemWidth;
        private float itemHeight;
        private float contentWidth;
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

        public void contentReset()
        {
            clearAll();
            nColumns = 0;
        }

        public void transformPopulation<T>(List<T> items, bool append, PopulationMode populationMode)
        {
            nColumns = 0;
            populate<T>(items, append, populationMode, true);
        }

        public void populate<T>(List<T> items, bool append, PopulationMode populationMode, bool resize)
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

                        if (!resize)
                        {
                            clearAll();
                        }

                        if (nColumns == 0)
                        {
                            StopAllCoroutines();
                            StartCoroutine(calculatenColumns<T>(items, append, PopulationMode.Grid));

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
                            int k = 0;
                            for (int i = 0; i < nRows; ++i)
                            {
                                for (int j = 0; j < nColumns && (index = (i * nColumns + j)) < items.Count; ++j)
                                {
                                    GameObject go = null;

                                    if (resize)
                                    {
                                        go = transform.GetChild(k).gameObject;

                                        ++k;
                                    }
                                    else
                                    {
                                        go = Instantiate(item, transform) as GameObject;
                                    }

                                    //float remainderPadding = (contentWidth - (((nColumns - 1) * verticalPadding) + (nColumns * itemWidth))) / 2.0f;

                                    //go.GetComponent<RectTransform>().anchoredPosition = new Vector2(j * (itemWidth + horizontalPadding) + remainderPadding, -i * (itemHeight + verticalPadding) - verticalPadding);

                                    float emptySpace = contentWidth - nColumns * itemWidth;
                                    float padding = emptySpace / (nColumns + 1);

                                    go.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding * (j + 1) + itemWidth * j, -i * (itemHeight + verticalPadding) - verticalPadding);

                                }
                            }

                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nRows * (itemHeight + verticalPadding)) + verticalPadding);
                        }

                    }
                    break;
            }

        }

        private IEnumerator calculatenColumns<T>(List<T> items, bool append, PopulationMode populationMode)
        {
            yield return new WaitForEndOfFrame();

            GameObject probeItem = Instantiate(item, transform) as GameObject;

            itemWidth = probeItem.GetComponent<RectTransform>().rect.width;
            itemHeight = probeItem.GetComponent<RectTransform>().rect.height;
            contentWidth = body.GetComponent<RectTransform>().rect.width - 20;

            Destroy(probeItem);

            nColumns = Mathf.FloorToInt(contentWidth / itemWidth);

            populate<T>(items, append, populationMode, false);
        }
    }
}