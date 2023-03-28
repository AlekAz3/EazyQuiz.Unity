using TMPro;
using UnityEngine;

public class ErrorScreen : MonoBehaviour
{
    [SerializeField] public GameObject ErrorLabel;
    [SerializeField] private TMP_Text ErrorText;

    public void Hide()
    {
        Debug.Log("HideError");
        ErrorLabel.SetActive(false);
    }

    public void Activate(string error)
    {
        Debug.Log("Show Error");
        ErrorLabel.SetActive(true);
        ErrorText.text = error;
    }
}

