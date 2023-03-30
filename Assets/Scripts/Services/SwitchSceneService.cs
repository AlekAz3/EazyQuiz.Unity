using UnityEngine.SceneManagement;

namespace EazyQuiz.Unity
{
    public class SwitchSceneService
    {
        public void ShowMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowHistoryScene()
        {
            SceneManager.LoadScene("HistoryScene");
        }

        public void ShowGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
