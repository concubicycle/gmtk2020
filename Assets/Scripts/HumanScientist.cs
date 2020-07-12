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
        public const int RobotLayer = 9;

        public string UpAnimation = "Scientist_Up";
        public string RightAnimation = "Scientist_Right";
        public string DownAnimation = "Scientist_Down";
        public string LeftAnimation = "Scientist_Left";

        public ScientistBehavior State = ScientistBehavior.Patrolling;
        public float PatrolStopWaitTime = 1.5f;
        public int SightRayCount = 15;
        public float SightConeRadius = Mathf.PI / 4.0f;
        public float VisibleInsanityCutoff = 80;


        private Patrol _patrol;
        private AIDestinationSetter _dest;
        private Animator _animator;
        private AIPath _aiPath;
        private GameObject _targetRobot = null;
        private Coroutine _currentRoutine = null;

        private Vector2 _forward;

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
                    _patrol.enabled = true;
                    _dest.enabled = false;
                    _aiPath.maxSpeed = 3;
                    _currentRoutine = StartCoroutine(DoPatrol());
                    break;
                case ScientistBehavior.Chasing:
                    _patrol.enabled = false;
                    _dest.enabled = true;
                    _dest.target = _targetRobot.GetComponent<Transform>();
                    _aiPath.maxSpeed = 7;
                    _currentRoutine = StartCoroutine(DoChasing());
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

                _forward = (_aiPath.destination - _aiPath.position).normalized;

                DetectMadRobots();

                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator DoChasing()
        {
            var caughtDistanceSq = 8;

            while (State == ScientistBehavior.Chasing)
            {
                var vel = new Vector2(_aiPath.velocity.x, _aiPath.velocity.y);
                SetAnimatorState(vel);

                var targetTransform = _targetRobot.GetComponent<Transform>();
                var transform = GetComponent<Transform>();
                
                var toTarget = targetTransform.position - transform.position;

                if (toTarget.sqrMagnitude < caughtDistanceSq)
                {
                    var robot = _targetRobot.GetComponent<StandardRobot>();
                    robot.IsHacked = false;
                    
                    TransitionTo(ScientistBehavior.Patrolling);
                }

                yield return 0;
            }
        }

        private void DetectMadRobots()
        {
            float theta = 0;
            float thetaInc = SightConeRadius / SightRayCount;

            // forward ray
            // Cast a ray straight down.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _forward);
            CheckHit(hit);
            
            for (int i = 1; i < SightRayCount; ++i)
            {
                var ray = Vector2Extensions.RotateVector(_forward, i * thetaInc);
                hit = Physics2D.Raycast(transform.position, ray);
                CheckHit(hit);

                ray = Vector2Extensions.RotateVector(_forward, -i * thetaInc);
                hit = Physics2D.Raycast(transform.position, ray);
                CheckHit(hit);
            }
        }

        private bool CheckHit(RaycastHit2D hit)
        {
            // If it hits something...
            if (hit.collider != null && hit.collider.gameObject.layer == RobotLayer)
            {
                var sanity = hit.collider.gameObject.GetComponent<Sanity>();
                var robot = hit.collider.gameObject.GetComponent<StandardRobot>();

                if (robot.IsHacked)
                {
                    _targetRobot = hit.collider.gameObject;
                    TransitionTo(ScientistBehavior.Chasing);
                }
                
                return true;
            }

            return false;
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
