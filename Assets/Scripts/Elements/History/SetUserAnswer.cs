using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Запись ответа пользователя в краточку
/// </summary>
public class SetUserAnswer : MonoBehaviour
{
    /// <summary>
    /// Текст ответа
    /// </summary>
    [SerializeField] private TMP_Text Answer;

    /// <summary>
    /// Текст вопроса
    /// </summary>
    [SerializeField] private TMP_Text Question;

    /// <summary>
    /// Дата ответа
    /// </summary>
    [SerializeField] private TMP_Text Date;

    /// <summary>
    /// фон
    /// </summary>
    [SerializeField] private Image Background;

    /// <summary>
    /// Запись ответа в карточку
    /// </summary>
    /// <param name="history">Исторический ответ пользователя в <see cref="UserAnswerHistory"/></param>
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
