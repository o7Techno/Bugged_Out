using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTime : MonoBehaviour
{
    public float despawnTime = 5f;
    void Start()
    {
        Invoke(nameof(Despawn), despawnTime);   
    }

    void Despawn()
    {
        GameObject.Destroy(gameObject);
    }

}
