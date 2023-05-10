using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Auth
{
    public class AuthPanel : MonoBehaviour
    {
        /// <summary>
        /// ���� ���� ��� ��������������
        /// </summary>
        [SerializeField] private TMP_InputField UsernameLoginInput;

        /// <summary>
        /// ���� ������ ��� �������������� 
        /// </summary>
        [SerializeField] private TMP_InputField PasswordLoginInput;

        /// <summary>
        /// ������ ������
        /// </summary>
        [SerializeField] private InformationScreen _error;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        /// <summary>
        /// ������� ������ "�����"
        /// </summary>
        public async void Login()
        {
            string username = UsernameLoginInput.text;
            string password = PasswordLoginInput.text;
            _loadingScreen.Show();
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                _loadingScreen.Hide();
                _error.ShowError("���� ������ ����");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.ShowError("������ 8�� �������� ������");
                return;
            }
            try
            {
                await _userService.Authtenticate(username, password);
            }
            catch (Exception)
            {
                _loadingScreen.Hide();
                _error.ShowError("������ �� ��������\n��������� ������� �����");
                return;
            }

            if (_userService.UserInfo is null)
            {
                _loadingScreen.Hide();
                _error.ShowError("������������ �� ������");
                return;
            }

            _scene.ShowMainMenuScene();
        }
    }
}