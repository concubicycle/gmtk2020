using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    enum ScientistBehavior
    {
        Patrolling,
        Chasing,
        Recycling
    }


    class HumanScientist : MonoBehaviour
    {
        public string UpAnimation = "Scientist_Up";
        public string RightAnimation = "Scientist_Right";
        public string DownAnimation = "Scientist_Down";
        public string LeftAnimation = "Scientist_Left";

        public ScientistBehavior State = ScientistBehavior.Patrolling;
        public float PatrolStopWaitTime = 1.5f;

        private Patrol _patrol;
        private AIDestinationSetter _dest;
        private Animator _animator;
        private AIPath _aiPath;
        private GameObject _targetRobot = null;
        private Coroutine _currentRoutine = null;

        private void Start()
        {
            _patrol = GetComponent<Patrol>();
            _dest = GetComponent<AIDestinationSetter>();
            _animator = GetComponent<Animator>();
            _aiPath = GetComponent<AIPath>();

            _patrol.enabled = true;
            _dest.enabled = false;

            TransitionTo(State);
        }

        private void TransitionTo(ScientistBehavior state)
        {
            State = state;

            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }

            switch (state)
            {
                case ScientistBehavior.Patrolling:
                    _aiPath.enabled = true;
                    _currentRoutine = StartCoroutine(DoPatrol());
                    break;
                case ScientistBehavior.Chasing:
                    break;
                case ScientistBehavior.Recycling:
                    break;
            }
        }

        private IEnumerator DoPatrol()
        {
            float timeSinceDestinationCutoff = 1.0f;
            float timeSinceDestination = timeSinceDestinationCutoff + 1.0f;

            Vector3 lastDestination = _aiPath.destination;

            while (State == ScientistBehavior.Patrolling)
            {
                var vel = new Vector2(_aiPath.velocity.x, _aiPath.velocity.y);
                SetAnimatorState(vel);

                if (lastDestination != _aiPath.destination)
                {
                    lastDestination = _aiPath.destination;
                    _aiPath.enabled = false;
                    yield return new WaitForSeconds(PatrolStopWaitTime);
                    _aiPath.enabled = true;
                }
                else
                {
                    timeSinceDestination += Time.deltaTime;
                }

                // detect enemies here

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
