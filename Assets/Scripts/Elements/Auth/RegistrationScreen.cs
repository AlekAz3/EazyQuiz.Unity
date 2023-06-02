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
        /// ���� ����
        /// </summary>
        [SerializeField] private TMP_InputField usernameRegisterInput;

        /// <summary>
        /// ���� ������  
        /// </summary>
        [SerializeField] private TMP_InputField passwordRegisterInput;

        /// <summary>
        /// ������ ������ 
        /// </summary>
        [SerializeField] private TMP_InputField repeatPasswordRegisterInput;

        /// <summary>
        /// ����� ������ 
        /// </summary>
        [SerializeField] private TMP_Dropdown countryRegisterInput;

        /// <summary>
        /// ������ ������
        /// </summary>
        [SerializeField] private InformationScreen error;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [SerializeField] private LoadingScreen loadingScreen;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private AuthorizationPanel panel;

        /// <inheritdoc cref="ApiProvider"/>
        [Inject] private readonly ApiProvider _apiProvider;

        /// <summary>
        /// ������� ������ "������������������"
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
                error.ShowError("���� ������ ����");
                return;
            }
            if (username.Contains(' ') || password.Contains(' '))
            {
                loadingScreen.Hide();
                error.ShowError("� ������ ��� ������ ������������ �������");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                loadingScreen.Hide();
                error.ShowError("� ������ ������ 8�� ��������");
                return;
            }

            if (!password.IsEqual(repeatPassword))
            {
                loadingScreen.Hide();
                error.ShowError("������ �� ���������");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                loadingScreen.Hide();
                error.ShowError("� ������ ����������� ���������\n\n� �������� ������ ����� ������������ ������ ����� ����������� �������� � �����");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                loadingScreen.Hide();
                error.ShowError("������ ������� ������\n\n������ �������������� ������� ��������� ����� � �����");
                return;
            }
            try
            {
                if (await _apiProvider.CheckUsername(username))
                {
                    loadingScreen.Hide();
                    error.ShowError("����� ��� ��� ����������");
                    return;
                }
            }
            catch (Exception)
            {
                loadingScreen.Hide();
                error.ShowError("������ �� ��������\n��������� ������� �����");
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