using EazyQuiz.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]private GameObject screen;

    [SerializeField] private TMP_Text text;

    public void Show(bool isTrueAnswer)
    {
        screen.SetActive(true);
        if (isTrueAnswer)
        {
            text.text = "Ответ верный";
        }
        else
        {
            text.text = "Ответ не верный верный";
        }
    }
}
