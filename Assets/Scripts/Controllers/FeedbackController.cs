using EazyQuiz.Unity.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class FeedbackController : MonoBehaviour
    {
        [Inject] private readonly SwitchSceneService _scene;

        public void Exit()
        {
            _scene.ShowMainMenuScene();
        }
    }
}
