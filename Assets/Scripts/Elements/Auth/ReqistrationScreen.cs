using EazyQuiz.Extensions;
using EazyQuiz.Unity.Controllers;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Auth
{
    public class ReqistrationScreen : MonoBehaviour
    {
        /// <summary>
        /// Ввод ника
        /// </summary>
        [SerializeField] private TMP_InputField UsernameRegisteInput;

        /// <summary>
        /// Ввод пароля  
        /// </summary>
        [SerializeField] private TMP_InputField PasswordRegisteInput;

        /// <summary>
        /// Повтор пароля 
        /// </summary>
        [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;

        /// <summary>
        /// Ввод возраста 
        /// </summary>
        [SerializeField] private TMP_InputField AgeRegisteInput;

        /// <summary>
        /// Выбор пола 
        /// </summary>
        [SerializeField] private TMP_Dropdown GenderRegisteInput;

        /// <summary>
        /// Выбор страны 
        /// </summary>
        [SerializeField] private TMP_Dropdown CountryRegisteInput;

        /// <summary>
        /// Панель Ошибки
        /// </summary>
        [SerializeField] private InformationScreen _error;

        /// <summary>
        /// Панель Загрузки
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <summary>
        /// Панель
        /// </summary>
        [SerializeField] private AuthtorizationPanel _panel;

        /// <inheritdoc cref="ApiProvider"/>
        [Inject] private readonly ApiProvider _apiProvider;

        /// <summary>
        /// Нажатие кнопки "Зарегистрироваться"
        /// </summary>
        public async void Registrate()
        {
            _loadingScreen.Show();
            string username = UsernameRegisteInput.text.Trim();
            string password = PasswordRegisteInput.text.Trim();
            string repeatpassword = RepeatPasswordRegisteInput.text.Trim();
            string country = CountryRegisteInput.captionText.text;

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || repeatpassword.IsNullOrEmpty())
            {
                _loadingScreen.Hide();
                _error.ShowError("Есть пустые поля");
                return;
            }
            if (username.Contains(' ') || password.Contains(' '))
            {
                _loadingScreen.Hide();
                _error.ShowError("В Логине или пароле присутствуют пробелы");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("В пароле меньше 8ми символов");
                return;
            }

            if (!password.IsEqual(repeatpassword))
            {
                _loadingScreen.Hide();
                _error.ShowError("Пароли не совпадают");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("В пароле спецсимволы запрещены\n\nВ качестве пароля можно использовать только буквы английского алфавита и цифры");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                _loadingScreen.Hide();
                _error.ShowError("Пароль слишком слабый\n\nДолжны присутствовать большие маленький буквы и цифры");
                return;
            }
            try
            {
                if (await _apiProvider.CheckUsername(username))
                {
                    _loadingScreen.Hide();
                    _error.ShowError("Такой ник уже существует");
                    return;
                }
            }
            catch (Exception)
            {
                _loadingScreen.Hide();
                _error.ShowError("Сервер не доступен\nПовторите попытку позже");
                return;
            }

            await _apiProvider.Registrate(
                password,
                username,
                country
            );
            _loadingScreen.Hide();
            _panel.Switch();

            UsernameRegisteInput.text = "";
            PasswordRegisteInput.text = "";
            RepeatPasswordRegisteInput.text = "";
        }
    }
}