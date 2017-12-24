using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandle : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
        buttonSpriteRend = gameObject.GetComponent<SpriteRenderer>();
        isPressed = false;
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cube" || col.gameObject.tag == "Player1" | col.gameObject.tag == "Player2")
        {
            if (!isPressed)
            {
                if(col.gameObject.tag == "Player1")
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
