using System.Collections;
using EazyQuiz.Unity.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// Элемент Таймера
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Таймер
        /// </summary>
        [SerializeField] private Slider slider;

        /// <summary>
        /// Окно завершения игры
        /// </summary>
        [SerializeField] private GameOverScreen gameOverScreen;
        
        /// <summary>
        /// Контроллер
        /// </summary>
        [SerializeField] private GameController gameController;

        /// <summary>
        /// Время на ответ на вопрос
        /// </summary>
        private float _time;

        /// <summary>
        /// Запустить таймер
        /// </summary>
        /// <param name="cooldownTime">Время таймера</param>
        public void StartTimer(float cooldownTime)
        {
            Debug.Log("Start Timer");
            slider.maxValue = cooldownTime;
            _time = cooldownTime;
            StartCoroutine(TimerCoroutine());
        }

        /// <summary>
        /// Карутина ожидания таймера
        /// </summary>
        private IEnumerator TimerCoroutine()
        {
            while (_time > 0)
            {
                _time -= Time.deltaTime;
                slider.value = _time;
                yield return null;
            }

            gameController.TimeOver();
        }

        /// <summary>
        /// Остановка таймера
        /// </summary>
        public void StopTimer()
        {
            StopAllCoroutines();
        }
    }
}