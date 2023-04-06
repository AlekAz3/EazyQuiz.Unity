using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Auth;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.History;
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

        [SerializeField] private ErrorScreen ErrorScreen;

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
            var historyAnswers = await _apiProvider.GetCurrentUserQuestions(
                user.UserInfo.Id,
                new GetHistoryCommand() { PageNumber = page, PageSize = 10 },
                user.UserInfo.Token
                );
            Debug.Log(historyAnswers.Count);
            count = (int)historyAnswers.Count;
            GenerateGameObjects(historyAnswers.Items);
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
        /// ���������� ������ �� ������
        /// </summary>
        public async void SendUserQuestion()
        {
            var question = QuestionText.text;
            var answer = AnswerText.text;

            if (question.IsNullOrEmpty() || answer.IsNullOrEmpty())
            {
                ErrorScreen.Activate("���� ������ ����");
                return;
            }
            var q = new AddQuestionByUser()
            {
                UserId = user.UserInfo.Id,
                QuestionText = question,
                AnswerText = answer,
            };

            await _apiProvider.SendUserQuestion(q, user.UserInfo.Token);
            QuestionText.text = string.Empty;
            AnswerText.text = string.Empty;
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