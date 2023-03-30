using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameOverScreen _gameOverScreen;

    /// <summary>
    /// Время на ответ на вопрос
    /// </summary>
    private float time;

    /// <summary>
    /// Запустить таймер
    /// </summary>
    /// <param name="cooldownTime">Время таймера</param>
    public void StartTimer(float cooldownTime)
    {
        Debug.Log("Start Timer");
        slider.maxValue = cooldownTime;
        time = cooldownTime;
        StartCoroutine(TimerCoroutine());
    }

    /// <summary>
    /// Карутина ожидания таймера
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimerCoroutine()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            slider.value = time;
            yield return null;
        }
        _gameOverScreen.Show("Время вышло");
    }

    /// <summary>
    /// Остановка таймера
    /// </summary>
    public void StopTimer()
    {
        StopAllCoroutines();
    }
}
