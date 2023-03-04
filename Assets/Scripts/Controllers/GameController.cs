using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;

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

    private QuestionResponse question;

    private int IdCorrectAnswer;

    [Inject] private UserService _userService;
    [Inject] private ApiProvider _apiProvider;
    private GameOverScreen _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen = gameovers.GetComponent<GameOverScreen>();
        question = Task.Run(() => { return _apiProvider.GetQuestion(_userService.UserInfo.Token); }).Result;
        QuestiolLabel.text = question.TextQuestion;
        IdCorrectAnswer = question.IdCorrectAnswer;
        var answers = new List<Answer>() { new Answer(question.IdCorrectAnswer, question.TextCorrectAnswer), new Answer(question.IdFirstAnswer, question.TextFirstAnswer), new Answer(question.IdSecondAnswer, question.TextSecondAnswer), new Answer(question.IdThirdAnswer, question.TextThirdAnswer) };
        answers.Shuffle();
        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponent<UserAnswerClick>().WriteAnswer(answers[i]);
        }
    }

    public async Task CheckUserAnswer(int userAnswer)
    {
        if (userAnswer == IdCorrectAnswer)
        {
            _gameOverScreen.Show(true);
            _userService.AddPoint();
            Debug.Log("Correct");
        }
        else
        {
            _gameOverScreen.Show(false);
            Debug.Log("Wrong");
        }

        await _userService.SendUserAnswer(userAnswer, question.IdQuestion);
    }

    public void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
