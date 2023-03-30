using EazyQuiz.Extensions;
using EazyQuiz.Unity;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class ReqistrationScreen : MonoBehaviour
{
    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField] private TMP_InputField UsernameRegisteInput;

    /// <summary>
    /// ���� ������  
    /// </summary>
    [SerializeField] private TMP_InputField PasswordRegisteInput;

    /// <summary>
    /// ������ ������ 
    /// </summary>
    [SerializeField] private TMP_InputField RepeatPasswordRegisteInput;

    /// <summary>
    /// ���� �������� 
    /// </summary>
    [SerializeField] private TMP_InputField AgeRegisteInput;

    /// <summary>
    /// ����� ���� 
    /// </summary>
    [SerializeField] private TMP_Dropdown GenderRegisteInput;

    /// <summary>
    /// ����� ������ 
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

    /// <summary>
    /// ������
    /// </summary>
    [SerializeField] private AuthtorizationPanel _panel;

    /// <inheritdoc cref="ApiProvider"/>
    [Inject] private ApiProvider _apiProvider;

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
        _panel.Switch();
    }
}
