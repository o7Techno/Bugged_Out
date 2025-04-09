using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public Transform playerPosition;
    public float spawnRange;
    public LayerMask ground;
    public TerrainData terrainData;

    public void SpawnMob(GameObject mob)
    {
        float xCoord = Random.Range(-spawnRange, spawnRange) + playerPosition.position.x;
        float zCoord = Random.Range(-spawnRange,spawnRange) + playerPosition.position.z;
        Spawner.Spawn(mob, xCoord, zCoord, ground, true, terrainData.uniformScale, playerPosition);
    }
}
