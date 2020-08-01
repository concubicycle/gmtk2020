using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    class FlickerOnHacked : MonoBehaviour
    {
        private Terminal _terminal;
        private Hackable _hackable;

        public float InitialWait = 3.0f;

        private void Start()
        {
            _terminal = GetComponent<Terminal>();
            _hackable = GetComponent<Hackable>();

            StartCoroutine(FlickerOn());
        }


        private IEnumerator FlickerOn()
        {
            float initialInterval = 1.0f;

            yield return new WaitForSeconds(InitialWait);

            while (initialInterval > 0)
            {
                var subtr = Random.Range(0.1f, 0.3f);

                yield return new WaitForSeconds(initialInterval / 4);
                _hackable.IsHacked = !_hackable.IsHacked;
                yield return new WaitForSeconds(initialInterval / 4);
                _hackable.IsHacked = !_hackable.IsHacked;

                yield return new WaitForSeconds(initialInterval);
                initialInterval -= subtr;
            }

            _hackable.IsHacked = true;
        }
    }
}
