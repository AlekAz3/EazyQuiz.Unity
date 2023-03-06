using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections.Generic;
using System.Linq;
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


    /// <summary>
    /// ��� ��������
    /// </summary>
    private List<QuestionWithAnswers> questions = new List<QuestionWithAnswers>();

    /// <summary>
    /// ������� ������� 
    /// </summary>
    private int order = 0;

    [Inject] private UserService _userService;
    [Inject] private ApiProvider _apiProvider;
    private GameOverScreen _gameOverScreen;

    private async void Awake()
    {
        _gameOverScreen = gameovers.GetComponent<GameOverScreen>();
        await NewQuestion();
    }

    /// <summary>
    /// ��������� ������
    /// </summary>
    private async Task NewQuestion()
    {
        if (questions.Count() - order < 5)
        {
            await GetQuestions();
        }
        SetQuestion();
    }

    /// <summary>
    /// ���������� �������� � ������� 
    /// </summary>
    public async Task GetQuestions()
    {
        if (order > 25)
        {
            order = 0;
            questions.Clear();
        }
        var ques = await _apiProvider.GetQuestions(_userService.UserInfo.Token);

        questions.AddRange(ques);
    }

    /// <summary>
    /// ������ ������ �������� � ������� � ���������
    /// </summary>
    public void SetQuestion()
    {
        QuestiolLabel.text = questions[order].Text;
        var answers = questions[order].Answers
            .ToList()
            .Shuffle();

        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponent<UserAnswerClick>().WriteAnswer(answers[i]);
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// </summary>
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

        await _userService.SendUserAnswer(answer, questions[order].QuestionId);
    }

    /// <summary>
    /// ��������� ������
    /// </summary>
    public async void NextQuestion()
    {
        order++;
        await NewQuestion();
        _gameOverScreen.Hide();
    }

    /// <summary>
    /// ����� � ������� ����
    /// </summary>
    public void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
