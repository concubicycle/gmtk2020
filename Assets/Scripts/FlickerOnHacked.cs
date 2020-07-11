using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    class FlickerOnHacked : MonoBehaviour
    {
        private Terminal _terminal;


        private void Start()
        {
            _terminal = GetComponent<Terminal>();

            StartCoroutine(FlickerOn());
        }


        private IEnumerator FlickerOn()
        {
            float initialInterval = 1.0f;

            while (initialInterval > 0)
            {
                var subtr = Random.Range(0.1f, 0.3f);

                yield return new WaitForSeconds(initialInterval / 4);
                _terminal.IsHacked = !_terminal.IsHacked;
                yield return new WaitForSeconds(initialInterval / 4);
                _terminal.IsHacked = !_terminal.IsHacked;

                yield return new WaitForSeconds(initialInterval);
                initialInterval -= subtr;
            }

            _terminal.IsHacked = true;
        }
    }
}
