using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]private GameObject screen;

    [SerializeField] private TMP_Text text;

    public void Show(string inpitText)
    {
        screen.SetActive(true);
        text.text = inpitText;
    }

    public void Hide()
    {
        screen.SetActive(false);
    }
}
