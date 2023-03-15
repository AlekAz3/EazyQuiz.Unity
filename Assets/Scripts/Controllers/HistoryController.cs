using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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

    private async void Awake()
    {
        await AddHistoryCard();
        scrollbar.value = 0;
    }

    public async Task AddHistoryCard()
    {
        var a = await apiProvider.GetHistory(
            user.UserInfo.Id,
            new AnswersGetHistoryCommand() { PageNumber = page, PageSize = 10 },
            user.UserInfo.Token
            );
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
}
