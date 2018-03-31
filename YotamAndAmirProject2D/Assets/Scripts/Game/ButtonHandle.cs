using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandle : Photon.MonoBehaviour , IPunObservable
{
    [SerializeField]
    private GameObject[] doors;

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

    void Start ()
    {
        buttonSpriteRend = gameObject.GetComponent<SpriteRenderer>();//used to change the sprite of the button
        isPressed = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}//used for syncing

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")//activating the button
        {
            if (!isPressed)
            { 
                source.volume = 0.7f;
                SoundManager.instance.PlayEffect(source, clicked);

                isPressed = true;
                PressOrReleasHandle(isPressed);//opening the door
                buttonSpriteRend.sprite = turnedOn;//changing the sprite
            }
        }
    }

 
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")//deactivating the button
        {
            if (isPressed)
            {
                source.volume = 0.7f;
                SoundManager.instance.PlayEffect(source,  released);

                isPressed = false;
                PressOrReleasHandle(isPressed);//closing the door
                buttonSpriteRend.sprite = turnedOff;//changing the button sprite
            }
        }
    }
    
    private void PressOrReleasHandle(bool pressed) // pressed (to new state) == true, back to first state == false
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (pressed) // button pressed
            {
                doors[i].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.4F);
            }
            else // button released
            {
                doors[i].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            }
            doors[i].GetComponent<Collider2D>().enabled = !doors[i].GetComponent<Collider2D>().enabled;
        }
    }
}
