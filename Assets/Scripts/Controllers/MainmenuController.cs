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
    private  LoadingScreen _loadingScreen;

    [SerializeField] private TMP_Text UsernameLabel;
    [SerializeField] private TMP_Text PointsLabel;
    [SerializeField] private GameObject LoadingGO;

    private void Awake()
    {
        _loadingScreen = LoadingGO.GetComponent<LoadingScreen>();
        UsernameLabel.text = _userService.UserInfo.UserName;
        PointsLabel.text = $"Очки: {_userService.UserInfo.Points}";
    }

    public void StartGameButtonClick()
    {
        _loadingScreen.Show();
        SceneManager.LoadScene("GameScene");
    }

    public void ViewHistoryButtonClick()
    {
        _loadingScreen.Show();
        SceneManager.LoadScene("HistoryScene");
    }
}
