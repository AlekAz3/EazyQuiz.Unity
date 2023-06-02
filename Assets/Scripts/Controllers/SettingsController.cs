using EazyQuiz.Unity.Services;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class SettingsController : MonoBehaviour
    {
        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        [Inject] private readonly SaveUserService _saveUser;

        public void ExitAccount()
        {
            _saveUser.DeleteUser();
            _scene.ShowAuthScene();
        }

        public void Exit()
        {
            _scene.ShowMainMenuScene();
        }
    }
}