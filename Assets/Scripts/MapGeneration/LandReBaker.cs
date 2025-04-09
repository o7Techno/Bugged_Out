using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LandReBaker : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

}
