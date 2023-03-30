using EazyQuiz.Extensions;
using EazyQuiz.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// ���������� ������ �������������� � �����������
/// </summary>
public class AuthController : MonoBehaviour
{
    /// <summary>
    /// ������ ��������������
    /// </summary>
    [SerializeField] private GameObject LoginGO;

    /// <summary>
    /// ������ �����������
    /// </summary>
    [SerializeField] private GameObject RegisterGO;

    /// <summary>
    /// ������� "����"
    /// </summary>
    [SerializeField] private GameObject LoginLabel;

    /// <summary>
    /// ������� "�����������"
    /// </summary>
    [SerializeField] private GameObject RegisterLabel;
    
    /// <summary>
    /// ���� ���� ��� ��������������
    /// </summary>
    [SerializeField] private TMP_InputField UsernameLoginInput;

    /// <summary>
    /// ���� ������ ��� �������������� 
    /// </summary>
    [SerializeField] private TMP_InputField PasswordLoginInput;


    /// <summary>
    /// ���� ���� ��� �����������
    /// </summary>
    [SerializeField] private TMP_InputField UsernameRegisteInput;

    /// <summary>
    /// ���� ������ ��� ����������� 
    /// </summary>
    [SerializeField] private TMP_InputField PasswordRegisteInput;

    /// <summary>
    /// ������ ������ ��� �����������
    /// </summary>
    [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;

    /// <summary>
    /// ���� �������� ��� �����������
    /// </summary>
    [SerializeField] private TMP_InputField AgeRegisteInput;

    /// <summary>
    /// ����� ���� ��� �����������
    /// </summary>
    [SerializeField] private TMP_Dropdown GenderRegisteInput;

    /// <summary>
    /// ����� ������ ��� �����������
    /// </summary>
    [SerializeField] private TMP_Dropdown CountryRegisteInput;

    /// <summary>
    /// ������ ������
    /// </summary>
    [SerializeField] private ErrorScreen _error;


    /// <summary>
    /// ������ ��������
    /// </summary>
    [SerializeField] private LoadingScreen _loadingScreen;

    /// <inheritdoc cref="UserService"/>
    [Inject] private UserService _userService;

    /// <inheritdoc cref="ApiProvider"/>
    [Inject] private ApiProvider _apiProvider;

    private void Awake()
    {
        Screen.fullScreen = false;
    }

    /// <summary>
    /// ������������ ����� � �����������
    /// </summary>
    public void Switch()
    {
        LoginGO.SetActive(!LoginGO.activeSelf);
        LoginLabel.SetActive(!LoginLabel.activeSelf);

        RegisterGO.SetActive(!RegisterGO.activeSelf);
        RegisterLabel.SetActive(!RegisterLabel.activeSelf);
    }

    /// <summary>
    /// ������� ������ "�����"
    /// </summary>
    public async void Login()
    {
        string username = UsernameLoginInput.text;
        string password = PasswordLoginInput.text;
        _loadingScreen.Show();
        if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
        {
            _loadingScreen.Hide();
            _error.Activate("���� ������ ����");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            _loadingScreen.Hide();
            _error.Activate("������ 8�� �������� ������");
            return;
        }
        try
        {
            await _userService.Authtenticate(username, password);
        }
        catch (Exception)
        {
            _loadingScreen.Hide();
            _error.Activate("������ �� ��������\n��������� ������� �����");
            return;
        }

        if (_userService.UserInfo.Id == Guid.Empty)    
        {
            _loadingScreen.Hide();
            _error.Activate("������������ �� ������");
            return;
        }

        SceneManager.LoadScene("MainMenu");

    }

    /// <summary>
    /// ������� ������ "������������������"
    /// </summary>
    public async void Registrate()
    {
        string username = UsernameRegisteInput.text;
        string password = PasswordRegisteInput.text;
        string repeatpassword = RepeatPasswordRegisteInput.text;
        string age = AgeRegisteInput.text;
        string gender = GenderRegisteInput.captionText.text;
        string country = CountryRegisteInput.captionText.text;

        if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || repeatpassword.IsNullOrEmpty() || age.IsNullOrEmpty())
        {
            _error.Activate("���� ������ ����");
            return;
        }

        if (!password.IsMoreEightSymbols())
        {
            _error.Activate("� ������ ������ 8�� ��������");
            return;
        }

        if (!password.IsEqual(repeatpassword))
        {
            _error.Activate("������ �� ���������");
            return;
        }

        if (!password.IsNoBannedSymbols())
        {
            _error.Activate("� ������ ����������� ���������\n\n� �������� ������ ����� ������������ ������ ����� ����������� �������� � �����");
            return;
        }

        if (!(password.IsContaintsUpperCaseLetter() && password.IsContaintsLowerCaseLetter() && password.IsContaintsNumeric()))
        {
            _error.Activate("������ ������� ������\n\n������ �������������� ������� ��������� ����� � �����");
            return;
        }

        if (Convert.ToInt32(age) <= 0)
        {
            _error.Activate("�������� �������");
            return;
        }

        _loadingScreen.Show();

        try
        {
            if (await _apiProvider.CheckUsername(username))
            {
                _loadingScreen.Hide();
                _error.Activate("����� ��� ��� ����������");
                return;
            }
        }
        catch (Exception)
        {
            _loadingScreen.Hide();
            _error.Activate("������ �� ��������\n��������� ������� �����");
            return;
        }

        await _apiProvider.Registrate(
            password, 
            username,
            Convert.ToInt32(age), 
            gender,
            country
        );
        _loadingScreen.Hide();
        Switch();
    }
}
