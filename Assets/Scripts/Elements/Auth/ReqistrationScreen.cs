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
        /// ���� ����
        /// </summary>
        [SerializeField] private TMP_InputField UsernameRegisteInput;

        /// <summary>
        /// ���� ������  
        /// </summary>
        [SerializeField] private TMP_InputField PasswordRegisteInput;

        /// <summary>
        /// ������ ������ 
        /// </summary>
        [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;

        /// <summary>
        /// ���� �������� 
        /// </summary>
        [SerializeField] private TMP_InputField AgeRegisteInput;

        /// <summary>
        /// ����� ���� 
        /// </summary>
        [SerializeField] private TMP_Dropdown GenderRegisteInput;

        /// <summary>
        /// ����� ������ 
        /// </summary>
        [SerializeField] private TMP_Dropdown CountryRegisteInput;

        /// <summary>
        /// ������ ������
        /// </summary>
        [SerializeField] private InformationScreen _error;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private AuthtorizationPanel _panel;

        /// <inheritdoc cref="ApiProvider"/>
        [Inject] private readonly ApiProvider _apiProvider;

        /// <summary>
        /// ������� ������ "������������������"
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
                _error.ShowError("���� ������ ����");
                return;
            }
            if (username.Contains(' ') || password.Contains(' '))
            {
                _loadingScreen.Hide();
                _error.ShowError("� ������ ��� ������ ������������ �������");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("� ������ ������ 8�� ��������");
                return;
            }

            if (!password.IsEqual(repeatpassword))
            {
                _loadingScreen.Hide();
                _error.ShowError("������ �� ���������");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("� ������ ����������� ���������\n\n� �������� ������ ����� ������������ ������ ����� ����������� �������� � �����");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                _loadingScreen.Hide();
                _error.ShowError("������ ������� ������\n\n������ �������������� ������� ��������� ����� � �����");
                return;
            }
            try
            {
                if (await _apiProvider.CheckUsername(username))
                {
                    _loadingScreen.Hide();
                    _error.ShowError("����� ��� ��� ����������");
                    return;
                }
            }
            catch (Exception)
            {
                _loadingScreen.Hide();
                _error.ShowError("������ �� ��������\n��������� ������� �����");
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