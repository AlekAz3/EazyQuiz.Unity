using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.UserQuestion;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер для добавления вопросов в викторину 
    /// </summary>
    public class AddQuestionController : MonoBehaviour
    {
        /// <summary>
        /// Префаб карточки вопроса
        /// </summary>
        [SerializeField] public GameObject prefab;

        /// <summary>
        /// Тест предложенного вопроса 
        /// </summary>
        [SerializeField] private TMP_InputField questionText;
        
        /// <summary>
        /// Текст ответа на предложенный вопрос 
        /// </summary>
        [SerializeField] private TMP_InputField answerText;

        /// <summary>
        /// Всплывающее окно информации
        /// </summary>
        [SerializeField] private InformationScreen infoScreen;

        /// <summary>
        /// Сервис общения с сервером
        /// </summary>
        [Inject] private readonly ApiProvider _apiProvider;
        
        /// <summary>
        /// Сервис работы с пользователем
        /// </summary>
        [Inject] private readonly UserService _user;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        public RectTransform content;

        /// <summary>
        /// Текущая "Страница"
        /// </summary>
        private int _page = 0;

        /// <summary>
        /// Всего элементов
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// Флаг
        /// </summary>
        private bool _flag = true;

        private async void Awake()
        {
            await AddHistoryQuestion(); 
        }

        /// <summary>
        /// Добавить карточку ответа на вопрос
        /// </summary>
        private async Task AddHistoryQuestion()
        {
            if (_user.UserInfo.Token != null)
            {
                var historyQuestion = await _apiProvider.GetCurrentUserQuestions(
                    new GetHistoryCommand() { PageNumber = _page, PageSize = 10 },
                    _user.UserInfo.Token.Jwt
                );
                Debug.Log(historyQuestion.Count);
                _count = (int)historyQuestion.Count;
                GenerateGameObjects(historyQuestion.Items);
            }
        }
        
        /// <summary>
        /// Создать карточки 
        /// </summary>
        private void GenerateGameObjects(IEnumerable<QuestionByUserResponse> questionHistory)
        {
            foreach (var item in questionHistory)
            {
                var instants = Instantiate(prefab, content, false);
                instants.GetComponent<SetUserQuestion>().ItemView(item);
            }
        }
        
        /// <summary>
        /// Скроллбар
        /// </summary>
        public async void ValueCheck(Vector2 vector)
        {
            if (vector.y > 0.005)
            {
                _flag = true;
            }

            if (!(vector.y < 0.005) || !_flag) return;

            if (!AddPage()) return;
            
            _flag = false;
            await AddHistoryQuestion();
        }

        /// <summary>
        /// Переводит на следующую страницу
        /// </summary>
        private bool AddPage()
        {
            if (!(Math.Ceiling(_count / 10d) > _page)) return false;
            
            _page++;
            
            return true;
        }

        /// <summary>
        /// Отправляет вопрос на сервер
        /// </summary>
        public async void SendUserQuestion()
        {
            var questionText = this.questionText.text;
            var answerText = this.answerText.text;

            if (questionText.IsNullOrEmpty() || answerText.IsNullOrEmpty())
            {
                infoScreen.ShowError("Есть пустые поля");
                return;
            }
            infoScreen.ShowInformation("Ваш предложенный вопрос отправлен");
            var question = new AddQuestionByUser()
            {
                QuestionText = questionText,
                AnswerText = answerText,
            };

            await _apiProvider.SendUserQuestion(question, _user.UserInfo.Token.Jwt);
            this.questionText.text = string.Empty;
            this.answerText.text = string.Empty;
            await Refresh();
        }

        /// <summary>
        /// Обновление карточек
        /// </summary>
        private async Task Refresh()
        {
            _page = 0;
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            await AddHistoryQuestion();
        }


        /// <summary>
        /// Выход в главное меню
        /// </summary>
        public void ExitToMenu()
        {
            _scene.ShowMainMenuScene();
        }
    }
}