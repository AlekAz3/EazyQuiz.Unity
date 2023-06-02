using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Leaderboard
{
    public class UserPublicElement : MonoBehaviour
    {
        [SerializeField] private GameObject element;
        [SerializeField] private TMP_Text username;
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text place;
        
        public void ApplyUserPublicElement(int placeInTop, PublicUserInfo userInfo)
        {
            username.text = userInfo.UserName;
            score.text = userInfo.Points.ToString();
            place.text = placeInTop.ToString();
        }

        public void Clear()
        {
            element.SetActive(true);
            username.text = string.Empty;
            score.text = string.Empty;
            place.text = string.Empty;
        }

        public void Hide()
        {
            element.SetActive(false);
        }
    }
}
