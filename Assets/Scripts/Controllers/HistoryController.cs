using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.History;
using EazyQuiz.Unity.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер панели просмотра истории ответов
    /// </summary>
    public class HistoryController : MonoBehaviour
    {
        /// <summary>
        /// Префаб карточки ответа
        /// </summary>
        [SerializeField] public GameObject prefab;

        /// <summary>
        /// Скроллбар
        /// </summary>
        [SerializeField] public Scrollbar scrollbar;

        [SerializeField] private LoadingScreen loadingScreen;

        /// <summary>
        /// Сервис пользователя
        /// </summary>
        [Inject] private readonly UserService _user;

        /// <summary>
        /// Сервис общения с сервером
        /// </summary>
        [Inject] private readonly ApiProvider _apiProvider;

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
            loadingScreen.Show();
            await AddHistoryCard();
            loadingScreen.Hide();
        }

        /// <summary>
        /// Добавить карточку ответа на вопрос
        /// </summary>
        private async Task AddHistoryCard()
        {
            var historyAnswers = await _apiProvider.GetHistory(
                new GetHistoryCommand() { PageNumber = _page, PageSize = 10 },
                _user.UserInfo.Token.Jwt
                );
            Debug.Log(historyAnswers.Count);
            _count = (int)historyAnswers.Count;
            GenerateGameObjects(historyAnswers.Items);
        }

        /// <summary>
        /// Сгенерировать карточки ответа на вопрос
        /// </summary>
        /// <param name="answerHistory"></param>
        private void GenerateGameObjects(IEnumerable<UserAnswerHistory> answerHistory)
        {
            foreach (var item in answerHistory)
            {
                var instants = Instantiate(prefab, content, false);
                instants.GetComponent<SetUserAnswer>().ItemView(item);
            }
        }

        /// <summary>
        /// Проверка значение скроллбара для автоподгрузки истории
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
            await AddHistoryCard();
            Debug.Log("AddPage");
        }

        /// <summary>
        /// Переводит на следующую страницу
        /// </summary>
        /// <returns></returns>
        private bool AddPage()
        {
            if (!(Math.Ceiling(_count / 10d) > _page)) return false;
            
            _page++;
            
            return true;
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