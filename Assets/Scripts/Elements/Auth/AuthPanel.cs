using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Auth
{
    public class AuthPanel : MonoBehaviour
    {
        /// <summary>
        /// Ввод ника для аутентификации
        /// </summary>
        [SerializeField] private TMP_InputField UsernameLoginInput;

        /// <summary>
        /// Ввод пароля для аутентификации 
        /// </summary>
        [SerializeField] private TMP_InputField PasswordLoginInput;

        /// <summary>
        /// Панель Ошибки
        /// </summary>
        [SerializeField] private ErrorScreen _error;

        /// <summary>
        /// Панель Загрузки
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <inheritdoc cref="UserService"/>
        [Inject] private UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private SwitchSceneService _scene;

        /// <summary>
        /// Нажатие кнопки "Войти"
        /// </summary>
        public async void Login()
        {
            string username = UsernameLoginInput.text;
            string password = PasswordLoginInput.text;
            _loadingScreen.Show();
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                _loadingScreen.Hide();
                _error.Activate("Есть пустые поля");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.Activate("Меньше 8ми символов пароль");
                return;
            }
            try
            {
                await _userService.Authtenticate(username, password);
            }
            catch (Exception)
            {
                _loadingScreen.Hide();
                _error.Activate("Сервер не доступен\nПовторите попытку позже");
                return;
            }

            if (_userService.UserInfo.Id == Guid.Empty)
            {
                _loadingScreen.Hide();
                _error.Activate("Пользователь не найден");
                return;
            }

            _scene.ShowMainMenuScene();
        }
    }
}