using EazyQuiz.Models.DTO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetUserAnswer : MonoBehaviour
{
    [SerializeField] private TMP_Text Answer;
    [SerializeField] private TMP_Text Question;

    public void ItemView(UserAnswerHistory history)
    {
        Answer.text = history.AnswerText;
        Question.text = history.QuestionText;
    }
}
