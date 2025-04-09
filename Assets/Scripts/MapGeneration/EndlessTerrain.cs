//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class EndlessTerrain : MonoBehaviour
//{
//    const float viewerMoveThresholdForChunkUpdate = 25f;
//    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

//    public TreeSpawner treeSpawner;

//    public LODInfo[] detailLevels;
//    public static float maxViewDst;
//    public Transform viewer;
//    public Material mapMaterial;

//    static MapGenerator mapGenerator;

//    public static Vector2 viewerPosition;
//    Vector2 viewerPositionOld;
//    int chunkSize;
//    int chunksVisibleInViewDst;

//    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
//    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

//    private void Start()
//    {
//        treeSpawner = GetComponent<TreeSpawner>();
//        mapGenerator = FindObjectOfType<MapGenerator>();
//        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshhold;
//        chunkSize = mapGenerator.mapChunkSize - 1;
//        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
//        UpdateVisibleChunks();

//    }

//    private void Update()
//    {
//        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

//        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
//        {
//            viewerPositionOld = viewerPosition;
//            UpdateVisibleChunks();
//        }
//    }
//    void UpdateVisibleChunks()
//    {
//        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
//        {
//            terrainChunksVisibleLastUpdate[i].SetVisible(false);
//        }
//        terrainChunksVisibleLastUpdate.Clear();
//        int currentChunkCoordX = Mathf.RoundToInt(viewer.position.x / chunkSize);
//        int currentChunkCoordY = Mathf.RoundToInt(viewer.position.z / chunkSize);
//        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
//        {
//            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
//            {
//                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
//                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
//                {
//                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
//                }
//                else
//                {
//                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial, treeSpawner));
//                }
//            }
//        }
//    }

//    public class TerrainChunk
//    {
//        GameObject meshObject;
//        Vector2 position;
//        Bounds bounds;
//        int previousLODIndex = -1;

//        MapGenerator.MapData mapData;
//        MeshRenderer meshRenderer;
//        MeshFilter meshFilter;
//        MeshCollider meshCollider;
//        LODInfo[] detailLevels;
//        LODMesh[] lodMeshes;
//        LODMesh collisionLODMesh;
//        bool mapDataReceived;
//        TreeSpawner treeSpawner;
//        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material, TreeSpawner treeSpawner)
//        {
//            this.detailLevels = detailLevels;
//            position = coord * size;
//            bounds = new Bounds(position, Vector2.one * size);
//            Vector3 positionV3 = new Vector3(position.x, 0, position.y);
//            this.treeSpawner = treeSpawner;

//            meshObject = new GameObject("Terrain Chunk");
//            meshRenderer = meshObject.AddComponent<MeshRenderer>();
//            meshFilter = meshObject.AddComponent<MeshFilter>();
//            meshCollider = meshObject.AddComponent<MeshCollider>();

//            meshRenderer.material = material;
//            meshObject.transform.position = positionV3 * mapGenerator.terrainData.uniformScale;
//            meshObject.transform.parent = parent;
//            meshObject.layer = 6;
//            meshObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;
//            SetVisible(false);

//            lodMeshes = new LODMesh[detailLevels.Length];
//            for (int i = 0; i < detailLevels.Length; i++)
//            {
//                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
//                if (detailLevels[i].useForCollider)
//                {
//                    collisionLODMesh = lodMeshes[i];
//                }
//            }
//            mapGenerator.RequestMapData(position, OnMapDataReceived);


//        }

//        void OnMapDataReceived(MapGenerator.MapData mapData)
//        {
//            this.mapData = mapData;
//            mapDataReceived = true;
//            UpdateTerrainChunk();
//        }


//        public void UpdateTerrainChunk()
//        {
//            if (mapDataReceived)
//            {
//                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
//                bool visible = viewerDstFromNearestEdge <= maxViewDst;

//                if (visible)
//                {
//                    int lodIndex = 0;
//                    for (int i = 0; i < detailLevels.Length - 1; i++)
//                    {
//                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshhold)
//                        {
//                            lodIndex = i + 1;
//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }
//                    if (lodIndex != previousLODIndex)
//                    {
//                        LODMesh lodMesh = lodMeshes[lodIndex];
//                        if (lodMesh.hasMesh)
//                        {
//                            previousLODIndex = lodIndex;
//                            meshFilter.mesh = lodMesh.mesh;


//                        }
//                        else if (!lodMesh.hasRequestedMesh)
//                        {
//                            lodMesh.RequestMesh(mapData);
//                        }
//                    }
//                    if (lodIndex == 0)
//                    {
//                        if (collisionLODMesh.hasMesh)
//                        {
//                            meshCollider.sharedMesh = collisionLODMesh.mesh;
//                        }
//                        else if (!collisionLODMesh.hasRequestedMesh)
//                        {
//                            collisionLODMesh.RequestMesh(mapData);
//                        }
//                    }
//                    terrainChunksVisibleLastUpdate.Add(this);
//                }
//                SetVisible(visible);
//                treeSpawner.GenerateItemToSpawn();
//            }
//        }

//        public void SetVisible(bool visible)
//        {
//            meshObject.SetActive(visible);
//        }

//        public bool IsVisible()
//        {
//            return meshObject.activeSelf;
//        }
//    }

//    class LODMesh
//    {
//        public Mesh mesh;
//        public bool hasRequestedMesh;
//        public bool hasMesh;
//        int lod;
//        System.Action updateCallback;

//        public LODMesh(int lod, System.Action updateCallback)
//        {
//            this.lod = lod;
//            this.updateCallback = updateCallback;
//        }

//        void OnMeshDataRecieved(MeshData meshData)
//        {
//            mesh = meshData.CreateMesh();
//            hasMesh = true;

//            updateCallback();
//        }

//        public void RequestMesh(MapGenerator.MapData mapData)
//        {
//            hasRequestedMesh = true;
//            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
//        }
//    }


//    [System.Serializable]
//    public struct LODInfo
//    {
//        public int lod;
//        public float visibleDstThreshhold;
//        public bool useForCollider;
//    }
//}
