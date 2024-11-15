using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool isShaking = false;
    public float duration = 1f;
    public AnimationCurve curve;

    void OnEnable()
    {
        Player.OnParry += Shake;
        Enemy.OnShoot += Shake;
    }

    void OnDisable()
    {
        Player.OnParry -= Shake;
        Enemy.OnShoot -= Shake;
    }

    private void Shake()
    {
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
