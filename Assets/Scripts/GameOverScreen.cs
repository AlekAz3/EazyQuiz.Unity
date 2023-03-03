using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]private GameObject screen;

    public Task Show(bool isTrueAnswer)
    {
        Instantiate(screen);
        return Task.CompletedTask;
    }
}
