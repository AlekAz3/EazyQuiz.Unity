using EazyQuiz.Extensions;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Elements.Settings
{
    /// <summary>
    /// Смена ника
    /// </summary>
    public class ChangeUsername : MonoBehaviour
    {
        /// <summary>
        /// Новый ник
        /// </summary>
        [SerializeField] private TMP_InputField newUsername;
        
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
        
        /// <summary>
        /// Смена ника
        /// </summary>
        public async void ChangeNicknameButton()
        {
            var nickname = newUsername.text.Trim();
            
            if (nickname.IsNullOrEmpty())
            {
                information.ShowError("Пустое поле Ник");
                return;
            }
            loading.Show();

            var existUser = await _userService.CheckNickname(nickname);
            
            if (existUser)
            {
                loading.Hide();
                information.ShowError("Этот ник уже занят");
                return;
            }
            
            await _userService.ChangeNickname(nickname);
            
            loading.Hide();

            information.ShowInformation("Ваш ник успешно изменён");

            newUsername.text = string.Empty;
        }
    }
}
