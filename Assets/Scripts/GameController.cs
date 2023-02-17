using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Button> Buttons;
    [SerializeField] private TMP_Text QuestiolLabel;

    private bool RightAnswer;

    private UserResponse User;
    private ApiProvider _apiProvider;

    private async void Awake()
    {
        User = GameObject.Find("User").GetComponent<UserController>().User;
        var question = await _apiProvider.GetQuestion(User.Token);

        QuestiolLabel.text = question.TextQuestion
    }

    private void AnswerButtonClick()
    {

    }

}
