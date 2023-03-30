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

        /// <summary>
        /// Сервис пользователя
        /// </summary>
        [Inject] private readonly UserService user;

        /// <summary>
        /// Сервис общения с сервером
        /// </summary>
        [Inject] private readonly ApiProvider apiProvider;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private SwitchSceneService _scene;

        public RectTransform content;

        /// <summary>
        /// Текущая "Страница"
        /// </summary>
        private int page = 0;

        /// <summary>
        /// Всего элементов
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Флаг
        /// </summary>
        private bool flag = true;

        private async void Awake()
        {
            await AddHistoryCard();
        }

        /// <summary>
        /// Добавить карточку ответа на вопрос
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
        /// Сгенерировать карточки ответа на вопрос
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
        /// Проверка значение скроллбара для автоподгрузки истории
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
        /// Переводит на следующую страницу
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
        /// Выход в главное меню
        /// </summary>
        public void ExitToMenu()
        {
            _scene.ShowMainMenuScene();
        }
    }
}