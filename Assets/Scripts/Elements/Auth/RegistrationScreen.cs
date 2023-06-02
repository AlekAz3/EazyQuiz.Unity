using System;
using EazyQuiz.Unity.Controllers;
using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Auth
{
    public class RegistrationScreen : MonoBehaviour
    {
        /// <summary>
        /// Ввод ника
        /// </summary>
        [SerializeField] private TMP_InputField usernameRegisterInput;

        /// <summary>
        /// Ввод пароля  
        /// </summary>
        [SerializeField] private TMP_InputField passwordRegisterInput;

        /// <summary>
        /// Повтор пароля 
        /// </summary>
        [SerializeField] private TMP_InputField repeatPasswordRegisterInput;

        /// <summary>
        /// Выбор страны 
        /// </summary>
        [SerializeField] private TMP_Dropdown countryRegisterInput;

        /// <summary>
        /// Панель Ошибки
        /// </summary>
        [SerializeField] private InformationScreen error;

        /// <summary>
        /// Панель Загрузки
        /// </summary>
        [SerializeField] private LoadingScreen loadingScreen;

        /// <summary>
        /// Панель
        /// </summary>
        [SerializeField] private AuthorizationPanel panel;

        /// <inheritdoc cref="ApiProvider"/>
        [Inject] private readonly ApiProvider _apiProvider;

        /// <summary>
        /// Нажатие кнопки "Зарегистрироваться"
        /// </summary>
        public async void Registrate()
        {
            loadingScreen.Show();
            var username = usernameRegisterInput.text.Trim();
            var password = passwordRegisterInput.text.Trim();
            var repeatPassword = repeatPasswordRegisterInput.text.Trim();
            var country = countryRegisterInput.captionText.text;

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || repeatPassword.IsNullOrEmpty())
            {
                loadingScreen.Hide();
                error.ShowError("Есть пустые поля");
                return;
            }
            if (username.Contains(' ') || password.Contains(' '))
            {
                loadingScreen.Hide();
                error.ShowError("В Логине или пароле присутствуют пробелы");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                loadingScreen.Hide();
                error.ShowError("В пароле меньше 8ми символов");
                return;
            }

            if (!password.IsEqual(repeatPassword))
            {
                loadingScreen.Hide();
                error.ShowError("Пароли не совпадают");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                loadingScreen.Hide();
                error.ShowError("В пароле спецсимволы запрещены\n\nВ качестве пароля можно использовать только буквы английского алфавита и цифры");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                loadingScreen.Hide();
                error.ShowError("Пароль слишком слабый\n\nДолжны присутствовать большие маленький буквы и цифры");
                return;
            }
            try
            {
                if (await _apiProvider.CheckUsername(username))
                {
                    loadingScreen.Hide();
                    error.ShowError("Такой ник уже существует");
                    return;
                }
            }
            catch (Exception)
            {
                loadingScreen.Hide();
                error.ShowError("Сервер не доступен\nПовторите попытку позже");
                return;
            }

            await _apiProvider.Registration(
                password,
                username,
                country
            );
            loadingScreen.Hide();
            panel.Switch();

            usernameRegisterInput.text = "";
            passwordRegisterInput.text = "";
            repeatPasswordRegisterInput.text = "";
        }
    }
}