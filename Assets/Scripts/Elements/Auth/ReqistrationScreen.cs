using EazyQuiz.Extensions;
using EazyQuiz.Unity;
using System;
using TMPro;
using UnityEngine;
using Zenject;

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
    [SerializeField] private ErrorScreen _error;

    /// <summary>
    /// Панель Загрузки
    /// </summary>
    [SerializeField] private LoadingScreen _loadingScreen;

    /// <summary>
    /// Панель
    /// </summary>
    [SerializeField] private AuthtorizationPanel _panel;

    /// <inheritdoc cref="ApiProvider"/>
    [Inject] private ApiProvider _apiProvider;

    /// <summary>
    /// Нажатие кнопки "Зарегистрироваться"
    /// </summary>
    public async void Registrate()
    {
        string username = UsernameRegisteInput.text;
        string password = PasswordRegisteInput.text;
        string repeatpassword = RepeatPasswordRegisteInput.text;
        string age = AgeRegisteInput.text;
        string gender = GenderRegisteInput.captionText.text;
        string country = CountryRegisteInput.captionText.text;

        if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || repeatpassword.IsNullOrEmpty() || age.IsNullOrEmpty())
        {
            _error.Activate("Есть пустые поля");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            _error.Activate("В пароле меньше 8ми символов");
            return;
        }

        if (!password.IsEqual(repeatpassword))
        {
            _error.Activate("Пароли не совпадают");
            return;
        }

        if (!password.IsNoBannedSymbols())
        {
            _error.Activate("В пароле спецсимволы запрещены\n\nВ качестве пароля можно использовать только буквы английского алфавита и цифры");
            return;
        }

        if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
        {
            _error.Activate("Пароль слишком слабый\n\nДолжны присутствовать большие маленький буквы и цифры");
            return;
        }

        if (Convert.ToInt32(age) <= 0)
        {
            _error.Activate("Неверный возраст");
            return;
        }

        _loadingScreen.Show();

        try
        {
            if (await _apiProvider.CheckUsername(username))
            {
                _loadingScreen.Hide();
                _error.Activate("Такой ник уже существует");
                return;
            }
        }
        catch (Exception)
        {
            _loadingScreen.Hide();
            _error.Activate("Сервер не доступен\nПовторите попытку позже");
            return;
        }

        await _apiProvider.Registrate(
            password,
            username,
            Convert.ToInt32(age),
            gender,
            country
        );
        _loadingScreen.Hide();
        _panel.Switch();
    }
}
