using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen;

    public void Show()
    {
        screen.SetActive(true);
    }
}
