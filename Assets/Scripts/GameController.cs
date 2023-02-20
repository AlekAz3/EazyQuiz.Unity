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

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Button> Buttons;
    [SerializeField] private GameObject GameOver;

    [SerializeField] private TMP_Text QuestiolLabel;

    private QuestionResponse question;

    private int IdCorrectAnswer;
    private UserResponse User;
    private ApiProvider _apiProvider;

    private void Awake()
    {
        _apiProvider = new ApiProvider();
        User = GameObject.Find("User").GetComponent<UserController>().User;
        question = Task.Run(() => { return _apiProvider.GetQuestion(User.Token); }).Result;
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
        GameOver.SetActive(true);
        if (userAnswer == IdCorrectAnswer)
        {
            Debug.Log("Correct");
        }
        Debug.Log("Wrong");

        var ua = new UserAnswer() { IdUser = User.Id, IdAnswer = userAnswer, IdQuestion = question.IdQuestion };
        await _apiProvider.SendUserAnswer(ua, User.Token);
    }

    public void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
