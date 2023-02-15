using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    [SerializeField] private GameObject Login;
    [SerializeField] private GameObject Register;

    private UserResponse user;

    private ApiProvider _apiProvider;

    void Start()
    {
        _apiProvider = new ApiProvider();
        
    }
    public void Switch()
    {
        Login.SetActive(!Login.activeSelf);
        Register.SetActive(!Register.activeSelf);
    }
}
