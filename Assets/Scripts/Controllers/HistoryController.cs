using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.History;
using EazyQuiz.Unity.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// ���������� ������ ��������� ������� �������
    /// </summary>
    public class HistoryController : MonoBehaviour
    {
        /// <summary>
        /// ������ �������� ������
        /// </summary>
        [SerializeField] public GameObject prefab;

        /// <summary>
        /// ���������
        /// </summary>
        [SerializeField] public Scrollbar scrollbar;

        /// <summary>
        /// ������ ������������
        /// </summary>
        [Inject] private readonly UserService user;

        /// <summary>
        /// ������ ������� � ��������
        /// </summary>
        [Inject] private readonly ApiProvider apiProvider;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private SwitchSceneService _scene;

        public RectTransform content;

        /// <summary>
        /// ������� "��������"
        /// </summary>
        private int page = 0;

        /// <summary>
        /// ����� ���������
        /// </summary>
        private int count = 0;

        /// <summary>
        /// ����
        /// </summary>
        private bool flag = true;

        private async void Awake()
        {
            await AddHistoryCard();
        }

        /// <summary>
        /// �������� �������� ������ �� ������
        /// </summary>
        private async Task AddHistoryCard()
        {
            var historyAnswers = await apiProvider.GetHistory(
                user.UserInfo.Id,
                new AnswersGetHistoryCommand() { PageNumber = page, PageSize = 10 },
                user.UserInfo.Token
                );
            Debug.Log(historyAnswers.Count);
            count = (int)historyAnswers.Count;
            GenerateGameObjects(historyAnswers.Items);
        }

        /// <summary>
        /// ������������� �������� ������ �� ������
        /// </summary>
        /// <param name="answerHistory"></param>
        private void GenerateGameObjects(IEnumerable<UserAnswerHistory> answerHistory)
        {
            foreach (var item in answerHistory)
            {
                var instants = Instantiate(prefab);
                instants.transform.SetParent(content, false);
                instants.GetComponent<SetUserAnswer>().ItemView(item);
            }
        }

        /// <summary>
        /// �������� �������� ���������� ��� ������������� �������
        /// </summary>
        /// <param name="vector"></param>
        public async void ValueCheck(Vector2 vector)
        {
            if (vector.y > 0.2)
            {
                flag = true;
            }

            if (vector.y < 0.2 && flag)
            {
                if (AddPage())
                {
                    flag = false;
                    await AddHistoryCard();
                    Debug.Log("AddPage");

                }
            }
        }

        /// <summary>
        /// ��������� �� ��������� ��������
        /// </summary>
        /// <returns></returns>
        private bool AddPage()
        {
            if (Math.Ceiling(count / 10d) > page)
            {
                page++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// ����� � ������� ����
        /// </summary>
        public void ExitToMenu()
        {
            _scene.ShowMainMenuScene();
        }
    }
}