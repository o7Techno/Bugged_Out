using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class SmallObjectSpawner : MonoBehaviour
{
    MapGenerator mapGenerator;
    public LayerMask ground;
    public TerrainData terrainData;
    public NoiseData noiseData;
    [SerializeField]
    public ObjectData[] environmentObjects;
    public Transform parent;
    int mapChunkSize;
    float[,] heightMap;

    private void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        mapChunkSize = mapGenerator.mapChunkSize;
    }

    public void GeneratePositionAndSpawn()
    {
        heightMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, WorldData.seed, 35.16f, 5, 0.45f, 2, Vector2.zero, Noise.NormalizeMode.Local);
        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(0); j++)
            {
                List<ObjectData> filteredList = new List<ObjectData>();
                for (int k = 0; k < environmentObjects.Length; k++)
                {
                    if (Random.Range(0, environmentObjects[k].infrequency) == 0)
                    {
                        filteredList.Add(environmentObjects[k]);
                    }
                }
                int itemIndex = Random.Range(0, filteredList.Count);
                if (filteredList.Count > 0)
                {
                    if (filteredList[itemIndex].objectMinHeight <= heightMap[i, j] && filteredList[itemIndex].objectMaxHeight >= heightMap[i, j])
                    {
                        float x = (float)j / heightMap.GetLength(0) * 80 * terrainData.uniformScale - 40 * terrainData.uniformScale + Random.Range(-20f, 20f);
                        float z = (float)i / heightMap.GetLength(0) * 80 * terrainData.uniformScale - 40 * terrainData.uniformScale + Random.Range(-20f, 20f);
                        Spawn(filteredList[itemIndex], x, z);
                    }
                }
            }
        }
    }

    public void Spawn(ObjectData objectData, float xCoordinate, float zCoordinate)
    {
        //Getting point to spawn on.
        RaycastHit hit;
        Vector3 shooter = new Vector3(xCoordinate, 300f, zCoordinate);
        Physics.Raycast(shooter, Vector3.down, out hit, 300f, ground);

        float yRotation = UnityEngine.Random.Range(0f, 360f);

        //Removing points too low.
        if (hit.point.y > 4.5 * terrainData.uniformScale || hit.point.y < 2 * terrainData.uniformScale)
        {
            return;
        }

        //Checking for stuff nearby.
        bool success = true;
        for (int i = 0; i < objectData.distances.Length; i++)
        {
            if (Physics.CheckSphere(hit.point + Vector3.up * objectData.distances[i].height, objectData.distances[i].distance, objectData.distances[i].layer))
            {
                success = false;
                break;
            }
        }

        //Spawning the object.
        if (success)
        {
            Quaternion rotation;
            if (objectData.rotateXZAxis)
            {
                float xRotation = UnityEngine.Random.Range(0f, 11f);
                float zRotation = UnityEngine.Random.Range(0f, 11f);
                rotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
            }
            else
            {
                rotation = Quaternion.Euler(new Vector3(0, yRotation, 0));
            }

            GameObject instantiated = GameObject.Instantiate(objectData.gameObject, hit.point - new Vector3(0, objectData.downOffset, 0), rotation);
            instantiated.transform.SetParent(parent);
        }
    }

    [System.Serializable]
    public class ObjectData
    {
        public int infrequency;
        [Range(0, 1)]
        public float objectMaxHeight;
        [Range(0, 1)]
        public float objectMinHeight;
        public float downOffset;
        public bool rotateXZAxis;
        public GameObject gameObject;
        public LayerDistance[] distances;

        [System.Serializable]
        public class LayerDistance
        {
            public LayerMask layer;
            public float distance;
            public float height;
        }

    }
}