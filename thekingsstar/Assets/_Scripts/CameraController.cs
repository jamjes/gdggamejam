using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool lerping = false;
    float lerpDuration = .5f;
    float timerRef = 0;
    Vector3 startPosition, endPosition;
    int reference = 0;

    private void OnEnable()
    {
        PlayerController.OnMoveEnd += LerpCamera;
    }

    private void OnDisable()
    {
        PlayerController.OnMoveEnd -= LerpCamera;
    }

    private void LerpCamera()
    {
        lerping = true;
        startPosition = transform.position;
        reference += 17;
        Debug.Log("lerp set to true, moving camera to = " + reference);
        endPosition = new Vector3(reference, startPosition.y, startPosition.z);
        Debug.Log("end position x = " + endPosition.x);
        
    }

    private void Update()
    {
        if (!lerping)
        {
            return;
        }

        timerRef += Time.deltaTime;
        float percentageComplete = lerpDuration * timerRef;

        if (percentageComplete > 1)
        {
            percentageComplete = 1;
        }

        transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);

        if (percentageComplete == 1)
        {
            lerping = false;
            timerRef = 0;

            Debug.Log("lerp complete, camera is at = " + transform.position.x);
        }
    }
}
