using FMOD;
using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    enum RobotState
    {
        Routine,
        PlayerControlled,
        Insane,
        Broken
    }

    [RequireComponent(typeof(Hackable))]
    class StandardRobot : MonoBehaviour
    {
        public const int TerminalLayer = 10;

        public string UpAnimation = "Robot_Up";
        public string RightAnimation = "Robot_Right";
        public string DownAnimation = "Robot_Down";
        public string LeftAnimation = "Robot_Left";


        public RobotState State = RobotState.Routine;
        public float InputTimeout = 0.5f;
        public float Sanity = 10.0f;
        public float TerminalWaitTime = 1.0f;
        
        public float SanityDrain = 25;
        public float SanityRegain = 15;
        public UnityEngine.Experimental.Rendering.Universal.Light2D Light = null;

        private Coroutine _currentRoutine = null;

        private AILerp _aiPath;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Sanity _sanity;
        private Health _health;
        private Hackable _hackable;


        private void Awake()
        {
            _aiPath = GetComponent<AILerp>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sanity = GetComponent<Sanity>();
            _health = GetComponent<Health>();
            _hackable = GetComponent<Hackable>();
        }

        private void Start()
        {
            Light.gameObject.SetActive(_hackable.IsHacked);
            _hackable.HackedStatusChanged += (bool value) => Light.gameObject.SetActive(value);
            TransitionTo(State);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == TerminalLayer)
            {
                var terminal = collision.gameObject.GetComponent<Hackable>();
                if (terminal.IsHacked)
                {
                    _hackable.IsHacked = true;
                }
            }
        }
      

        private void Update()
        {
            if (Sanity < 0 &&
                State != RobotState.Broken &&
                State != RobotState.Insane)
            {
                TransitionTo(RobotState.Insane);
            }
        }

        private void TransitionTo(RobotState state)
        {
            State = state;

            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }

            switch (state)
            {   
                case RobotState.Insane:
                    _currentRoutine = StartCoroutine(DoInsane());
                    break;
                case RobotState.Routine:
                    _aiPath.enabled = true;
                    _currentRoutine = StartCoroutine(DoRoutine());
                    break;
                case RobotState.PlayerControlled:
                    _currentRoutine = StartCoroutine(DoPlayerControlled());
                    break;
            }
        }

        private IEnumerator DoRoutine()
        {
            float timeSinceDestinationCutoff = 1.0f;
            float timeSinceDestination = timeSinceDestinationCutoff + 1.0f;

            Vector3 lastDestination = _aiPath.destination;

            yield return new WaitForSeconds(0.5f);

            while (State == RobotState.Routine)
            {
                var sanityFulll = (_sanity.maxSanity - _sanity.SanityPoints) < 2;

                if (sanityFulll && _hackable.IsControlled &&
                    (Input.GetKey("w") ||
                     Input.GetKey("s") ||
                     Input.GetKey("d") ||
                     Input.GetKey("a")))
                {
                    TransitionTo(RobotState.PlayerControlled);
                }

                var dir = _aiPath.WalkDirection;
                var vel = new Vector2(dir.x, dir.y);

                SetAnimatorState(vel);

                if (lastDestination != _aiPath.destination)
                {
                    lastDestination = _aiPath.destination;
                    _aiPath.enabled = false;
                    yield return new WaitForSeconds(TerminalWaitTime);
                    _aiPath.enabled = true;
                }
                else
                {
                    timeSinceDestination += Time.deltaTime;
                }

                _sanity.SanityPoints += Time.deltaTime * SanityRegain;

                yield return 0;
            }
        }

        private IEnumerator DoPlayerControlled()
        {
            float inputTimeoutRemaining = InputTimeout;
            _aiPath.enabled = false;

            while (State == RobotState.PlayerControlled)
            {
                if (Input.GetKey("w"))
                {
                    _rigidbody.velocity = new Vector3(0, _aiPath.speed, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("s"))
                {
                    _rigidbody.velocity = new Vector3(0, -_aiPath.speed, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("d"))
                {
                    _rigidbody.velocity = new Vector3(_aiPath.speed, 0, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("a"))
                {
                    _rigidbody.velocity = new Vector3(-_aiPath.speed, 0, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else
                {
                    var state = _animator.GetCurrentAnimatorStateInfo(0);
                    _animator.Play(state.nameHash, 0, 0);
                    //_animator.speed = 0;
                    _rigidbody.velocity = Vector3.zero;
                    inputTimeoutRemaining -= Time.deltaTime;
                }

                if (inputTimeoutRemaining <= 0 || !_hackable.IsControlled)
                {
                    TransitionTo(RobotState.Routine);
                }

                if (_sanity.SanityPoints <= 0)
                {
                    TransitionTo(RobotState.Routine);
                }

                SetAnimatorState(_rigidbody.velocity);
                _sanity.SanityPoints -= Time.deltaTime * SanityDrain;
                yield return 0;
            }
        }

        private IEnumerator DoInsane()
        {
            _hackable.IsHacked = false;

            while (State == RobotState.PlayerControlled)
            {
                yield return 0;
            }

            TransitionTo(RobotState.Routine);
        }

        private IEnumerator DoBroken()
        {
            while (State == RobotState.Broken)
            {
                yield return 0;
            }
        }

        private void SetAnimatorState(Vector2 velocity)
        {
            var vnorm = velocity.normalized;
            var up = Vector2.up;
            var dprod = Vector2.Dot(vnorm, up);

            if (dprod > 0.70710678118f)
                _animator.Play("Base Layer." + UpAnimation);
            else if (dprod < -0.70710678118f)
                _animator.Play("Base Layer." + DownAnimation);
            else if (vnorm.x > 0)
                _animator.Play("Base Layer." + RightAnimation);
            else
                _animator.Play("Base Layer." + LeftAnimation);
        }
    }
}
