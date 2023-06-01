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
        [SerializeField] private InformationScreen _error;

        /// <summary>
        /// Панель Загрузки
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        [Inject] private readonly SaveUserService _saveUser;

        private void Awake()
        {
            var user = _saveUser.LoadUser();
            if (user != null)
            {
                _userService.SetUser(user);
                _scene.ShowMainMenuScene();
            }
        }

        /// <summary>
        /// Нажатие кнопки "Войти"
        /// </summary>
        public async void Login()
        {
            string username = UsernameLoginInput.text.Trim();
            string password = PasswordLoginInput.text.Trim();

            _loadingScreen.Show();

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                _loadingScreen.Hide();
                _error.ShowError("Есть пустые поля");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("Меньше 8ми символов пароль");
                return;
            }

            try
            {
                await _userService.Authtenticate(username, password);
            }
            catch (Exception ex)
            {
                _loadingScreen.Hide();
                _error.ShowError("Сервер не доступен\nПовторите попытку позже");
                Debug.LogException(ex);
                return;
            }

            if (_userService.UserInfo is null)
            {
                _loadingScreen.Hide();
                _error.ShowError("Ник или пароль введены неправильно");
                return;
            }

            _scene.ShowMainMenuScene();
        }
    }
}