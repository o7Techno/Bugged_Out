using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.Collections;
using Unity.Jobs;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{

    
    public bool autoUpdate;
    [Range(0, 6)]
    public int editorPreviewLOD;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;
    public MapData mapData;
    public Material terrainMaterial;


    private void Awake()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

    }

    private void Start()
    {
        DrawMap();
    }

    public int mapChunkSize
    {
        get {
            if (terrainData.useFlatShading)
            {
                return 95;
            }
            return 239;
        }
    }

    public enum DrawMode
    {
        NoiseMap,
        Mesh,
        Falloff
    }
    public DrawMode drawMode;


    float[,] falloffMap;


    void OnValuesUpdated()
    {
        DrawMap();
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);

    }


    public void DrawMap()
    {
        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
        mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        ObjectSpawner treeSpawner = FindObjectOfType<ObjectSpawner>();
        SmallObjectSpawner smallObjectSpawner = FindObjectOfType<SmallObjectSpawner>();
        PlayerSpawner playerSpawner = FindObjectOfType<PlayerSpawner>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);
            display.DrawMesh(meshData);
            treeSpawner.GeneratePositionAndSpawn();
            smallObjectSpawner.GeneratePositionAndSpawn();
            GameObject.FindAnyObjectByType<NavMeshSurface>().BuildNavMesh();
            playerSpawner.SpawnPlayer();
            playerSpawner.EnablePlayer();
            //WorldData.loaded = true;
        }
        else if (drawMode == DrawMode.Falloff)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));

        }
    }


    MapData GenerateMapData(Vector2 centre)
    {
        //float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, WorldData.seed, 35.16f, 5, 0.45f, 2, centre, Noise.NormalizeMode.Local);
        if (terrainData.useFalloff)
        {
            if (falloffMap == null)
            {
                falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize + 2);
            }
            for (int y = 0; y < mapChunkSize + 2; y++)
            {
                for (int x = 0; x < mapChunkSize + 2; x++)
                {
                    if (terrainData.useFalloff)
                    {
                        noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                    }
                }
            }
        }

        return new MapData(noiseMap);
    }

    private void OnValidate()
    {
        if (terrainData != null)
        {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }
        if (noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

  

    public struct MapData
    {
        public readonly float[,] heightMap;

        public MapData(float[,] heightMap)
        {
            this.heightMap = heightMap;
        }
    }


  
}
