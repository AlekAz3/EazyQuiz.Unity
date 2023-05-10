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
        [SerializeField] private GameObject Object;
        [SerializeField] private Image Background;
        [SerializeField] private TMP_Text Place;
        [SerializeField] private TMP_Text Username;
        [SerializeField] private TMP_Text Score;

        public void ApplyUserPublucElement(int place, PublicUserInfo userInfo)
        {
            switch (place)
            {
                case 1:
                    Background.color = Color.yellow;
                    break;
                case 2:
                    Background.color = Color.gray;
                    break;
                case 3:
                    Background.color = Color.red;
                    break;
            }
            if (place>5)
            {
                Background.color = Color.cyan;
            }
            Place.text = place.ToString();
            Username.text = userInfo.UserName;
            Score.text = userInfo.Points.ToString();
        }

        public void Clear()
        {
            Object.SetActive(true);
            Background.color = Color.white;
            Username.text = string.Empty;
            Score.text = string.Empty;
        }

        public void Hide()
        {
            Object.SetActive(false);
        }
    }
}
