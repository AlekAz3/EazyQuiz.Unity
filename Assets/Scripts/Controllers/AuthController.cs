using EazyQuiz.Extensions;
using EazyQuiz.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Контроллер панели Аутентификации и Регистрации
/// </summary>
public class AuthController : MonoBehaviour
{
    /// <summary>
    /// Панель Аутентификации
    /// </summary>
    [SerializeField] private GameObject LoginGO;

    /// <summary>
    /// Панель Регистрации
    /// </summary>
    [SerializeField] private GameObject RegisterGO;

    /// <summary>
    /// Надпись "Вход"
    /// </summary>
    [SerializeField] private GameObject LoginLabel;

    /// <summary>
    /// Надпись "Регистрация"
    /// </summary>
    [SerializeField] private GameObject RegisterLabel;
    
    /// <summary>
    /// Ввод ника для аутентификации
    /// </summary>
    [SerializeField] private TMP_InputField UsernameLoginInput;

    /// <summary>
    /// Ввод пароля для аутентификации 
    /// </summary>
    [SerializeField] private TMP_InputField PasswordLoginInput;


    /// <summary>
    /// Ввод ника для регистрации
    /// </summary>
    [SerializeField] private TMP_InputField UsernameRegisteInput;

    /// <summary>
    /// Ввод пароля для регистрации 
    /// </summary>
    [SerializeField] private TMP_InputField PasswordRegisteInput;

    /// <summary>
    /// Повтор пароля для регистрации
    /// </summary>
    [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;

    /// <summary>
    /// Ввод возраста для регистрации
    /// </summary>
    [SerializeField] private TMP_InputField AgeRegisteInput;

    /// <summary>
    /// Выбор пола для регистрации
    /// </summary>
    [SerializeField] private TMP_Dropdown GenderRegisteInput;

    /// <summary>
    /// Выбор страны для регистрации
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

    /// <inheritdoc cref="UserService"/>
    [Inject] private UserService _userService;

    /// <inheritdoc cref="ApiProvider"/>
    [Inject] private ApiProvider _apiProvider;

    private void Awake()
    {
        Screen.fullScreen = false;
    }

    /// <summary>
    /// Переключение входа и регистрации
    /// </summary>
    public void Switch()
    {
        LoginGO.SetActive(!LoginGO.activeSelf);
        LoginLabel.SetActive(!LoginLabel.activeSelf);

        RegisterGO.SetActive(!RegisterGO.activeSelf);
        RegisterLabel.SetActive(!RegisterLabel.activeSelf);
    }

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

        SceneManager.LoadScene("MainMenu");

    }

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
        Switch();
    }
}
