using System.Collections;
using UnityEngine;


namespace Assets.Scripts
{
    [RequireComponent(typeof(Hackable))]
    class Terminal : MonoBehaviour
    {
        public const int RobotLayer = 9;

        public UnityEngine.Experimental.Rendering.Universal.Light2D Light = null;
        public GameObject LevelEndUi = null;
        public bool IsVictoryTerminal = false;

        [SerializeField]
        private float _hackSpeed = 60;

        private Hackable _hackable;
        private Coroutine _hackEffect = null;
        private float _hackedProgress;
        private Color _initialLightColor;        

        private void Awake()
        {
            _hackable = GetComponent<Hackable>();
        }

        private void Start()
        {
            _initialLightColor = Light.color;
            _hackable.HackedStatusChanged += value =>
            {
                Light.color = _hackable.IsHacked ? Color.red : _initialLightColor;

                if (_hackable.IsHacked && IsVictoryTerminal)
                {
                    LevelEndUi.SetActive(true);
                }
            };

            Light.color = _hackable.IsHacked ? Color.red : _initialLightColor;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == RobotLayer)
            {
                var hackable = collision.gameObject.GetComponent<Hackable>();

                if (hackable.IsHacked && !_hackable.IsHacked)
                    _hackEffect = StartCoroutine(HackSparks());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == RobotLayer)
            {
                var hackable = collision.gameObject.GetComponent<Hackable>();
                if (hackable.IsHacked && _hackEffect != null)
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

            _hackable.IsHacked = true;
        }
    }
}
