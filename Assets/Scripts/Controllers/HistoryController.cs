using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class HistoryController : MonoBehaviour
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public Scrollbar scrollbar;
    [Inject] private readonly UserService user;
    [Inject] private readonly ApiProvider apiProvider;

    public RectTransform content;

    private int page = 0;
    private int count = 0;
    private bool flag = true;

    private async void Awake()
    {
        await AddHistoryCard();
    }

    public async Task AddHistoryCard()
    {
        var a = await apiProvider.GetHistory(
            user.UserInfo.Id,
            new AnswersGetHistoryCommand() { PageNumber = page, PageSize = 10 },
            user.UserInfo.Token
            );
        Debug.Log(a.Count);
        count = (int)a.Count;
         GenerateGameObjects(a.Items);
    }

    private void GenerateGameObjects(IEnumerable<UserAnswerHistory> answerHistory)
    {
        foreach (var item in answerHistory)
        {
            var instants = Instantiate(prefab);
            instants.transform.SetParent(content, false);
            instants.GetComponent<SetUserAnswer>().ItemView(item);
        }
    }

    public async void ValueCheck(Vector2 vector)
    {
        if (vector.y > 0.2)
        {
            flag = true;
        }

        if (vector.y<0.2 && flag)
        {
            if (AddPage())
            {
                flag = false;
                await AddHistoryCard();
                Debug.Log("AddPage");

            }
        }
    }

    public bool AddPage()
    {
        if (Math.Ceiling(count / 10d) > page)
        {
            page++;
            return true;
        }
        return false;
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
