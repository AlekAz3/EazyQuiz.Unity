using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class AuthController : MonoBehaviour
{
    [SerializeField] private GameObject LoginGO;
    [SerializeField] private GameObject RegisterGO;
    [SerializeField] private GameObject ErrorGO;

    [SerializeField] private GameObject LoginLabel;
    [SerializeField] private GameObject RegisterLabel;

    [SerializeField] private GameObject userProfile;

    [SerializeField] private TMP_InputField UsernameLoginInput;
    [SerializeField] private TMP_InputField PasswordLoginInput;

    [SerializeField] private TMP_InputField UsernameRegisteInput;
    [SerializeField] private TMP_InputField PasswordRegisteInput;
    [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;
    [SerializeField] private TMP_InputField AgeRegisteInput;
    [SerializeField] private TMP_Dropdown GenderRegisteInput;
    [SerializeField] private TMP_Dropdown CountryRegisteInput;
    private ErrorController error;

    private UserResponse user;

    private ApiProvider _apiProvider;

    private void Start()
    {
        _apiProvider = new ApiProvider();
        error = ErrorGO.GetComponent<ErrorController>();
    }


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
            error.Activate("Есть пустые поля");
            return;
        }

        var user = await _apiProvider.Authtenticate(username, password);

        if (user.Id == 0)
        {
            error.Activate("Неверный логин/пароль");
            return;
        }

        userProfile.GetComponent<UserController>().User = user;
        Debug.Log(JsonConvert.SerializeObject(user));
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
            error.Activate("Есть пустые поля");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            error.Activate("В пароле меньше 8ми символов");
            return;
        }

        if (!password.IsEqual(repeatpassword))
        {
            error.Activate("Пароли не совпадают");
            return;
        }

        if (!password.IsNoBannedSymbols())
        {
            error.Activate("В пароле спецсимволы запрещены\n В качестве пароля можно использовать только буквы английского алфавита и цифры");
            return;
        }

        if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
        {
            error.Activate("Пароль слишком слабый \n В пароле должны присутствовать большие маленький буквы и цифры");
            return;
        }

        if (Convert.ToInt32(age) <= 0)
        {
            error.Activate("Неверный возраст");
            return;
        }

        if ( await _apiProvider.CheckUsername(username))
        {
            error.Activate("Такой ник уже существует");
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
