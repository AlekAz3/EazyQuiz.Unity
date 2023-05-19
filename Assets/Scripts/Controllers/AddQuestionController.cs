using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.UserQuestion;
using EazyQuiz.Unity.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class AddQuestionController : MonoBehaviour
    {
        [SerializeField] public GameObject prefab;

        [SerializeField] private TMP_InputField QuestionText;

        [SerializeField] private TMP_InputField AnswerText;

        [SerializeField] private InformationScreen InfoScreen;

        /// <summary>
        /// ������ ������� � ��������
        /// </summary>
        [Inject] private readonly ApiProvider _apiProvider;

        [Inject] private readonly UserService user;

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
            await AddHistoryQuestion(); 
        }

        /// <summary>
        /// �������� �������� ������ �� ������
        /// </summary>
        private async Task AddHistoryQuestion()
        {
            var historyQuestion = await _apiProvider.GetCurrentUserQuestions(
                new GetHistoryCommand() { PageNumber = page, PageSize = 10 },
                user.UserInfo.Token
                );
            Debug.Log(historyQuestion.Count);
            count = (int)historyQuestion.Count;
            GenerateGameObjects(historyQuestion.Items);
        }

        private void GenerateGameObjects(IEnumerable<QuestionByUserResponse> questionHistory)
        {
            foreach (var item in questionHistory)
            {
                var instants = Instantiate(prefab);
                instants.transform.SetParent(content, false);
                instants.GetComponent<SetUserQuestion>().ItemView(item);
            }
        }

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
                    await AddHistoryQuestion();
                    Debug.Log("AddPage");
                }
            }
        }

        /// <summary>
        /// ��������� �� ��������� ��������
        /// </summary>
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
        /// ���������� ������ �� ������
        /// </summary>
        public async void SendUserQuestion()
        {
            var questionText = QuestionText.text;
            var answerText = AnswerText.text;

            if (questionText.IsNullOrEmpty() || answerText.IsNullOrEmpty())
            {
                InfoScreen.ShowError("���� ������ ����");
                return;
            }
            InfoScreen.ShowInformation("��� ������������ ������ ���������");
            var question = new AddQuestionByUser()
            {
                QuestionText = questionText,
                AnswerText = answerText,
            };

            await _apiProvider.SendUserQuestion(question, user.UserInfo.Token);
            QuestionText.text = string.Empty;
            AnswerText.text = string.Empty;
            await Refresh();
        }

        private async Task Refresh()
        {
            page = 0;
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            await AddHistoryQuestion();
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