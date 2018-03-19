using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectButton : Photon.MonoBehaviour, IPunObservable
{
    [SerializeField]
    private GameObject[] gameObjects;

    [Space]

    private bool isPressed;
    private SpriteRenderer buttonSpriteRend;

    [Header("Sprites")]
    public Sprite turnedOff;
    public Sprite turnedOn;

    [Space]

    [Header("Audio")]
    public AudioClip clicked;
    public AudioClip released;

    // Use this for initialization
    void Start()
    {
        buttonSpriteRend = gameObject.GetComponent<SpriteRenderer>();
        isPressed = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")
        {
            if (!isPressed)
            {
                if (col.gameObject.tag == "Player1")
                {
                    SoundManager.instance.efxSource1.volume = 0.7f;
                    SoundManager.instance.PlayEffect1(clicked);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.7f;
                    SoundManager.instance.PlayEffect2(clicked);
                }
                isPressed = true;
                PressOrReleasHandle(isPressed);
                buttonSpriteRend.sprite = turnedOn;
            }
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")
        {
            if (isPressed)
            {
                if (col.gameObject.tag == "Player1")
                {
                    SoundManager.instance.efxSource1.volume = 0.7f;
                    SoundManager.instance.PlayEffect1(released);

                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.7f;
                    SoundManager.instance.PlayEffect2(released);

                }
                isPressed = false;
                PressOrReleasHandle(isPressed);
                buttonSpriteRend.sprite = turnedOff;
            }
        }
    }

    private void PressOrReleasHandle(bool pressed) // pressed (to new pos) == true, back to first pos == false
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(!gameObjects[i].activeInHierarchy);
        }
    }
}

