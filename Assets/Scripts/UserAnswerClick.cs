using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UserAnswerClick : MonoBehaviour
{
    [SerializeField] private TMP_Text ButtonText;

    private int IdAnswer;

    public void WriteAnswer(Answer answer)
    {
        ButtonText.text = answer.Text;
        IdAnswer = answer.Id;
    }

    public async void SendAnswer()
    {
        var a = GameObject.Find("BackGround").GetComponent<GameController>();
        await a.CheckUserAnswer(IdAnswer);
    }
}
