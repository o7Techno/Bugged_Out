using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail, bool useFlatShading)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
        int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int borderedSize = heightMap.GetLength(0);
        int meshSize = borderedSize - 2 * meshSimplificationIncrement;
        int meshSizeUnsimplified = borderedSize - 2;

        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, useFlatShading);
        int[,] vertexIndiciesMap = new int[borderedSize, borderedSize];
        int meshVertexIndex = 0;
        int borderVertexIndex = -1;
        for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
            {
                bool isBorderVertex = y==0 || y == borderedSize - 1 || x ==0 || x==borderedSize -1;
                if (isBorderVertex)
                {
                    vertexIndiciesMap[x, y] = borderVertexIndex;
                    borderVertexIndex--;
                }
                else
                {
                    vertexIndiciesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
        }
        for (int y = 0; y < borderedSize; y+=meshSimplificationIncrement)
        {
            for (int x = 0; x < borderedSize; x+=meshSimplificationIncrement)
            {
                int vertexIndex = vertexIndiciesMap[x, y];
                Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float)meshSize, (y - meshSimplificationIncrement) / (float)meshSize);
                float height = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y* meshSizeUnsimplified);

                meshData.AddVertex(vertexPosition, percent, vertexIndex);


                if (x < borderedSize - 1 && y < borderedSize - 1)
                {
                    int a = vertexIndiciesMap[x, y];
                    int b = vertexIndiciesMap[x + meshSimplificationIncrement, y];
                    int c = vertexIndiciesMap[x, y + meshSimplificationIncrement];
                    int d = vertexIndiciesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];
                    meshData.AddTriangle(a, d, c);
                    meshData.AddTriangle(d, a, b);
                }
                vertexIndex++;
            }
        }
        meshData.Finalise();
        return meshData;
    }
}

public class MeshData
{
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] bakedNormals;
    int triangleIndex;

    Vector3[] borderVerticies;
    int[] borderTriangles;
    int borderTriangleIndex;
    bool useFlatShading;
    public MeshData(int verticiesPerLine, bool useFlatShading)
    {
        vertices = new Vector3[verticiesPerLine * verticiesPerLine];
        uvs = new Vector2[verticiesPerLine * verticiesPerLine];
        triangles = new int[(verticiesPerLine - 1) * (verticiesPerLine - 1) * 6];

        borderVerticies = new Vector3[verticiesPerLine * 4 + 4];
        borderTriangles = new int[24 * verticiesPerLine];
        this.useFlatShading = useFlatShading;
    }

    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        if (vertexIndex < 0)
        {
            borderVerticies[-vertexIndex - 1] = vertexPosition;
        }
        else
        {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        if (a < 0 || b <0 || c<0)
        {
            borderTriangles[borderTriangleIndex] = a;
            borderTriangles[borderTriangleIndex + 1] = b;
            borderTriangles[borderTriangleIndex + 2] = c;
            borderTriangleIndex += 3;

        } else {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }
    }

    Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndicies(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        int borderTriangleCount = borderTriangles.Length / 3;
        for (int i = 0; i < borderTriangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = borderTriangles[normalTriangleIndex];
            int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
            int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndicies(vertexIndexA, vertexIndexB, vertexIndexC);
            if (vertexIndexA >= 0)
            {
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0)
            {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0)
            {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }
        for (int i = 0;i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }
        return vertexNormals;
    }

    Vector3 SurfaceNormalFromIndicies(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = (indexA < 0) ? borderVerticies[-indexA - 1] : vertices[indexA];
        Vector3 pointB = (indexB < 0) ? borderVerticies[-indexB - 1] : vertices[indexB];
        Vector3 pointC = (indexC < 0) ? borderVerticies[-indexC - 1] : vertices[indexC];
        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }
    public void Finalise()
    {
        if (useFlatShading)
        {
            FlatShading();
        }
        else
        {
            BakeNormals();
        }
    }

    void BakeNormals()
    {
        bakedNormals = CalculateNormals();
    }

    void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedUvs[i] = uvs[triangles[i]];
            flatShadedVertices[i] = vertices[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlatShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.normals = bakedNormals;
        }
        return mesh;
    }
}
