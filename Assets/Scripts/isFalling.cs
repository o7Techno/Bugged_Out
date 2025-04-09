using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isFalling : MonoBehaviour
{
    Vector3 position;
    bool first_flag = true;
    bool flag = true;
    void Update()
    {
        if (first_flag)
        {
            first_flag = false;
            StartCoroutine(GetPosition());
        }
        if (flag)
        {
            StartCoroutine(Falling());

        }
    }

    IEnumerator GetPosition()
    {
        yield return new WaitForSeconds(1f);
        position = transform.position;
    }

    IEnumerator Falling()
    {
        yield return null;
        if ((transform.position - position).magnitude > 5)
        {
            WorldData.loaded = true;
            flag = false;
        }
    }
}
