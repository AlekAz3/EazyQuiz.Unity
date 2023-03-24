using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorScreen : MonoBehaviour
{
    [SerializeField] private GameObject ErrorLabel;
    [SerializeField] private TMP_Text ErrorText;

    public void Hide()
    {
        ErrorLabel.SetActive(false);
    }

    public void Activate(string error)
    {
        ErrorLabel.SetActive(true);
        ErrorText.text = error;
    }
}

