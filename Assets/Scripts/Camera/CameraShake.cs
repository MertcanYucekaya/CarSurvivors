using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CameraSc cameraSc;

    public IEnumerator ShakeAnimation(float duration, float magnitude)
    {
        cameraSc.isFollow=false;
        float elapsedTime = 0f;
        Vector3 target = transform.position;
        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(target.x + x, target.y + y, target.z);
            elapsedTime += Time.deltaTime;

            yield return null;
            if(Time.timeScale == 0)
            {
                cameraSc.isFollow = true;
                yield break;
            }
        }
        cameraSc.isFollow = true;
    }

    public void StartShake(float _duration, float _magnitude)
    {
        StartCoroutine(ShakeAnimation(_duration, _magnitude));
    }
}
