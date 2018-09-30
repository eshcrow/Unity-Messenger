using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    [Header ("InputFields")]

    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    [Header ("Buttons")]

    public Button loginButton;
    public Button signUpButton;

    [Header ("Strings")]

    private string username = string.Empty;
    private string password = string.Empty;

    private void Awake ()
    {
        loginButton.interactable = false;
    }

    public void OnUsernameInputFieldEditEnded (string usernameString)
    {
        if (!string.IsNullOrEmpty (usernameString))
        {
            username = usernameString;
        }

        CheckAndEnableLoginButton ();
    }

    public void OnPasswordInputFieldEditEnded (string passwordString)
    {
        if (!string.IsNullOrEmpty (passwordString))
        {
            password = passwordString;
        }

        CheckAndEnableLoginButton ();
    }

    private void CheckAndEnableLoginButton ()
    {
        bool isUsernameEntered = !string.IsNullOrEmpty (username);

        bool isPasswordEntered = !string.IsNullOrEmpty (password);

        loginButton.interactable = (isUsernameEntered && isPasswordEntered);
    }

    public void OnLoginButtonClicked ()
    {
        Messenger.Instance.Login (username, password);
    }

    public void OnSignUpButtonClicked ()
    {

    }
}