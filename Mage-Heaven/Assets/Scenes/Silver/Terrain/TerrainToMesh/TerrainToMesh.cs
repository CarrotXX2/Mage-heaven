using UnityEngine;
using System.IO;

public class AutoTerrainToMesh : MonoBehaviour
{
    public string assetName = "AutoTerrainMesh"; // Je kan dit aanpassen

    void Start()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogWarning("Geen actieve terrain gevonden!");
            return;
        }

        TerrainData td = terrain.terrainData;
        int res = td.heightmapResolution;

        Vector3[] vertices = new Vector3[res * res];
        int[] triangles = new int[(res - 1) * (res - 1) * 6];

        // Zet vertices
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                float height = td.GetHeight(x, y);
                vertices[y * res + x] = new Vector3(x, height, y);
            }
        }

        // Zet triangles
        int t = 0;
        for (int y = 0; y < res - 1; y++)
        {
            for (int x = 0; x < res - 1; x++)
            {
                int i = y * res + x;
                triangles[t++] = i;
                triangles[t++] = i + res;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + res;
                triangles[t++] = i + res + 1;
            }
        }

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles
        };
        mesh.RecalculateNormals();

        GameObject meshObj = new GameObject("TerrainMesh", typeof(MeshFilter), typeof(MeshRenderer));
        meshObj.GetComponent<MeshFilter>().mesh = mesh;
        meshObj.transform.position = terrain.transform.position;

#if UNITY_EDITOR
        string path = $"Assets/{assetName}.asset";
        UnityEditor.AssetDatabase.CreateAsset(mesh, path);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Mesh opgeslagen als: {path}");
#endif
    }
}
