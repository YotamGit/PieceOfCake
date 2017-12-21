using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandle : MonoBehaviour {

    [SerializeField]
    private GameObject[] doors;

    private bool isPressed;

    private SpriteRenderer buttonSpriteRend;

    public Sprite turnedOff;
    public Sprite turnedOn;

    public AudioClip clicked;
    public AudioClip released;

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

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cube" || col.gameObject.tag == "Player")
        {
            if (!isPressed)
            {
                SoundManager.instance.efxSource.volume = 0.7f;
                SoundManager.instance.PlayEffect(clicked);
                isPressed = true;
                PressOrReleasHandle(isPressed);
                buttonSpriteRend.sprite = turnedOn;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cube" || col.gameObject.tag == "Player")
        {
            if (isPressed)
            {
                SoundManager.instance.efxSource.volume = 0.7f;
                SoundManager.instance.PlayEffect(released);
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
