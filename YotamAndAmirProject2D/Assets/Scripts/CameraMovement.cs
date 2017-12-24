using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [Header("Properties")]
    public string targetName1;
    //public string targetName2;


    [SerializeField]
    private float xMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float yMin;

    private bool playerChange = false;
    private Transform target;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.Find(targetName1).transform;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //if(Input.GetKeyDown(KeyCode.Y))
        //{
        //    if (!playerChange)
        //    {
        //        playerChange = true;
        //        target = GameObject.Find(targetName2).transform;
        //    }
        //    else
        //    {
        //        playerChange = false;
        //        target = GameObject.Find(targetName1).transform;
        //    }
        //}
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
	}
}
