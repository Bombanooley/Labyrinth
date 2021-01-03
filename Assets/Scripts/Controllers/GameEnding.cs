using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Labyrinth
{
    public class GameEnding : MonoBehaviour
    {
        public GameObject player;

        public float fadeDuration = 1f;
        public float displayImageDuration = 1f;
        public float _timer;

        public void RestartButton()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        public void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)
        {
            imageCanvasGroup.gameObject.SetActive(true);
            _timer += Time.deltaTime;
            imageCanvasGroup.alpha = _timer / fadeDuration;

            if (_timer > fadeDuration + displayImageDuration)
            {
                if (doRestart)
                    Time.timeScale = 1;
                else
                {
                    if (SceneManager.GetActiveScene().buildIndex == 1) SceneManager.LoadScene("SampleScene");
                    else
                        ExitGame();
                }
            }
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
