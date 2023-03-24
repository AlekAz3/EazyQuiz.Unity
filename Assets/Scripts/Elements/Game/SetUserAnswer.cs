using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetUserAnswer : MonoBehaviour
{
    [SerializeField] private TMP_Text Answer;
    [SerializeField] private TMP_Text Question;
    [SerializeField] private TMP_Text Date;
    [SerializeField] private Image Background;

    public void ItemView(UserAnswerHistory history)
    {
        Answer.text = history.AnswerText;
        Question.text = history.QuestionText;
        Date.text = history.AnswerTime.ToString("dd.MM.yyyy");
        if (history.IsCorrect)
        {
            Background.color = Color.green;
        }
        else
        {
            Background.color = Color.red;
        }
    }
}
