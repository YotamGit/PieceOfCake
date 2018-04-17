using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using DatabaseControl; 

public class DBCManager : MonoBehaviour {

    //All public variables bellow are assigned in the Inspector

    //These are the GameObjects which are parents of groups of UI elements. The objects are enabled and disabled to show and hide the UI elements.
    public GameObject loginParent;
    public GameObject registerParent;
    public GameObject loggedInParent;
    public GameObject loadingParent;

    //These are all the InputFields which we need in order to get the entered usernames, passwords, etc
    public TMP_InputField Login_UsernameField;
    public TMP_InputField Login_PasswordField;
    public TMP_InputField Register_UsernameField;
    public TMP_InputField Register_PasswordField;
    public TMP_InputField Register_ConfirmPasswordField;
    public TMP_InputField LoggedIn_DataInputField;
    public TMP_InputField LoggedIn_DataOutputField;

    //These are the UI Texts which display errors
    public TextMeshProUGUI Login_ErrorText;
    public TextMeshProUGUI Register_ErrorText;

    //This UI Text displays the username once logged in. It shows it in the form "Logged In As: " + username
    public TextMeshProUGUI LoggedIn_DisplayUsernameText;

    //These store the username and password of the player when they have logged in
    public string playerUsername = "";
    public string playerPassword = "";

    [SerializeField]
    private LobbyNetwork lobbyNetwork;

    [SerializeField]
    private MainMenu mainMenuScript;

    public Button loginButton, loginBackButton;

    public Button signinButton, signinBackButton;

    public TextMeshProUGUI PlayerStatsText;

    //Called at the very start of the game
    void Awake()
    {
        ResetAllUIElements();
        DontDestroyOnLoad(this);
    }

    //Called by Button Pressed Methods to Reset UI Fields
    void ResetAllUIElements()
    {
        //This resets all of the UI elements. It clears all the strings in the input fields and any errors being displayed
        /*Login_UsernameField.text = "";
        Login_PasswordField.text = "";
        Register_UsernameField.text = "";
        Register_PasswordField.text = "";
        Register_ConfirmPasswordField.text = "";*/
        /*LoggedIn_DataInputField.text = "";
        LoggedIn_DataOutputField.text = "";
        Login_ErrorText.text = "";
        Register_ErrorText.text = "";
        LoggedIn_DisplayUsernameText.text = "";*/
    }

    //Called by Button Pressed Methods. These use DatabaseControl namespace to communicate with server.
    IEnumerator LoginUser()
    {
        IEnumerator e = DCF.Login(playerUsername, playerPassword); // << Send request to login, providing username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //Username and Password were correct. Stop showing 'Loading...' and show the LoggedIn UI. And set the text to display the username.
            loginButton.interactable = true;
            loginBackButton.interactable = true;
            lobbyNetwork.JoinLobbyAs();
            /*ResetAllUIElements();
            loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);*/
            //LoggedIn_DisplayUsernameText.text = "Logged In As: " + playerUsername;
        }
        else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to LoginUI
            StartCoroutine(mainMenuScript.DisplayError("Invalid Username or Password\nPlease Try again")); // telling the main menu to display the error message
            loginButton.interactable = true;
            loginBackButton.interactable = true;
            /*loadingParent.gameObject.SetActive(false);
            loginParent.gameObject.SetActive(true);*/
            /*if (response == "UserError")
            {
                //The Username was wrong so display relevent error message
                Login_ErrorText.text = "Error: Username not Found";
            }
            else
            {
                if (response == "PassError")
                {
                    //The Password was wrong so display relevent error message
                    Login_ErrorText.text = "Error: Password Incorrect";
                } else
                {
                    //There was another error. This error message should never appear, but is here just in case.
                    Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
                }
            }*/
        }
    }
    IEnumerator RegisterUser()
    {
        IEnumerator e = DCF.RegisterUser(playerUsername, playerPassword, "Cakes Eaten: 0\nCakes Shared: 0\nDeaths: 0"); // << Send request to register a new user, providing submitted username and password. It also provides an initial value for the data string on the account, which is "Hello World".
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //Username and Password were valid. Account has been created. Stop showing 'Loading...' and show the loggedIn UI and set text to display the username.
            //TODO: enable login screen
            signinBackButton.interactable = true;
            signinButton.interactable = true;
            registerParent.SetActive(false);
            loginParent.SetActive(true);
            /*ResetAllUIElements();
            loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);
            LoggedIn_DisplayUsernameText.text = "Logged In As: " + playerUsername;*/
        } else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to RegisterUI
            /*loadingParent.gameObject.SetActive(false);
            registerParent.gameObject.SetActive(true);*/
            if (response == "UserError")
            {
                StartCoroutine(mainMenuScript.DisplayError("Username Already Exists")); // telling the main menu to display the error message

                //The username has already been taken. Player needs to choose another. Shows error message.
                //Register_ErrorText.text = "Error: Username Already Taken";
            }
            else
            {
                StartCoroutine(mainMenuScript.DisplayError("Unknown Error. Please try again later")); // telling the main menu to display the error message

                //There was another error. This error message should never appear, but is here just in case.
            }
            signinBackButton.interactable = true;
            signinButton.interactable = true;
        }
    }
    IEnumerator GetData()
    {
        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PhotonNetwork.Disconnect();
            /*ResetAllUIElements();
            playerUsername = "";
            playerPassword = "";
            loginParent.gameObject.SetActive(true);
            loadingParent.gameObject.SetActive(false);
            Login_ErrorText.text = "Error: Unknown Error. Please try again later.";*/
        }
        else
        {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            /*loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);*/
            //LoggedIn_DataOutputField.text = response;
            PlayerStatsText.text = response;
        }
    }
    IEnumerator AddDeathCount()
    {
        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PhotonNetwork.Disconnect();
        }
        else
        {
            string[] splitStr = response.Split('\n');
            int Deaths = System.Convert.ToInt32(splitStr[2].Substring(7, splitStr[2].Length - 7)) + 1;
            string newDeath = "Deaths: " + Deaths.ToString();
            StartCoroutine(SetDataAndRestart(splitStr[0] + "\n" + splitStr[1] + "\n" + newDeath));
        }
    }
    public IEnumerator AddVictoryCount(int cakesShared, int cakesEaten)
    {
        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PhotonNetwork.Disconnect();
        }
        else
        {
            string[] splitStr = response.Split('\n');
            
            int cakesEatenint = System.Convert.ToInt32(splitStr[0].Substring(13, splitStr[0].Length - 13)) + cakesEaten;
            string newCakesEaten = "Cakes Eaten: " + cakesEatenint.ToString();

            int cakesSharedint = System.Convert.ToInt32(splitStr[1].Substring(13, splitStr[1].Length - 13)) + cakesShared;
            string newCakesShared = "Cakes Shared: " + cakesSharedint.ToString();

            StartCoroutine(SetDataAndRestart(newCakesEaten + "\n" + newCakesShared + "\n" + splitStr[2]));
        }
    }
    IEnumerator SetData(string data)
    {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            /*loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);*/
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PhotonNetwork.Disconnect();
            /*ResetAllUIElements();
            playerUsername = "";
            playerPassword = "";
            loginParent.gameObject.SetActive(true);
            loadingParent.gameObject.SetActive(false);
            Login_ErrorText.text = "Error: Unknown Error. Please try again later.";*/
        }
    }

    private bool firstDBManager = false;

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0 && firstDBManager)
        {
            GameObject[] dataBaseManagers = GameObject.FindGameObjectsWithTag("DataBaseManager");

            if (dataBaseManagers.Length >= 1)
            {
                if(dataBaseManagers[0] == gameObject)
                {
                    DBCManager dbManager = dataBaseManagers[1].GetComponent<DBCManager>();
                    dbManager.playerUsername = playerUsername;
                    dbManager.playerPassword = playerPassword;
                }
                else
                {
                    DBCManager dbManager = dataBaseManagers[0].GetComponent<DBCManager>();
                    dbManager.playerUsername = playerUsername;
                    dbManager.playerPassword = playerPassword;
                }
                Destroy(gameObject);
            }
        }
        firstDBManager = true;
    }

    IEnumerator SetDataAndRestart(string data)
    {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PhotonNetwork.Disconnect();
        }
    }

    //user: min 3 letters - pass: min 6 letters. contains only letters and numbers
    private bool IsValidText(string text, int minLetters)
    {
        if (text.Length >= minLetters && text.Length <= 17) //there is a EOS at the end of the string
        {
            text = text.ToLower();
            for (int i = 0; i < text.Length - 1; i++)
            {
                if (!((text[i] > 96 && text[i] < 123) || (text[i] > 47 && text[i] < 58)))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    //UI Button Pressed Methods
    public void Login_LoginButtonPressed ()
    {
        //Called when player presses button to Login

        //Get the username and password the player entered
        playerUsername = Login_UsernameField.text.ToUpper();
        playerPassword = Login_PasswordField.text.ToUpper();
        if (IsValidText(playerUsername, 3) && IsValidText(playerPassword, 6))
        {
            PhotonNetwork.playerName = playerUsername;
        }
        else
        {
            StartCoroutine(mainMenuScript.DisplayError("Invalid Username or Password\nPlease Try again")); // telling the main menu to display the error message
            
            loginButton.interactable = true;
            loginBackButton.interactable = true;
            return;
        }
        StartCoroutine(LoginUser());
        /*loginButton.interactable = true;
        signinBackButton.interactable = true;*/
        /*//Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
        if (playerUsername.Length > 3)
        {
            if (playerPassword.Length > 5)
            {
                //Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
                loginParent.gameObject.SetActive(false);
                loadingParent.gameObject.SetActive(true);
                StartCoroutine(LoginUser());
            }
            else
            {
                //Password too short so it must be wrong
                Login_ErrorText.text = "Error: Password Incorrect";
            }
        } else
        {
            //Username too short so it must be wrong
            Login_ErrorText.text = "Error: Username Incorrect";
        }*/
    }
    public void Login_RegisterButtonPressed ()
    {
        //Called when the player hits register on the Login UI, so switches to the Register UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(false);
        registerParent.gameObject.SetActive(true);
    }
    public void Register_RegisterButtonPressed ()
    {
        //Called when the player presses the button to register

        //Get the username and password and repeated password the player entered
        playerUsername = Register_UsernameField.text.ToUpper();
        playerPassword = Register_PasswordField.text.ToUpper(); 
        string confirmedPassword = Register_ConfirmPasswordField.text.ToUpper();
                
        //Make sure username and password are long enough
        if (IsValidText(playerUsername, 3) && IsValidText(playerPassword, 6))
        {
            if (playerPassword == confirmedPassword)
            {
                StartCoroutine(RegisterUser());
            }
            else
            {
                StartCoroutine(mainMenuScript.DisplayError("Passwords Do Not Match"));

                signinBackButton.interactable = true;
                signinButton.interactable = true;
            }
        }
        else
        {
            StartCoroutine(mainMenuScript.DisplayError("Invalid Username or Password\nPlease Try again")); // telling the main menu to display the error message
            
            signinBackButton.interactable = true;
            signinButton.interactable = true;
            return;
        }

        /*if (playerUsername.Length > 3)
        {
            if (playerPassword.Length > 5)
            {
                //Check the two passwords entered match
                if (playerPassword == confirmedPassword)
                {
                    //Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
                    registerParent.gameObject.SetActive(false);
                    loadingParent.gameObject.SetActive(true);
                    StartCoroutine(RegisterUser());
                }
                else
                {
                    //Passwords don't match, show error
                    Register_ErrorText.text = "Error: Password's don't Match";
                }
            }
            else
            {
                //Password too short so show error
                Register_ErrorText.text = "Error: Password too Short";
            }
        }
        else
        {
            //Username too short so show error
            Register_ErrorText.text = "Error: Username too Short";
        }*/
    }
    public void PlayerStats_StatsButtonPressed()
    {
        //Called when the player asks to see his stats

        StartCoroutine(GetData());
    }
    public void AddDeathAndRestart()
    {
        StartCoroutine(AddDeathCount());
    }

    public void Register_BackButtonPressed ()
    {
        //Called when the player presses the 'Back' button on the register UI. Switches back to the Login UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(true);
        registerParent.gameObject.SetActive(false);
    }
    public void LoggedIn_SaveDataButtonPressed ()
    {
        //Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server
        loadingParent.gameObject.SetActive(true);
        loggedInParent.gameObject.SetActive(false);
        StartCoroutine(SetData(LoggedIn_DataInputField.text));
    }
    public void LoggedIn_LoadDataButtonPressed ()
    {
        //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server
        /*loadingParent.gameObject.SetActive(true);
        loggedInParent.gameObject.SetActive(false);*/
        StartCoroutine(GetData());
    }
    public void LoggedIn_LogoutButtonPressed ()
    {
        //Called when the player hits the 'Logout' button. Switches back to Login UI and forgets the player's username and password.
        //Note: Database Control doesn't use sessions, so no request to the server is needed here to end a session.
        ResetAllUIElements();
        playerUsername = "";
        playerPassword = "";
        loginParent.gameObject.SetActive(true);
        loggedInParent.gameObject.SetActive(false);
    }
}
