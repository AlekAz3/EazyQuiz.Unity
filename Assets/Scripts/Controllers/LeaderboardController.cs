using System.Collections.Generic;
using System.Linq;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.Leaderboard;
using EazyQuiz.Unity.Services;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class LeaderboardController : MonoBehaviour
    {
        [SerializeField] private List<UserPublicElement> usersElements;
        [SerializeField] private LoadingScreen loading;

        [Inject] private readonly ApiProvider _apiProvider;
        [Inject] private readonly UserService _userService;
        [Inject] private readonly SwitchSceneService _scene;


        public void Awake()
        {
            RefrashLeaderboard(0);
        }

        public async void RefrashLeaderboard(int country)
        {
            loading.Show();
            foreach (var user in usersElements)
            {
                user.Clear();
            }

            string countryStr = null;

            switch (country)
            {
                case 0:
                    break;
                case 1:
                    countryStr = "������";
                    break;
                case 2:
                    countryStr = "��������";
                    break;
                case 3:
                    countryStr = "���������";
                    break;
                case 4:
                    countryStr = "�������";
                    break;
            }

            var userPosition = await _apiProvider.GetUserPosition(countryStr, _userService.UserInfo.Token.Jwt);
            var users = (await _apiProvider.GetLeaderboard(countryStr, _userService.UserInfo.Token.Jwt)).ToList();

            if (userPosition<=5)
            {
                usersElements.Last().Hide();
                for (var i = 0; i < users.Count; i++)
                {
                    usersElements[i].ApplyUserPublicElement(i + 1, users[i]);
                }
            }
            else
            {
                for (var i = 0; i < users.Count; i++)
                {
                    usersElements[i].ApplyUserPublicElement(i + 1, users[i]);
                }
                usersElements.Last().ApplyUserPublicElement(userPosition, new EazyQuiz.Models.DTO.PublicUserInfo() { UserName = _userService.UserInfo.UserName, Points = _userService.UserInfo.Points });
            }
            loading.Hide();
        }

        /// <summary>
        /// ����� � ������� ����
        /// </summary>
        public void ExitButtonClick()
        {
            loading.Show();
            _scene.ShowMainMenuScene();
        }
    }
}