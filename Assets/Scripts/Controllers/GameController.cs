using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Button> Buttons;
    [SerializeField] private GameObject gameovers;
    [SerializeField] private TMP_Text QuestiolLabel;

    private QuestionWithAnswers question;

    [Inject] private UserService _userService;
    [Inject] private ApiProvider _apiProvider;
    private GameOverScreen _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen = gameovers.GetComponent<GameOverScreen>();
        question = Task.Run(() => { return _apiProvider.GetQuestion(_userService.UserInfo.Token); }).Result;
        QuestiolLabel.text = question.Text;

        var answers = question.Answers
            .ToList()
            .Shuffle();

        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponent<UserAnswerClick>().WriteAnswer(answers[i]);
        }
    }

    public async Task CheckUserAnswer(Answer answer)
    {
        if (answer.IsCorrect)
        {
            _gameOverScreen.Show(true);
            Debug.Log("Correct");
        }
        else
        {
            _gameOverScreen.Show(false);
            Debug.Log("Wrong");
        }

        await _userService.SendUserAnswer(answer, question.QuestionId);
    }

    public void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
