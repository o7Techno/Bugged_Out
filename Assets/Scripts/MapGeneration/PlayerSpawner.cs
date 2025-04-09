using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    LayerMask ground;
    [SerializeField]
    Transform player;
    public void SpawnPlayer()
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(0, 600, 0), Vector3.down, out hit, 600f, ground);
        player.position = hit.point + Vector3.up * 20;
    }

    public void EnablePlayer()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
    }
}
