using EazyQuiz.Models.DTO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainmenuController : MonoBehaviour
{
    private UserResponse User;

    [SerializeField] private TMP_Text UsernameLabel;
    [SerializeField] private TMP_Text PointsLabel;

    private void Awake()
    {
        User = GameObject.Find("User").GetComponent<UserController>().User;
        UsernameLabel.text = User.UserName;
        PointsLabel.text = $"Очки: {User.Points}";
    }
}
