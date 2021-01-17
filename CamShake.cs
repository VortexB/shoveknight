using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public IEnumerator Shake(float time,float str)
    {
        Vector3 orgPos = transform.position;
        float timeLeft = 0;
        while (timeLeft<time)
        {
            transform.position = new Vector3(orgPos.x+ Random.Range(-0.5f, 0.5f)*str, orgPos.y + Random.Range(-0.5f, 0.5f) * str, orgPos.z);
            timeLeft += Time.deltaTime;
            yield return null;
        }
        transform.position = orgPos;
    }
}
