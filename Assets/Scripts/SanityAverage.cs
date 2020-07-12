using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    class SanityAverage : MonoBehaviour
    {
        public float AverageSanity;


        private void Update()
        {
            var all = FindObjectsOfType<Sanity>();
            var n = all.Count();
            var sum = 0.0f;

            foreach (var obj in all)
                sum += obj.GetComponent<Sanity>().SanityPoints;

            AverageSanity = sum / n;
        }
    }
}
