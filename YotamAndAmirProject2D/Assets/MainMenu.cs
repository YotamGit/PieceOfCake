using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer auidoMixer;

    public TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullScreen;


    public TMP_Dropdown qualityDropdown;

    Resolution[] resolutions;
    
    public static MainMenu Instance;

    public LobbyCanvas LobbyCanvas;
    //public LobbyCanvas LobbyCanvas
    //{
    //    get { return _lobbyCanvas; }
    //}

    [SerializeField]
    private WaitingRoomCanvas WaitingRoomCanvas;
    //public WaitingRoomCanvas WaitingRoomCanvas
    //{
    //    get { return _waitingRoomCanvas; }
    //}

    private void Awake()
    {
        Instance = this;

        // setting the graphics settings according to the graphics that the player enters:

        qualityDropdown.value = QualitySettings.GetQualityLevel();


        // getting resolutions and placing them in the resolution dropdown:

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); // clearing current res options in dropdown

        List<string> options = new List<string>(); // list that holds all the option resolutions

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) // placing res in the list
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); // adding the resolution options to the list
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    private void Start()
    {
        fullScreen.isOn = Screen.fullScreen; // updating the fullscreen button if the player started the game with fullscreen or not
        //SetFullscreen(Screen.fullScreen);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        auidoMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}