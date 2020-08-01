using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Hackable : MonoBehaviour
    {
        public event Action<bool> HackedStatusChanged;        

        public string Name = "Unnamed item";

        public bool IsHacked
        {
            get => _isHacked;
            set
            {
                _isHacked = value;
                HackedStatusChanged?.Invoke(value);
            }
        }

        public bool IsControlled
        {
            get => _isControlled;
            set
            {
                _isControlled = value;                
            }
        }

        [SerializeField]
        private bool _isHacked;

        [SerializeField]
        private bool _isControlled;
    }
}
