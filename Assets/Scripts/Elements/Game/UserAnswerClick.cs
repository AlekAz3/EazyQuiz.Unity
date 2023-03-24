using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

public class UserAnswerClick : MonoBehaviour
{
    [SerializeField] private TMP_Text ButtonText;

    private Answer _answer;

    public void WriteAnswer(Answer answer)
    {
        _answer = answer;
        ButtonText.text = answer.AnswerText;
    }

    public async void SendAnswer()
    {
        var a = GameObject.Find("BackGround").GetComponent<GameController>();
        await a.CheckUserAnswer(_answer);
    }
}
