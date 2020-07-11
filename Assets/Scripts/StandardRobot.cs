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

    class StandardRobot : MonoBehaviour
    {
        public RobotState State = RobotState.Routine;
        public float InputTimeout = 0.5f;

        private Coroutine _currentRoutine = null;

        private AIPath _aiPath;
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private void Start()
        {
            _aiPath = GetComponent<AIPath>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            TransitionTo(State);
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
                    StartCoroutine(DoInsane());
                    break;
                case RobotState.Routine:
                    StartCoroutine(DoRoutine());
                    break;
                case RobotState.PlayerControlled:
                    StartCoroutine(DoPlayerControlled());
                    break;
            }
        }

        private IEnumerator DoRoutine()
        {
            _aiPath.enabled = true;
            
            while (State == RobotState.Routine)
            {
                if (Input.GetKey("w") ||
                     (Input.GetKey("s")) ||
                     (Input.GetKey("d")) ||
                     (Input.GetKey("a")))
                {
                    TransitionTo(RobotState.PlayerControlled);
                }

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
                    _animator.Play("Base Layer.Robot_Up");
                    _rigidbody.velocity = new Vector3(0, _aiPath.maxSpeed, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("s"))
                {
                    _animator.Play("Base Layer.Robot_Down");
                    _rigidbody.velocity = new Vector3(0, -_aiPath.maxSpeed, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("d"))
                {
                    _animator.Play("Base Layer.Robot_Right");
                    _rigidbody.velocity = new Vector3(_aiPath.maxSpeed, 0, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else if (Input.GetKey("a"))
                {
                    _animator.Play("Base Layer.Robot_Left");
                    _rigidbody.velocity = new Vector3(-_aiPath.maxSpeed, 0, 0);
                    inputTimeoutRemaining = InputTimeout;
                }
                else
                {
                    var state = _animator.GetCurrentAnimatorStateInfo(0);
                    _animator.Play(state.nameHash, 0, 0);
                    _animator.speed = 0;
                    _rigidbody.velocity = Vector3.zero;
                    inputTimeoutRemaining -= Time.deltaTime;
                }

                if (inputTimeoutRemaining <= 0)
                {
                    TransitionTo(RobotState.Routine);
                }

                yield return 0;
            }
        }

        private IEnumerator DoInsane()
        {
            while (State == RobotState.PlayerControlled)
            {
                yield return 0;
            }
        }

        private IEnumerator DoBroken()
        {
            while (State == RobotState.Broken)
            {
                yield return 0;
            }
        }
    }
}
