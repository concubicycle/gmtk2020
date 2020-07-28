using System.Collections.Generic;
using UnityEngine;
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
            for (int i = 0; i < TabButtons.Count; ++i)
            {
                TabButtons[i].onClick.AddListener(() =>
                {
                    SetUI(i);
                });
            }
        }

        private void SetUI(int index)
        {
            for (int i = 0; i < index; i++)
                TabUis[i].SetActive(false);

            TabUis[index++].SetActive(true);

            for (int i = 0; i < TabUis.Count; i++)
                TabUis[i].SetActive(false);
        }
    }
}
