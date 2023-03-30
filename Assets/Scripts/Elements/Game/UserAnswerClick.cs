using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

/// <summary>
/// ������� ������ �� ������
/// </summary>
public class UserAnswerClick : MonoBehaviour
{
    /// <summary>
    /// ����� ������ � ������
    /// </summary>
    [SerializeField] private TMP_Text ButtonText;

    /// <summary>
    /// GameController
    /// </summary>
    [SerializeField] private GameController gameController;

    /// <summary>
    /// ����� ����������� �� ������
    /// </summary>
    private Answer _answer;

    /// <summary>
    /// �������� ����� �� ������
    /// </summary>
    /// <param name="answer">����� � <see cref="Answer"/></param>
    public void WriteAnswer(Answer answer)
    {
        _answer = answer;
        ButtonText.text = answer.AnswerText;
    }

    /// <summary>
    /// �������� ����� 
    /// </summary>
    public async void SendAnswer()
    {
        await gameController.CheckUserAnswer(_answer);
    }
}
