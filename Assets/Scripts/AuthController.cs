using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    [SerializeField] private GameObject LoginGO;
    [SerializeField] private GameObject RegisterGO;

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

    private UserResponse user;

    private ApiProvider _apiProvider;

    private void Start()
    {
        _apiProvider = new ApiProvider();
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
        var user = await _apiProvider.Authtenticate(UsernameLoginInput.text, PasswordLoginInput.text);
        Debug.Log(JsonConvert.SerializeObject(user)); 
    }

    public async void Registrate()
    {
        await _apiProvider.Registrate(
            PasswordRegisteInput.text, 
            UsernameRegisteInput.text, 
            Convert.ToInt32(AgeRegisteInput.text), 
            GenderRegisteInput.itemText.text, 
            CountryRegisteInput.itemText.text);
    }
}
