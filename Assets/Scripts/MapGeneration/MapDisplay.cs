using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    
    public void DrawTexture(Texture2D texture)
    {

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;
        meshFilter.gameObject.layer = 6;

    }
}
