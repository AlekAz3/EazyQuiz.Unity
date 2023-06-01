using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
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

        [SerializeField] private LoadingScreen loadingScreen;

        /// <summary>
        /// ������ ������������
        /// </summary>
        [Inject] private readonly UserService user;

        /// <summary>
        /// ������ ������� � ��������
        /// </summary>
        [Inject] private readonly ApiProvider apiProvider;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

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
            loadingScreen.Show();
            await AddHistoryCard();
            loadingScreen.Hide();
        }

        /// <summary>
        /// �������� �������� ������ �� ������
        /// </summary>
        private async Task AddHistoryCard()
        {
            var historyAnswers = await apiProvider.GetHistory(
                new GetHistoryCommand() { PageNumber = page, PageSize = 10 },
                user.UserInfo.Token.Jwt
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
        public async void ValueCheck(Vector2 vector)
        {
            if (vector.y > 0.005)
            {
                flag = true;
            }

            if (vector.y < 0.005 && flag)
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