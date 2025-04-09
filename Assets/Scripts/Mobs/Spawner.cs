using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public static class Spawner
{
    public static void Spawn(GameObject gameObject, float xCoordinate, float zCoordinate, LayerMask ground, bool isMob, float uniformScale, Transform parent)
    {
        RaycastHit hit;
        Vector3 shooter = new Vector3(xCoordinate, 300f, zCoordinate);
        bool success = true;
        if (!Physics.Raycast(shooter, Vector3.down, out hit, 300f, ground))
        {
            success = Physics.Raycast(shooter, Vector3.down, out hit, 300f, ground);
        }
        float yRotation = UnityEngine.Random.Range(0f, 360f);
        NavMeshHit navMeshHit;
        if (hit.point.y > 4.5 * uniformScale || hit.point.y < 2 * uniformScale)
        {
            return;
        }
        if (isMob && NavMesh.SamplePosition(hit.point, out navMeshHit, 1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.eulerAngles.x, yRotation, gameObject.transform.rotation.eulerAngles.z));
            GameObject.Instantiate(gameObject, navMeshHit.position, rotation);
            return;
        }
        else if (!Physics.CheckSphere(hit.point + Vector3.up * 3, 5f, LayerMask.GetMask("Environment")) && !isMob)
        {
            float xRotation = UnityEngine.Random.Range(0f, 11f);
            float zRotation = UnityEngine.Random.Range(0f, 11f);
            Quaternion rotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
            GameObject instantiated = GameObject.Instantiate(gameObject, hit.point - new Vector3(0, 0.5f, 0), rotation);
            instantiated.transform.SetParent(parent);
        }
    }

}
