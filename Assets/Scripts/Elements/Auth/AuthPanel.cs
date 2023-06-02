using System;
using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
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
        [SerializeField] private TMP_InputField usernameLoginInput;

        /// <summary>
        /// ���� ������ ��� �������������� 
        /// </summary>
        [SerializeField] private TMP_InputField passwordLoginInput;

        /// <summary>
        /// ������ ������
        /// </summary>
        [SerializeField] private InformationScreen error;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [SerializeField] private LoadingScreen loadingScreen;

        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        [Inject] private readonly SaveUserService _saveUser;

        private async void Start()
        {
           var user = _saveUser.LoadUser();

            try
            {
                if (user == null || (Application.internetReachability == NetworkReachability.NotReachable)) 
                    return;
                
                loadingScreen.Show();
                if (await _userService.SetUser(user))
                {
                    _scene.ShowMainMenuScene();
                }
                loadingScreen.Hide();
            }
            catch (Exception)
            {
                loadingScreen.Hide();
                error.ShowError("������ �� ��������\n��������� ������� �����");
            }
        }

        /// <summary>
        /// ������� ������ "�����"
        /// </summary>
        public async void Login()
        {
            var username = usernameLoginInput.text.Trim();
            var password = passwordLoginInput.text.Trim();

            loadingScreen.Show();

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                loadingScreen.Hide();
                error.ShowError("���� ������ ����");
                return;
            }

            if (!password.IsMoreEightSymbols())
            {
                loadingScreen.Hide();
                error.ShowError("������ 8�� �������� ������");
                return;
            }

            try
            {
                await _userService.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                loadingScreen.Hide();
                error.ShowError("������ �� ��������\n��������� ������� �����");
                Debug.LogException(ex);
                return;
            }

            if (_userService.UserInfo is null)
            {
                loadingScreen.Hide();
                error.ShowError("��� ��� ������ ������� �����������");
                return;
            }

            _scene.ShowMainMenuScene();
        }
    }
}