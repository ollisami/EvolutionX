using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{

    public Block[, ,] blocks = new Block[chunkSize, chunkSize, chunkSize];

    public static int chunkSize = 16;
    public bool update = false;
    public bool rendered;

    MeshFilter filter;
    MeshCollider coll;

    public World world;
    public WorldPos pos;

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

    private List<Vector3>SmoothMesh (List<Vector3> meshData)
    {
        //Ei toimi, koska pitäis jotenki pystyy päivittää smoothatut naapurit listaan? plus lopputulos olis varmaa sama ku 
        //alkuperäses smoothi skriptis, eli ei hyvä...
        List<Vector3> smoothed = new List<Vector3>();
        HashSet<Vector3> set = new HashSet<Vector3>();
        foreach (Vector3 vec in meshData)
            set.Add(vec);

        foreach (Vector3 v in meshData)
        {
            List<Vector3> neighbours = new List<Vector3>();

            float totalX = 0;
            float totalY = 0;
            float totalZ = 0;

            Vector3 key = new Vector3(v.x + 1, v.y, v.z);
            if (set.Contains(key))
                neighbours.Add(key);

            key = new Vector3(v.x - 1, v.y, v.z);
            if (set.Contains(key))
            {
                totalX += key.x;
                totalY += key.y;
                totalZ += key.z;

                neighbours.Add(key);
            }

            key = new Vector3(v.x, v.y + 1, v.z);
            if (set.Contains(key))
            {
                totalX += key.x;
                totalY += key.y;
                totalZ += key.z;
                neighbours.Add(key);
            }

            key = new Vector3(v.x, v.y - 1, v.z);
            if (set.Contains(key))
            {
                totalX += key.x;
                totalY += key.y;
                totalZ += key.z;
                neighbours.Add(key);
            }
            
            key = new Vector3(v.x, v.y, v.z + 1);
            if (set.Contains(key))
            {
                totalX += key.x;
                totalY += key.y;
                totalZ += key.z;
                neighbours.Add(key);
            }

            key = new Vector3(v.x, v.y, v.z - 1);
            if (set.Contains(key))
            {
                totalX += key.x;
                totalY += key.y;
                totalZ += key.z;
                neighbours.Add(key);
            }

            if (neighbours.Count > 0)
            {
                Debug.Log("Original: " + v);
                Debug.Log("Smoothed: " + new Vector3(totalX / neighbours.Count, totalY / neighbours.Count, totalZ / neighbours.Count));
                Debug.Log("Count: " + neighbours.Count);
                smoothed.Add(new Vector3(totalX / neighbours.Count, totalY / neighbours.Count, totalZ / neighbours.Count));
            }
            else
            {
                smoothed.Add(v);
            }

        }
        return smoothed;
    }

}