using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovingText : MonoBehaviour
{
    [Range(-1, 1)]
    public float rotateSpeed, rotateMaxRange, waitingTime, smooth, add, timeWaitAtEnd;
    //shrinkSpeed, shrinkMaxRange
    private RectTransform objTransform;

    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        StartCoroutine(RotateText());
        //objTransform.rotation = new Quaternion(0, 0, 5, 0);
    }

    IEnumerator RotateText()
    {
        while (true)
        {

            /*Vector3 desiredPos = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotateSpeed);
            Vector3 calmpedPos = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.z, -30, 30));
            Vector3 smoothedPos = Vector3.Lerp(calmpedPos, desiredPos, 1);
            if(smoothedPos.z > 30 || smoothedPos.z < -30)
            {
                rotateSpeed = -rotateSpeed;
            }
            objTransform.Rotate(smoothedPos);*/

            Vector3 desiredPos = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.z + rotateSpeed, -rotateMaxRange, rotateMaxRange));
            Vector3 fromPos = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            Vector3 finalPos = Vector3.Lerp(fromPos, desiredPos, smooth);
            objTransform.rotation = new Quaternion(finalPos.x, finalPos.y, finalPos.z, objTransform.rotation.w);

            if (objTransform.rotation.z + add >= rotateMaxRange || objTransform.rotation.z - add <= -rotateMaxRange)
            {
                rotateSpeed = -rotateSpeed;
                yield return new WaitForSeconds(timeWaitAtEnd);
            }

            /*Vector3 desiredPos = new Vector3(transform.rotation.x, transform.rotation.y, rotateMaxRange);
            Vector3 fromPos = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            Vector3 finalPos = Vector3.Lerp(fromPos, desiredPos, smooth);
            objTransform.rotation = new Quaternion(finalPos.x, finalPos.y, finalPos.z, objTransform.rotation.w);

            if (objTransform.rotation.z + add >= rotateMaxRange || objTransform.rotation.z - add <= -rotateMaxRange)
            {
                rotateMaxRange = -rotateMaxRange;
                yield return new WaitForSeconds(timeWaitAtEnd);
            }*/

           yield return new WaitForSeconds(waitingTime);
        }
    }

    /*private void RotateText()
    {
        Vector3 desiredPos = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        Vector3 calmpedPos = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.z, -rotateMaxRange, rotateMaxRange));
        Vector3 smoothedPos = Vector3.Lerp(calmpedPos, desiredPos, rotateSpeed);
        transform.rotation = new Quaternion(smoothedPos.x, smoothedPos.y, smoothedPos.z, 0);
    }*/

}
