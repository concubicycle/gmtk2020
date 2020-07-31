using System.Collections;
using UnityEngine;


namespace Assets.Scripts
{
    class Terminal : MonoBehaviour, IHackable
    {
        public const int RobotLayer = 9;

        public UnityEngine.Experimental.Rendering.Universal.Light2D Light = null;
        public GameObject LevelEndUi = null;
        public bool IsVictoryTerminal = false;
        

        [SerializeField]
        private bool _isHacked = false;

        [SerializeField]
        private float _hackSpeed = 60;

        [SerializeField]
        private string _name = "Unnamed Terminal";

        private Coroutine _hackEffect = null;
        private float _hackedProgress;
        private Color _initialLightColor;

        public bool IsHacked
        {
            get => _isHacked;
            set
            {
                _isHacked = value;
                Light.color = _isHacked ? Color.red : _initialLightColor;

                if (_isHacked && IsVictoryTerminal)
                {
                    LevelEndUi.SetActive(true);
                }
            }
        }

        public string Name => _name;


        private void Start()
        {
            _initialLightColor = Light.color;
            IsHacked = _isHacked;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == RobotLayer)
            {
                var rb = collision.gameObject.GetComponent<StandardRobot>();

                if (rb.IsHacked && !IsHacked)
                    _hackEffect = StartCoroutine(HackSparks());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == RobotLayer)
            {
                var rb = collision.gameObject.GetComponent<StandardRobot>();
                if (rb.IsHacked && _hackEffect != null)
                {
                    StopCoroutine(_hackEffect);
                    _hackedProgress = 0;
                }
            }
        }

        private IEnumerator HackSparks()
        {
            while (_hackedProgress < 100)
            {
                _hackedProgress += Time.deltaTime * _hackSpeed;

                var flickerwait = UnityEngine.Random.Range(0.1f, 0.4f);
                Light.color = Color.red;
                yield return new WaitForSeconds(flickerwait);
                Light.color = _initialLightColor;

                _hackedProgress += flickerwait * _hackSpeed;

                flickerwait = UnityEngine.Random.Range(0.1f, 0.4f);
                yield return new WaitForSeconds(flickerwait);

                _hackedProgress += flickerwait * _hackSpeed;

                yield return 0;
            }

            IsHacked = true;
        }
    }
}
