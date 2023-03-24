using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen;

    public void Show()
    {
        screen.SetActive(true);
    }

    public void Hide()
    {
        screen.SetActive(false);
    }
}
