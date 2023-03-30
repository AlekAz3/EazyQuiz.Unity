using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

/// <summary>
/// Элемент ответа на вопрос
/// </summary>
public class UserAnswerClick : MonoBehaviour
{
    /// <summary>
    /// Текст ответа в кнопке
    /// </summary>
    [SerializeField] private TMP_Text ButtonText;

    /// <summary>
    /// GameController
    /// </summary>
    [SerializeField] private GameController gameController;

    /// <summary>
    /// Ответ назначенный на кнопку
    /// </summary>
    private Answer _answer;

    /// <summary>
    /// Записать ответ на кнопку
    /// </summary>
    /// <param name="answer">Ответ в <see cref="Answer"/></param>
    public void WriteAnswer(Answer answer)
    {
        _answer = answer;
        ButtonText.text = answer.AnswerText;
    }

    /// <summary>
    /// Записать ответ 
    /// </summary>
    public async void SendAnswer()
    {
        await gameController.CheckUserAnswer(_answer);
    }
}
