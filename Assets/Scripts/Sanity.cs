using UnityEngine;

namespace Assets.Scripts
{
    public delegate void OnInsaneHandler();
    public delegate void OnSanityChanged(float hp, float hpMax);


    class Sanity : MonoBehaviour
    {
        public event OnInsaneHandler EntityInsaneListeners;
        public event OnSanityChanged SanityChangedListeners;
                
        public float maxSanity = 100.0f;

        [SerializeField]
        private float _controlPoints = 100;

        public float SanityPoints
        {
            get
            {
                return _controlPoints;
            }
            set
            {
                if (_controlPoints > 0 && value <= 0 && EntityInsaneListeners != null)
                    EntityInsaneListeners();

                if (value <= 0)
                    _controlPoints = 0;
                else if (value > maxSanity)
                    _controlPoints = maxSanity;
                else
                    _controlPoints = value;

                SanityChangedListeners?.Invoke(_controlPoints, maxSanity);
            }
        }

        void Update()
        {
            SanityPoints = _controlPoints;
        }
    }
}
