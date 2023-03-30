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
        [SerializeField] private ErrorScreen _error;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <inheritdoc cref="UserService"/>
        [Inject] private UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private SwitchSceneService _scene;

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
                _error.Activate("���� ������ ����");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                _loadingScreen.Hide();
                _error.Activate("������ 8�� �������� ������");
                return;
            }
            try
            {
                await _userService.Authtenticate(username, password);
            }
            catch (Exception)
            {
                _loadingScreen.Hide();
                _error.Activate("������ �� ��������\n��������� ������� �����");
                return;
            }

            if (_userService.UserInfo.Id == Guid.Empty)
            {
                _loadingScreen.Hide();
                _error.Activate("������������ �� ������");
                return;
            }

            _scene.ShowMainMenuScene();
        }
    }
}