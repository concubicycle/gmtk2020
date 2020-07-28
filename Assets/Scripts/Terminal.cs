using System.Collections;
using System.Collections.Generic;
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

        public GameObject LevelEndUi = null;
        public bool IsVictoryTerminal = false;

        private float _hackedProgress;

        [SerializeField]
        private float _hackSpeed = 60;

        private Coroutine _hackEffect = null;


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
