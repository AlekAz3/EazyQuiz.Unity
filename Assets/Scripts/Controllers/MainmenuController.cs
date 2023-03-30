using EazyQuiz.Unity;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainmenuController : MonoBehaviour
{
    [Inject] private readonly UserService _userService;
    [SerializeField] private LoadingScreen _loadingScreen;

    [SerializeField] private TMP_Text UsernameLabel;
    [SerializeField] private TMP_Text PointsLabel;

    /// <inheritdoc cref="SwitchSceneService"/>
    [Inject] private SwitchSceneService _scene;

    private void Awake()
    {
        UsernameLabel.text = _userService.UserInfo.UserName;
        PointsLabel.text = $"Очки: {_userService.UserInfo.Points}";
    }

    public void StartGameButtonClick()
    {
        _loadingScreen.Show();
        _scene.ShowGameScene();
    }

    public void ViewHistoryButtonClick()
    {
        _loadingScreen.Show();
        _scene.ShowHistoryScene();
    }
}
