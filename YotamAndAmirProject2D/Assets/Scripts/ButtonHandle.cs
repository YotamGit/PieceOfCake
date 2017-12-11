using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandle : MonoBehaviour {

    [SerializeField]
    private string doorTag;
    //public int numOfDoor;

    [SerializeField]
    private float changeX;

    [SerializeField]
    private float changeY;

    private GameObject door;

    private Transform objectTransform;

    // Use this for initialization
    void Start () {
        objectTransform = gameObject.transform;
        door = GameObject.FindGameObjectsWithTag(doorTag)[0];
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        PressOrReleasHandle(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        PressOrReleasHandle(false);
    }

    private void PressOrReleasHandle(bool pressed) // pressed (to new pos) == true, back to first pos == false
    {
        Collider2D doorCol = door.GetComponent<Collider2D>();
        Material doorMaterial = door.GetComponent<Renderer>().material;

        if (pressed) // button pressed
        {
            objectTransform.position = new Vector3(objectTransform.position.x - changeX, objectTransform.position.y - changeY, objectTransform.position.z);
            doorMaterial.color = new Color(1, 1, 1, 0.4F);
            doorCol.isTrigger = !doorCol.isTrigger;
        }
        else // button released
        {
            objectTransform.position = new Vector3(objectTransform.position.x + changeX, objectTransform.position.y + changeY, objectTransform.position.z);
            doorMaterial.color = new Color(1, 1, 1, 1);
            doorCol.isTrigger = !doorCol.isTrigger;
        }
    }
}
