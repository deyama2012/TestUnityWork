using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using AxGrid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slots
{
    [Serializable]
    public class ExScroller : MonoBehaviourExtBind
    {
        [SerializeField] private RectTransform mask;
        [SerializeField] private RectTransform itemContainer;

        [Tooltip("Пропорционально высоте экрана")]
        [SerializeField] private float maxScrollSpeed = 2f;
        [SerializeField] private float singleItemHeight = 500f;

        [SerializeField, Range(1, 3f)] private float accelerateTime = 2f;
        [SerializeField, Min(1)] private float decelerateTime = 5f;
        [SerializeField, Min(1)] private int decelerateOffsetItemCount = 2;

        private float scrolledAmount;
        private float scrollSpeed;
        private Dictionary<RectTransform, Fruit> dictTransformToFruit;


        [OnStart]
        private void StartThis()
        {
            var bottomCenter = new Vector2(.5f, 0f);

            itemContainer.pivot = bottomCenter;
            itemContainer.anchorMin = new Vector2(0, 0);
            itemContainer.anchorMax = new Vector2(1, 0);
            itemContainer.anchoredPosition = Vector2.zero;

            var children = itemContainer
                .GetComponentsInChildren<RectTransform>()
                .Where(x => x != itemContainer)
                .ToList();

            foreach (var child in children)
            {
                child.pivot = bottomCenter;
                Instantiate(child, itemContainer);
            }

            // Вертикальное выравнивание по центру трансформа с маской
            scrolledAmount = GetTargetScrollAmount(1);

            // Словарь, чтобы потом по трансформу определить фрукт
            dictTransformToFruit =
                itemContainer
                .GetComponentsInChildren<Fruit>()
                .Select(x => new
                {
                    rectTransform = x.transform as RectTransform,
                    fruit = x
                })
                .ToDictionary(x => x.rectTransform, x => x.fruit);
        }


        [Bind(Names.SPINNER_START_REQUEST)]
        private void StartPath()
        {
            Path = new CPath();
            Path
                .EasingCircEaseIn(accelerateTime, 0, Screen.height * maxScrollSpeed, value =>
                {
                    scrollSpeed = value;
                })
                .Wait(Mathf.Clamp(3 - accelerateTime, 0, 3));
        }


        [Bind(Names.SPINNER_STOP_REQUEST)]
        private void StopPath(Action<Fruit> onCompleteCallback)
        {
            // Отключаем скорость, т.к. будем работать с scrolledAmount (сдвигом по оси Y) напрямую
            scrollSpeed = 0;

            float nextValidScrollAmount = GetTargetScrollAmount(decelerateOffsetItemCount);

            Path = new CPath();
            Path
                .EasingElasticEaseOut(decelerateTime, scrolledAmount, nextValidScrollAmount, value =>
                {
                    scrolledAmount = value;
                })
                .Action(() =>
                {
                    if (TryGetFocusedItem(out var fruit))
                    {
                        onCompleteCallback?.Invoke(fruit);
                    }
                });
        }


        [OnUpdate]
        public void UpdateThis()
        {
            scrolledAmount += scrollSpeed * Time.deltaTime;

            float halfContainerHeight = itemContainer.childCount / 2 * singleItemHeight;

            int index = 0;
            foreach (RectTransform child in itemContainer)
            {
                index++;
                var anchoredPosition = child.anchoredPosition;
                anchoredPosition.y = (-singleItemHeight * index) - (scrolledAmount % halfContainerHeight);
                child.anchoredPosition = anchoredPosition;
            }
        }

        private float GetTargetScrollAmount(int offsetItemCount)
        {
            // округление вверх до ближайшего числа, кратного высоте элементов
            float nextValidScrollAmount = Mathf.Ceil(scrolledAmount / singleItemHeight) * singleItemHeight;
            // дополнительный сдвиг на высоту указанного количества элементов
            nextValidScrollAmount += offsetItemCount * singleItemHeight;
            // сдвиг в обратном направении для того, чтобы вертикально выровнять элемент по центру трансформа с маской
            nextValidScrollAmount -= (mask.rect.height - singleItemHeight) * 0.5f;
            return nextValidScrollAmount;
        }


        private bool TryGetFocusedItem(out Fruit result)
        {
            var maskPosition = (Vector2)mask.position + mask.rect.center;
            result = null;
            foreach (RectTransform child in itemContainer)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(child, maskPosition) &&
                    dictTransformToFruit.TryGetValue(child, out var fruit))
                {
                    result = fruit;
                    return true;
                }
            }
            return false;
        }
    }
}
