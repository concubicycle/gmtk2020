using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    class TabView : MonoBehaviour
    {
        public List<Button> TabButtons = new List<Button>();
        public List<GameObject> TabUis = new List<GameObject>();
        public Canvas TabContentArea = null;

        private void Start()
        {
            Func<int, UnityAction> makeCallback = (int index) => () => SetUI(index);

            for (int i = 0; i < TabButtons.Count; ++i)
            {
                TabButtons[i].onClick.AddListener(makeCallback(i));
            }
        }

        private void SetUI(int index)
        {
            for (int i = 0; i < index; i++)
                TabUis[i].SetActive(false);

            TabUis[index++].SetActive(true);

            for (int i = index; i < TabUis.Count; i++)
                TabUis[i].SetActive(false);
        }
    }
}
