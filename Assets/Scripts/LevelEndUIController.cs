using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class LevelEndUIController : MonoBehaviour
    {
        public string NextLevel = null;
        public Button NextLevelButton = null;

        private void Start()
        {
            NextLevelButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(NextLevel);
            });
        }
    }
}
