using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Leaderboard
{
    /// <summary>
    /// Карточка пользователя
    /// </summary>
    public class UserPublicElement : MonoBehaviour
    {
        /// <summary>
        /// Карточка 
        /// </summary>
        [SerializeField] private GameObject element;
        
        /// <summary>
        /// Ник
        /// </summary>
        [SerializeField] private TMP_Text username;
        
        /// <summary>
        /// Счёт
        /// </summary>
        [SerializeField] private TMP_Text score;
        
        /// <summary>
        /// Место
        /// </summary>
        [SerializeField] private TMP_Text place;
        
        /// <summary>
        /// Применить пользователя на карточку
        /// </summary>
        /// <param name="placeInTop">Место в топе</param>
        /// <param name="userInfo">Информация о пользователе</param>
        public void ApplyUserPublicElement(int placeInTop, PublicUserInfo userInfo)
        {
            username.text = userInfo.UserName;
            score.text = userInfo.Points.ToString();
            place.text = placeInTop.ToString();
        }

        /// <summary>
        /// Очистка карточки
        /// </summary>
        public void Clear()
        {
            element.SetActive(true);
            username.text = string.Empty;
            score.text = string.Empty;
            place.text = string.Empty;
        }
        
        /// <summary>
        /// Скрыть карточку
        /// </summary>
        public void Hide()
        {
            element.SetActive(false);
        }
    }
}
