using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Scripts
{
    class Terminal : MonoBehaviour, IHackable
    {
        public const int RobotLayer = 9;

        public UnityEngine.Experimental.Rendering.Universal.Light2D Light = null;

        [SerializeField]
        private bool _isHacked = false;

        private Color _initialLightColor;

        private void Start()
        {
            _initialLightColor = Light.color;
            IsHacked = _isHacked;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == RobotLayer)
            {
                var rb = 
                    collision.gameObject.GetComponent<StandardRobot>();

                if (rb.IsHacked)
                {
                    IsHacked = true;
                }
            }
        }

        public bool IsHacked
        {
            get => _isHacked;
            set
            {
                _isHacked = value;
                Light.color = _isHacked ? Color.red : _initialLightColor;
            }
        }
    }
}
