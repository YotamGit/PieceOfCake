using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonHandle : MonoBehaviour {

    [SerializeField]
    private GameObject[] doors;

    private bool isPressed;

    private SpriteRenderer buttonSpriteRend;

    [SerializeField]
    private Sprite turnedOff;
    private Sprite turnedOn;

    /*private List<Collider2D> doorCols;

    private List<Material> doorMaterials;*/

    // Use this for initialization
    void Start () {
        buttonSpriteRend = gameObject.GetComponent<SpriteRenderer>();
        isPressed = false;
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPressed)
        {
            isPressed = true;
            PressOrReleasHandle(isPressed);
            buttonSpriteRend.sprite = turnedOn;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(isPressed)
        {
            isPressed = false;
            PressOrReleasHandle(isPressed);
            buttonSpriteRend.sprite = turnedOff;
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
