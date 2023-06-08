using EazyQuiz.Unity.Services;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер для настроек
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="SaveUserService"/>
        [Inject] private readonly SaveUserService _saveUser;
    
        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        public void ExitAccount()
        {
            _saveUser.DeleteUser();
            _scene.ShowAuthScene();
        }
        
        /// <summary>
        /// Выйти в главное меню
        /// </summary>
        public void Exit()
        {
            _scene.ShowMainMenuScene();
        }
    }
}