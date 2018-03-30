using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCursor : MonoBehaviour {

    private Camera mainCam;
    private Transform objTransform;
    private Rigidbody2D rb;


    private void Awake()
    {
        mainCam = Camera.main;
        objTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // Distance from camera to object.  We need this to get the proper calculation.
        float camDis = mainCam.transform.position.y - objTransform.position.y;

        // Get the mouse position in world space. Using camDis for the Z axis.
        Vector3 mouse = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

        float AngleRad = Mathf.Atan2(Input.mousePosition.y - objTransform.position.y, Input.mousePosition.x - objTransform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;

        rb.rotation = angle;
    }
}


