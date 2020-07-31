using UnityEngine;

namespace Assets.Scripts
{
    class Spawner : MonoBehaviour
    {
        public GameObject Prefab = null;


        public void DoSpawn()
        {
            var go = Instantiate(Prefab);
            var t = go.GetComponent<Transform>();
            t.position = transform.position;
        }
    }
}
