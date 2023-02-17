using EazyQuiz.Models.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{

    [SerializeField] private GameObject user;

    public UserResponse User { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(user);
    }

}
