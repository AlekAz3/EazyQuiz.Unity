using UnityEngine.SceneManagement;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// Сервис переключения сцен
    /// </summary>
    public class SwitchSceneService
    {
        /// <summary>
        /// Переключить на сцену главного меню
        /// </summary>
        public void ShowMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Переключить на сцену истории ответов 
        /// </summary>
        public void ShowHistoryScene()
        {
            SceneManager.LoadScene("HistoryScene");
        }

        /// <summary>
        /// Переключить на сцену ответа на вопросы 
        /// </summary>
        public void ShowGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void ShowLeaderboardScene()
        {
            SceneManager.LoadScene("Leaderboard");
        }

        /// <summary>
        /// Переключить на сцену предложения своего вопроса 
        /// </summary>
        public void ShowAddUserQuestionScene()
        {
            SceneManager.LoadScene("AddQuestionScene");
        }

        public void ShowAuthScene()
        {
            SceneManager.LoadScene("AuthScene");
        }

        public void ShowSettingScene()
        {
            SceneManager.LoadScene("SettingsScene");
        }

        public void ShowFeedbackScene()
        {
            SceneManager.LoadScene("FeedbackScene");
        }
    }
}
