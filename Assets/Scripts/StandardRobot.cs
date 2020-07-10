using FMOD;
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
        Returning
    }

    class StandardRobot : MonoBehaviour
    {
        public RobotState State = RobotState.Routine;

        private Coroutine _currentRoutine = null;

        private void Start()
        {
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
                case RobotState.Returning:
                    StartCoroutine(DoReturning());
                    break;
            }
        }

        private IEnumerator DoRoutine()
        {
            while (State == RobotState.Routine)
            {
                yield return 0;
            }
        }

        private IEnumerator DoPlayerControlled()
        {
            while (State == RobotState.PlayerControlled)
            {
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

        private IEnumerator DoReturning()
        {
            while (State == RobotState.Returning)
            {
                yield return 0;
            }
        }
    }
}
