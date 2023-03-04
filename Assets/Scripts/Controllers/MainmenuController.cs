using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainmenuController : MonoBehaviour
{
    [Inject] private readonly UserService _userService;

    [SerializeField] private TMP_Text UsernameLabel;
    [SerializeField] private TMP_Text PointsLabel;

    private void Awake()
    {
        UsernameLabel.text = _userService.UserInfo.UserName;
        PointsLabel.text = $"Очки: {_userService.UserInfo.Points}";
    }

    public void StartGameButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
