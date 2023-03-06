using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]private GameObject screen;

    [SerializeField] private TMP_Text text;

    public void Show(bool isTrueAnswer)
    {
        screen.SetActive(true);
        if (isTrueAnswer)
        {
            text.text = "Ответ верный!";
        }
        else
        {
            text.text = "Ответ не верный!";
        }
    }

    public void Hide()
    {
        screen.SetActive(false);
    }
}
