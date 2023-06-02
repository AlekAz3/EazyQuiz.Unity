using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Leaderboard
{
    /// <summary>
    /// �������� ������������
    /// </summary>
    public class UserPublicElement : MonoBehaviour
    {
        /// <summary>
        /// �������� 
        /// </summary>
        [SerializeField] private GameObject element;
        
        /// <summary>
        /// ���
        /// </summary>
        [SerializeField] private TMP_Text username;
        
        /// <summary>
        /// ����
        /// </summary>
        [SerializeField] private TMP_Text score;
        
        /// <summary>
        /// �����
        /// </summary>
        [SerializeField] private TMP_Text place;
        
        /// <summary>
        /// ��������� ������������ �� ��������
        /// </summary>
        /// <param name="placeInTop">����� � ����</param>
        /// <param name="userInfo">���������� � ������������</param>
        public void ApplyUserPublicElement(int placeInTop, PublicUserInfo userInfo)
        {
            username.text = userInfo.UserName;
            score.text = userInfo.Points.ToString();
            place.text = placeInTop.ToString();
        }

        /// <summary>
        /// ������� ��������
        /// </summary>
        public void Clear()
        {
            element.SetActive(true);
            username.text = string.Empty;
            score.text = string.Empty;
            place.text = string.Empty;
        }
        
        /// <summary>
        /// ������ ��������
        /// </summary>
        public void Hide()
        {
            element.SetActive(false);
        }
    }
}
