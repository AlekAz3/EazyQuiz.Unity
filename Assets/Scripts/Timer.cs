using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject gameovers;
    private float time;

    private GameOverScreen _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen = gameovers.GetComponent<GameOverScreen>();
    }

    public void StartTimer(float cooldownTime)
    {
        Debug.Log("Start Timer");
        slider.maxValue = cooldownTime;
        time = cooldownTime;
        StartCoroutine(timer());
    }


    IEnumerator timer()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            slider.value = time;
            yield return null;
        }
        _gameOverScreen.Show("����� �����");
    }
}