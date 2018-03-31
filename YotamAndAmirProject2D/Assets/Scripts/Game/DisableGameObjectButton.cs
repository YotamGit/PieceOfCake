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
    public AudioSource source;

    void Start()
    {
        buttonSpriteRend = gameObject.GetComponent<SpriteRenderer>();
        isPressed = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")
        {
            if (!isPressed)
            {
                source.volume = 0.7f;
                SoundManager.instance.PlayEffect(source, clicked);

                isPressed = true;
                PressOrReleasHandle(isPressed);//disabling the object
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
                source.volume = 0.7f;
                SoundManager.instance.PlayEffect(source, released);

                isPressed = false;
                PressOrReleasHandle(isPressed);//enabling the object
                buttonSpriteRend.sprite = turnedOff;
            }
        }
    }

    private void PressOrReleasHandle(bool pressed) // pressed (to new state) == true, back to first state == false
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(!gameObjects[i].activeInHierarchy);
        }
    }
}

