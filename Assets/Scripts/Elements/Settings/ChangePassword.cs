using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Settings
{
    /// <summary>
    /// ����� ������
    /// </summary>
    public class ChangePassword : MonoBehaviour
    {
        /// <summary>
        /// ����� ������
        /// </summary>
        [SerializeField] private TMP_InputField newPassword;
        
        /// <summary>
        /// ������ ������
        /// </summary>
        [SerializeField] private TMP_InputField repeatNewPassword;
        
        /// <summary>
        /// ����������
        /// </summary>
        [SerializeField] private InformationScreen information;

        /// <summary>
        /// ��������
        /// </summary>
        [SerializeField] private LoadingScreen loading;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;
        
        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="SaveUserService"/>
        [Inject] private readonly SaveUserService _saveUser;


        public async void ChangePasswordButtonClick()
        {
            var password = newPassword.text.Trim();
            var repeatPassword = this.repeatNewPassword.text.Trim();

            if (password.IsNullOrEmpty() || repeatPassword.IsNullOrEmpty())
            {
                information.ShowError("���� ������ ����");
                return;
            }
            
            if (!password.IsMoreEightSymbols())
            {
                information.ShowError("� ������ ������ 8�� ��������");
                return;
            }

            if (!password.IsEqual(repeatPassword))
            {
                information.ShowError("������ �� ���������");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                information.ShowError("� ������ ����������� ���������\n\n� �������� ������ ����� ������������ ������ ����� ����������� �������� � �����");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                information.ShowError("������ ������� ������\n\n������ �������������� ������� ��������� ����� � �����");
                return;
            }
            
            loading.Show();
            await _userService.ChangePassword(password);
            loading.Hide();
        }
    }
}
