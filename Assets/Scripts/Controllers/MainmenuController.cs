using EazyQuiz.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainmenuController : MonoBehaviour
{
    [Inject] private readonly UserService _userService;
    [SerializeField] private LoadingScreen _loadingScreen;

    [SerializeField] private TMP_Text UsernameLabel;
    [SerializeField] private TMP_Text PointsLabel;

    private void Awake()
    {
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
