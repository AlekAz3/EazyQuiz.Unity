using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    [SerializeField] private TMP_InputField UsernameInput;
    [SerializeField] private TMP_InputField PasswordInput;

    private UserResponse user;

    private ApiProvider _apiProvider;

    // Start is called before the first frame update
    void Start()
    {
        _apiProvider = new ApiProvider();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(user);
    }

    public async void EnterButtonClick()
    {
        var a = await _apiProvider.Authtenticate(UsernameInput.text, PasswordInput.text);
        user = a;
    }

}
