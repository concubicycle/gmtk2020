using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Scripts
{
    class Terminal : MonoBehaviour, IHackable
    {
        public const int RobotLayer = 9;

        public UnityEngine.Experimental.Rendering.Universal.Light2D Light;

        [SerializeField]
        private bool _isHacked = false;

        private void Start()
        {
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
            get { return _isHacked; }
            set
            {
                _isHacked = value;

                if (_isHacked)
                {
                    Light.color = Color.red;
                }
            }
        }
    }
}
