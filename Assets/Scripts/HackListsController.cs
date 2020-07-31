using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    
    class HackListsController : MonoBehaviour
    {
        public GameObject ButtonPrefab = null;
        public float UiUpdateInterval = 1;

        public RectTransform robotButtonList = null;
        public RectTransform terminalButtonList = null;


        private Terminal[] _terminals;
        private StandardRobot[] _robots;

        private Dictionary<Terminal, GameObject> _terminalButtons = new Dictionary<Terminal, GameObject>();
        private Dictionary<StandardRobot, GameObject> _robotButtons = new Dictionary<StandardRobot, GameObject>();


        private void Start()
        {
            // for now, there is a fixed number of robots and terminals 
            // in the scene. Lists will need to be updated later,
            // when this is no longer true. 
            _terminals = FindObjectsOfType<Terminal>();
            _robots = FindObjectsOfType<StandardRobot>();

            StartCoroutine(UpdateUi());
        }

        private IEnumerator UpdateUi()
        {
            while (true)
            {
                foreach (var t in _terminals)
                {
                    if (t.IsHacked) AddButton(t);
                    else RemoveButton(t);
                }

                foreach (var r in _robots)
                {
                    if (r.IsHacked) AddButton(r);
                    else RemoveButton(r);
                }


                yield return new WaitForSeconds(UiUpdateInterval);
            }
        }

        private void AddButton(Terminal t)
        {
            if (_terminalButtons.ContainsKey(t)) return;

            _terminalButtons[t] = Instantiate(ButtonPrefab);
            var button = _terminalButtons[t].GetComponent<HackableItemButton>();
            var buttonRect = _terminalButtons[t].GetComponent<RectTransform>();
            button.ConnectedItem = t;
            buttonRect.parent = terminalButtonList;
        }

        private void AddButton(StandardRobot r)
        {
            if (_robotButtons.ContainsKey(r)) return;

            _robotButtons[r] = Instantiate(ButtonPrefab);
            var button = _robotButtons[r].GetComponent<HackableItemButton>();
            var buttonRect = _robotButtons[r].GetComponent<RectTransform>();
            
            button.ConnectedItem = r;
            buttonRect.parent = robotButtonList;
        }

        private void RemoveButton(Terminal t)
        {
            if (!_terminalButtons.ContainsKey(t)) return;

            var button = _terminalButtons[t];
            Destroy(button);
        }

        private void RemoveButton(StandardRobot r)
        {
            if (!_robotButtons.ContainsKey(r)) return;

            var button = _robotButtons[r];
            Destroy(button);
        }
    }
}
