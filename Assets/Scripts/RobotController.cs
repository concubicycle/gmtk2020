using FMOD;
using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    enum RobState
    {
        Routine,
        PlayerControlled,
    }

    class RobotController : MonoBehaviour
    {
        private AIPath _aiPath;
        private Rigidbody2D _rigidBody;
        private Animator _animator;
        private Sanity _sanity;
        private Health _health;

        public RobState currentState = RobState.Routine;

        public float moveSpeed = 3f;
        public float InputTimeout = 0.5f;
        public float TerminalWaitTime = 1.0f;
        public float SanityDrain = 25;
        public float SanityRegain = 15;
        public bool isControlled = true;

        private Coroutine _currentRoutine = null;

        Vector2 movement;

        private void Start() {
            _aiPath = GetComponent<AIPath>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sanity = GetComponent<Sanity>();
            _health = GetComponent<Health>();

            TransitionTo(currentState);
        }

        void Update()
        {
            _animator.SetFloat("MoveX", movement.x);
            _animator.SetFloat("MoveY", movement.y);
            _animator.SetFloat("Speed", movement.sqrMagnitude);

            if ( movement.x >= 0.5 || movement.x <= -0.5 || movement.y >= 0.5 || movement.y <= -0.5 ) 
            {
                _animator.SetFloat("LastMoveX", movement.x);
                _animator.SetFloat("LastMoveY", movement.y);
            }
        }

        private void TransitionTo(RobState newState)
        {
            currentState = newState;

            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }

            switch (newState)
            {   
                case RobState.Routine:
                    _aiPath.enabled = true;
                    _currentRoutine = StartCoroutine(DoRoutine());
                    break;
                case RobState.PlayerControlled:
                    _currentRoutine = StartCoroutine(DoPlayerControlled());
                    break;
            }
        }

        private IEnumerator DoRoutine()
        {
            float timeSinceDestinationCutoff = 1.0f;
            float timeSinceDestination = timeSinceDestinationCutoff + 1.0f;

            Vector3 lastDestination = _aiPath.destination;

            while(currentState == RobState.Routine)
            {
                var sanityFull = (_sanity.maxSanity - _sanity.SanityPoints) < 2;

                if (sanityFull && isControlled &&
                    (Input.GetKey("w") ||
                     Input.GetKey("s") ||
                     Input.GetKey("d") ||
                     Input.GetKey("a")))
                {
                    TransitionTo(RobState.PlayerControlled);
                } else {
                    var aiMovement = new Vector2(_aiPath.velocity.x, _aiPath.velocity.y).normalized;
                    movement.x = aiMovement.x;
                    movement.y = aiMovement.y;
                }              

                if (lastDestination != _aiPath.destination)
                {
                    movement = Vector2.zero;
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

            movement = Vector2.zero;

            while (currentState == RobState.PlayerControlled)
            {
                // UnityEngine.Debug.Log(currentState);
                if (Input.GetKey("w") || 
                    Input.GetKey("a") || 
                    Input.GetKey("s") || 
                    Input.GetKey("d"))
                {
                    inputTimeoutRemaining = InputTimeout;

                    // Input
                    movement.x = Input.GetAxisRaw("Horizontal");
                    movement.y = Input.GetAxisRaw("Vertical");

                    // Movement
                    _rigidBody.MovePosition(_rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);

                } else {
                    var state = _animator.GetCurrentAnimatorStateInfo(0);
                    _animator.Play(state.nameHash, 0, 0);
                    inputTimeoutRemaining -= Time.deltaTime;
                }

                if (inputTimeoutRemaining <= 0 || 
                    _sanity.SanityPoints <= 0 || 
                    !isControlled) 
                {
                    TransitionTo(RobState.Routine);
                }

                _sanity.SanityPoints -= Time.deltaTime * SanityDrain;

                yield return 0;
            }
        }
    }
}
