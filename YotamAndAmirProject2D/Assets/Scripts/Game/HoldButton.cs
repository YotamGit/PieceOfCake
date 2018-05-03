using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoldButton : MonoBehaviour
{
    //[HideInInspector]
    private bool isPressed = false;

    //[HideInInspector]
    private float current;
    
    public Slider slider;

    [Range(0,1)]
    public float speed;

    [SerializeField]
    private bool toMenu;
	
    /*
     advancing the value of the slider until it reaches the end and then loading the menu or exiting the application
     */
	private void FixedUpdate ()
    {

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3)
            || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Escape)) || Time.timeScale == 0)
        {
            // setting the values of the exit buttons to 0
            current = 0;
            slider.value = 0;
            isPressed = false;
        }
        else if (isPressed)
        {
            current += speed;
            slider.value = current;

            if (current >= 1)
            {
                if (toMenu) // telling all the players in the room to leave the room and return to the lobby
                {
                    LoadSceneAndLeaveRoom();
                }
                else
                {
                    Application.Quit();
                    Debug.Log("Quiting...");
                }
            }
        }
    }

    /*
     canceling the progression of the slider if the player pressed any of the keys specified below
     */
  

    private bool PressedOnLeaveRoomButton = false;

    //leaving the room and loading the main menu after you left
    public void LoadSceneAndLeaveRoom()
    {
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        if(!PhotonNetwork.inRoom)
            SceneManager.LoadScene(0);//loading the main menu scene
        PressedOnLeaveRoomButton = true;
    }

    private void OnLeftRoom() // this function is called by photon when you leave the room
    {
        if(PressedOnLeaveRoomButton)
            SceneManager.LoadScene(0);//loading the main menu scene
    }

    public void OnPressed()
    {
        isPressed = true;
        current = 0f;
        slider.value = 0f;
    }

    public void OnReleased()
    {
        isPressed = false;
        current = 0f;
        slider.value = 0f;
    }
}
