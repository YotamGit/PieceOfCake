using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandle : MonoBehaviour {

    [SerializeField]
    private GameObject[] doors;

    [SerializeField]
    private float offsetX;

    [SerializeField]
    private float offsetY;

    //private Transform objectTransform;

    /*[SerializeField]
    private float cooldown;

    private float nextPressAllowed;*/

    private Collider2D objectCollider2D;

    // Use this for initialization
    void Start () {
        objectCollider2D = GetComponent<Collider2D>();
        //objectTransform = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (Time.time > nextPressAllowed)
        {*/
            PressOrReleasHandle(true);
            /*StartCoroutine("WaitaBit");
            nextPressAllowed = Time.time + cooldown;
        }*/
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        PressOrReleasHandle(false);
        //nextPressAllowed = Time.time;
    }

    /*IEnumerator WaitaBit()
    {
        yield return new WaitForSecondsRealtime(4);
    }*/

    private void PressOrReleasHandle(bool pressed) // pressed (to new pos) == true, back to first pos == false
    {
        for (int i = 0; i < doors.Length; i++)
        {
            Collider2D doorCol = doors[i].GetComponent<Collider2D>();
            Material doorMaterial = doors[i].GetComponent<Renderer>().material;
            ChangeDoorState(pressed, doorCol, doorMaterial);
        }

        if(pressed)
        {
            objectCollider2D.offset = new Vector2(objectCollider2D.offset.x + offsetX, objectCollider2D.offset.y + offsetY);
        }
        else
        {
            objectCollider2D.offset = new Vector2(objectCollider2D.offset.x + offsetX, objectCollider2D.offset.y - offsetY);
        }
    }

    private void ChangeDoorState(bool pressed, Collider2D doorCol, Material doorMaterial)
    {
        if (pressed) // button pressed
        {
            //objectTransform.position = new Vector3(objectTransform.position.x - changeX, objectTransform.position.y - changeY, objectTransform.position.z);
            doorCol.offset = new Vector2(doorCol.offset.x + offsetX, doorCol.offset.y + offsetY);
            doorMaterial.color = new Color(1, 1, 1, 0.4F);
            doorCol.isTrigger = !doorCol.isTrigger;
        }
        else // button released
        {
            //objectTransform.position = new Vector3(objectTransform.position.x + changeX, objectTransform.position.y + changeY, objectTransform.position.z);
            doorCol.offset = new Vector2(doorCol.offset.x + offsetX, doorCol.offset.y - offsetY);
            doorMaterial.color = new Color(1, 1, 1, 1);
            doorCol.isTrigger = !doorCol.isTrigger;
        }
    }
}
