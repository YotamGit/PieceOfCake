using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoldButton : MonoBehaviour {
    [HideInInspector]
    public bool isPressed;

    [HideInInspector]
    public float current;
    
    public Slider slider;

    [Range(0,1)]
    public float speed;

    [SerializeField]
    private bool toMenu;

	// Use this for initialization
	void Start () {
        isPressed = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (isPressed)
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

    private void Update()
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
    }

    public void LoadSceneAndLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
