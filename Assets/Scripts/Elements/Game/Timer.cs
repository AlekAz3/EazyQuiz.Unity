using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// ������� �������
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private Slider slider;

        /// <summary>
        /// ���� ���������� ����
        /// </summary>
        [SerializeField] private GameOverScreen gameOverScreen;

        /// <summary>
        /// ����� �� ����� �� ������
        /// </summary>
        private float _time;

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <param name="cooldownTime">����� �������</param>
        public void StartTimer(float cooldownTime)
        {
            Debug.Log("Start Timer");
            slider.maxValue = cooldownTime;
            _time = cooldownTime;
            StartCoroutine(TimerCoroutine());
        }

        /// <summary>
        /// �������� �������� �������
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimerCoroutine()
        {
            while (_time > 0)
            {
                _time -= Time.deltaTime;
                slider.value = _time;
                yield return null;
            }
            gameOverScreen.Show("����� �����");
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public void StopTimer()
        {
            StopAllCoroutines();
        }
    }
}