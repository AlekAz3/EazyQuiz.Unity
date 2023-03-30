using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ������ ������������ � ��������
/// </summary>
public class SetUserAnswer : MonoBehaviour
{
    /// <summary>
    /// ����� ������
    /// </summary>
    [SerializeField] private TMP_Text Answer;

    /// <summary>
    /// ����� �������
    /// </summary>
    [SerializeField] private TMP_Text Question;

    /// <summary>
    /// ���� ������
    /// </summary>
    [SerializeField] private TMP_Text Date;

    /// <summary>
    /// ���
    /// </summary>
    [SerializeField] private Image Background;

    /// <summary>
    /// ������ ������ � ��������
    /// </summary>
    /// <param name="history">������������ ����� ������������ � <see cref="UserAnswerHistory"/></param>
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
