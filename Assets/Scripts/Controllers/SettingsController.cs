using EazyQuiz.Unity.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class SettingsController : MonoBehaviour
    {
        [Inject] private readonly SwitchSceneService _scene;

        public void ExitAccount()
        {
            _scene.ShowAuthScene();
        }

        public void Exit()
        {
            _scene.ShowMainMenuScene();
        }
    }
}