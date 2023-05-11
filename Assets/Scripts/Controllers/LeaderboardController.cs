using EazyQuiz.Unity.Elements.Leaderboard;
using EazyQuiz.Unity.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class LeaderboardController : MonoBehaviour
    {
        [SerializeField] private List<UserPublicElement> usersElements;

        [Inject] private readonly ApiProvider _apiProvider;
        [Inject] private readonly UserService _userService;
        [Inject] private readonly SwitchSceneService _scene;

        public void Awake()
        {
            RefrashLeaderboard(0);
        }

        public async void RefrashLeaderboard(int country)
        {
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
                    countryStr = "Россия";
                    break;
                case 2:
                    countryStr = "Украина";
                    break;
                case 3:
                    countryStr = "Беларусь";
                    break;
            }

            var userPosition = await _apiProvider.GetUserPosition(countryStr, _userService.UserInfo.Token);
            var users = (await _apiProvider.GetLeaderboard(countryStr, _userService.UserInfo.Token)).ToList();

            if (userPosition<=5)
            {
                usersElements.Last().Hide();
                for (int i = 0; i < users.Count; i++)
                {
                    usersElements[i].ApplyUserPublucElement(i + 1, users[i]);
                }
            }
            else
            {
                for (int i = 0; i < users.Count; i++)
                {
                    usersElements[i].ApplyUserPublucElement(i + 1, users[i]);
                }
                usersElements.Last().ApplyUserPublucElement(userPosition, new Models.DTO.PublicUserInfo() { UserName = _userService.UserInfo.UserName, Points = _userService.UserInfo.Points });
            }
        }

        /// <summary>
        /// Выход в главное меню
        /// </summary>
        public void ExitButtonClick()
        {
            _scene.ShowMainMenuScene();
        }
    }
}