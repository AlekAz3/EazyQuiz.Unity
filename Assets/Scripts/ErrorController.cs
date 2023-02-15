using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorController : MonoBehaviour
{

    [SerializeField] private GameObject ErrorLabel;
    [SerializeField] private TMP_Text ErrorText;
    [SerializeField] private Button Button;

    public void OkButtonClick()
    {
        ErrorLabel.SetActive(false);
    }

    public void Activate(string error)
    {
        ErrorLabel.SetActive(true);
        ErrorText.text = error;
    }

}
