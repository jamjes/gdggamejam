using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool lerping = false;
    float lerpDuration = .5f;
    float timerRef = 0;
    Vector3 startPosition, endPosition;

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
        endPosition = startPosition + new Vector3(17, 0);
        
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
        }
    }
}
