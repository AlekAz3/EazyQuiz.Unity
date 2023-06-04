using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Settings
{
    /// <summary>
    /// Смена пароля
    /// </summary>
    public class ChangePassword : MonoBehaviour
    {
        /// <summary>
        /// Новый пароль
        /// </summary>
        [SerializeField] private TMP_InputField newPassword;
        
        /// <summary>
        /// Повтор пароля
        /// </summary>
        [SerializeField] private TMP_InputField repeatNewPassword;
        
        /// <summary>
        /// Информация
        /// </summary>
        [SerializeField] private InformationScreen information;

        /// <summary>
        /// Загрузка
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
                information.ShowError("Есть пустые поля");
                return;
            }
            
            if (!password.IsMoreEightSymbols())
            {
                information.ShowError("В пароле меньше 8ми символов");
                return;
            }

            if (!password.IsEqual(repeatPassword))
            {
                information.ShowError("Пароли не совпадают");
                return;
            }

            if (!password.IsNoBannedSymbols())
            {
                information.ShowError("В пароле спецсимволы запрещены\n\nВ качестве пароля можно использовать только буквы английского алфавита и цифры");
                return;
            }

            if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
            {
                information.ShowError("Пароль слишком слабый\n\nДолжны присутствовать большие маленький буквы и цифры");
                return;
            }
            
            loading.Show();
            await _userService.ChangePassword(password);
            loading.Hide();
        }
    }
}
