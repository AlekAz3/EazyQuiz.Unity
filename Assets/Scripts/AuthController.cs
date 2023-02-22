using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using Newtonsoft.Json;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class AuthController : MonoBehaviour
{
    [SerializeField] private GameObject LoginGO;
    [SerializeField] private GameObject RegisterGO;
    [SerializeField] private GameObject ErrorGO;

    [SerializeField] private GameObject LoginLabel;
    [SerializeField] private GameObject RegisterLabel;

    [SerializeField] private TMP_InputField UsernameLoginInput;
    [SerializeField] private TMP_InputField PasswordLoginInput;

    [SerializeField] private TMP_InputField UsernameRegisteInput;
    [SerializeField] private TMP_InputField PasswordRegisteInput;
    [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;
    [SerializeField] private TMP_InputField AgeRegisteInput;
    [SerializeField] private TMP_Dropdown GenderRegisteInput;
    [SerializeField] private TMP_Dropdown CountryRegisteInput;
    [Inject] private ErrorScreenService _errorService;

    [Inject] private UserService _userService;
    [Inject] private ApiProvider _apiProvider;
  
    public void Switch()
    {
        LoginGO.SetActive(!LoginGO.activeSelf);
        LoginLabel.SetActive(!LoginLabel.activeSelf);

        RegisterGO.SetActive(!RegisterGO.activeSelf);
        RegisterLabel.SetActive(!RegisterLabel.activeSelf);
    }

    public async void Login()
    {
        string username = UsernameLoginInput.text;
        string password = PasswordLoginInput.text;

        if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
        {
            _errorService.Activate("Есть пустые поля");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            _errorService.Activate("Меньше 8ми символов пароль");
            return;
        }

        await _userService.Authtenticate(username, password);

        if (_userService.UserInfo.Id == 0)
        {
            _errorService.Activate("Пользователь не найден\n\nНеверный логин/пароль");
            return;
        }

        SceneManager.LoadScene("MainMenu");

    }

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
            _errorService.Activate("Есть пустые поля");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            _errorService.Activate("В пароле меньше 8ми символов");
            return;
        }

        if (!password.IsEqual(repeatpassword))
        {
            _errorService.Activate("Пароли не совпадают");
            return;
        }

        if (!password.IsNoBannedSymbols())
        {
            _errorService.Activate("В пароле спецсимволы запрещены\n\nВ качестве пароля можно использовать только буквы английского алфавита и цифры");
            return;
        }

        if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
        {
            _errorService.Activate("Пароль слишком слабый\n\nДолжны присутствовать большие маленький буквы и цифры");
            return;
        }

        if (Convert.ToInt32(age) <= 0)
        {
            _errorService.Activate("Неверный возраст");
            return;
        }

        if ( await _apiProvider.CheckUsername(username))
        {
            _errorService.Activate("Такой ник уже существует");
            return;
        }

        await _apiProvider.Registrate(
            password, 
            username,
            Convert.ToInt32(age), 
            gender,
            country
        );
        Switch();
    }
}
