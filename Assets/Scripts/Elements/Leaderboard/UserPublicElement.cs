using EazyQuiz.Models.DTO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EazyQuiz.Unity.Elements.Leaderboard
{
    public class UserPublicElement : MonoBehaviour
    {
        [SerializeField] private Image Background;
        [SerializeField] private TMP_Text Text;

        public void ApplyUserPublucElement(int place, PublicUserInfo userInfo)
        {
            if(place == 1) 
            {
                Background.color = Color.yellow;
                Text.text = $"1. {userInfo.UserName} {userInfo.Points}";
            }

            if (place == 2)
            {
                Background.color = Color.gray;
                Text.text = $"2. {userInfo.UserName} {userInfo.Points}";
            }

            if (place == 3)
            {
                Background.color = Color.gray;
                Text.text = $"3. {userInfo.UserName} {userInfo.Points}";
            }
            Text.text = $"{place}. {userInfo.UserName} {userInfo.Points}";
        }
    }
}
