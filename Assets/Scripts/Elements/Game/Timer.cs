using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameOverScreen _gameOverScreen;

    /// <summary>
    /// ����� �� ����� �� ������
    /// </summary>
    private float time;

    /// <summary>
    /// ��������� ������
    /// </summary>
    /// <param name="cooldownTime">����� �������</param>
    public void StartTimer(float cooldownTime)
    {
        Debug.Log("Start Timer");
        slider.maxValue = cooldownTime;
        time = cooldownTime;
        StartCoroutine(TimerCoroutine());
    }

    /// <summary>
    /// �������� �������� �������
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
        _gameOverScreen.Show("����� �����");
    }

    /// <summary>
    /// ��������� �������
    /// </summary>
    public void StopTimer()
    {
        StopAllCoroutines();
    }
}
