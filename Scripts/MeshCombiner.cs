using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using B86.MeshWelder;
using System.Diagnostics;

public class MeshCombiner : MonoBehaviour
{
    //public static MeshCombiner meshCombiner;

    worldData world;

    int dictKeyWater = 0;
    int dictKeyBlocks = 0;
    Dictionary<int, Mesh> blockMeshes = new Dictionary<int, Mesh>();
    Dictionary<int, Mesh> waterMeshes = new Dictionary<int, Mesh>();
                           
    Vector2 chunkKey;


    public void BeginMeshing(Vector2 ChunkKey)
    {
        chunkKey = ChunkKey;
        
        //new Thread(() => GenerateFaces()) { IsBackground = true }.Start();

        StartCoroutine(GenerateFaces());
    }

    int[][][] blockArray;

    IEnumerator GenerateFaces()
    {
        yield return 0;
        world = worldData.world;
        chunk = world.GetChunk(chunkKey);
        chunkCorner = chunk.chunkCorner;
        blockArray = chunk.GetBlockArray();

        Stopwatch watch1 = new Stopwatch();
        watch1.Start();

        for (int x = 0; x < blockArray.Length; x++)
        {
            for (int y = 0; y < blockArray[x].Length; y++)
            {
                for (int z = 0; z < blockArray[x][y].Length; z++)
                {

                    CheckAdjacentCubes(x, y, z);

                }
            }
        }

        watch1.Stop();
        Stopwatch watch2 = new Stopwatch();
        watch2.Start();


        CombineBlocksMeshes(blockMeshes);
        CombineWaterMeshes(waterMeshes);


        watch2.Stop();
        world.addWatchTime(watch1.ElapsedMilliseconds);
        //UnityEngine.Debug.Log("Chunk: " + chunkKey + ", GenerateFaces: " + watch1.ElapsedMilliseconds + "ms, MeshFaces: " + watch2.ElapsedMilliseconds + "ms");

        //Combine();
    }

    heightMapGenerator heightMap;
    Vector3 chunkCorner;
    chunkData chunk;

    void Start()
    {
        heightMap = heightMapGenerator.heightMap;
        texture = textureCoords.Map;
    }



    void CheckAdjacentCubes(int cubeX, int cubeY, int cubeZ)
    {
        Vector3[] directions = new Vector3[6]
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };
        int block = blockArray[cubeX][cubeY][cubeZ];

        //If solid
        if (block != 0)
        {
            if (cubeX == 0)
            {
                if (cubeY > heightMap.GenerateHeights(cubeX - 1, cubeZ, chunkKey, chunkCorner) && block != 4)
                {
                    GenerateFace(cubeX, cubeY, cubeZ, Vector3.right);
                }
                directions[1].x = 0;
            }
            else if (cubeX == world.chunkSize - 1)
            {
                if (cubeY > heightMap.GenerateHeights(cubeX + 1, cubeZ, chunkKey, chunkCorner) && block != 4)
                {
                    GenerateFace(cubeX, cubeY, cubeZ, -Vector3.right);
                }
                directions[0].x = 0;
            }

            if (cubeZ == 0)
            {
                if (cubeY > heightMap.GenerateHeights(cubeX, cubeZ - 1, chunkKey, chunkCorner) && block != 4)
                {
                    GenerateFace(cubeX, cubeY, cubeZ, Vector3.forward);
                }
                directions[5].z = 0;
            }
            else if (cubeZ == world.chunkSize - 1)
            {
                if (cubeY > heightMap.GenerateHeights(cubeX, cubeZ + 1, chunkKey, chunkCorner) && block != 4)
                {
                    GenerateFace(cubeX, cubeY, cubeZ, -Vector3.forward);
                }
                directions[4].z = 0;
            }

            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i] != new Vector3(0, 0, 0))
                {
                    if (cubeY + (int)directions[i].y >= world.chunkHeight)
                    {
                        GenerateFace(cubeX, cubeY, cubeZ, -directions[i]);
                    }
                    else if (cubeY + (int)directions[i].y < 0)
                    {

                    }
                    else {
                        int Adjblock = blockArray[cubeX + (int)directions[i].x][cubeY + (int)directions[i].y][cubeZ + (int)directions[i].z];

                        if (Adjblock == 0)
                        {
                            GenerateFace(cubeX, cubeY, cubeZ, -directions[i]);
                        }
                        else if (Adjblock == 4)
                        {
                            if (block != 4)
                                GenerateFace(cubeX, cubeY, cubeZ, -directions[i]);
                        }
                    }

                }
            }

        }
    }




    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    textureCoords texture;


    //For FaceGeneration
    int[] newTrianglesRegular = new int[6] { 2, 3, 0, 2, 0, 1 };
    int[] newTrianglesNegativ = new int[6] { 0, 3, 2, 1, 0, 2 };
    Vector3[] normals = new Vector3[4];

    void GenerateFace(int x, int y, int z, Vector3 lookDirection)
    {
        
        Vector3 position = new Vector3(x, y, z) - new Vector3(world.chunkSize/2, world.chunkSize / 2, world.chunkSize / 2);

        Mesh mesh = new Mesh();
        
        

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = lookDirection;
        }





        int isNegative = -1;


        if (lookDirection.x < 0 || lookDirection.y < 0 || lookDirection.z < 0)
        {
            isNegative = 1;
        }
        //for (int i = 0; i < newVertices.Length; i++)
        //{
        //    //newVertices[i] += new Vector3(1, 1, 1) * 0.5f * isNegative;
        //}

        Vector3[] newVertices = GetVertices(lookDirection, position);

        //Make water little lower
        if (blockArray[x][y][z] == textureCoords.Water)
        {
            for (int i = 0; i < newVertices.Length; i++)
            {
                newVertices[i] -= new Vector3(0, 0.1f, 0);
            }
        }
        mesh.vertices = newVertices;




        if (isNegative == -1) mesh.triangles = newTrianglesRegular;
        else mesh.triangles = newTrianglesNegativ;

        mesh.normals = normals;



        int blockID = chunk.GetBlockID(new Vector3(x, y, z));
        mesh.uv = texture.getTexture(blockID, lookDirection);

        if(blockID == textureCoords.Water)
        {
            waterMeshes.Add(dictKeyWater, mesh);
            dictKeyWater++;
        }
        else
        {
            blockMeshes.Add(dictKeyBlocks, mesh);
            dictKeyBlocks++;
        } 
    }


    Vector3[] vertices = new Vector3[4];
    Vector3[] verticeOffset = new Vector3[4];

    Vector3[] GetVertices(Vector3 direction, Vector3 position)
    {
        //X 
        if (direction == new Vector3(1, 0, 0) || direction == new Vector3(-1, 0, 0))
        {
            verticeOffset = new Vector3[]{
                new Vector3(0, -0.5f, -0.5f),
                new Vector3(0, -0.5f, +0.5f),
                new Vector3(0, +0.5f, +0.5f),
                new Vector3(0, +0.5f, -0.5f)
            };
        }        
        //Y
        else if (direction == new Vector3(0, 1, 0) || direction == new Vector3(0, -1, 0))
        {
            verticeOffset = new Vector3[]{
                new Vector3(-0.5f, 0, -0.5f),
                new Vector3(+0.5f, 0, -0.5f),
                new Vector3(+0.5f, 0, +0.5f),
                new Vector3(-0.5f, 0, +0.5f)
            };
        }
        //Z
        else
        {
            verticeOffset = new Vector3[]{
                new Vector3(+0.5f, +0.5f, 0),
                new Vector3(+0.5f, -0.5f, 0),
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(-0.5f, +0.5f, 0),
            };
        }



        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = verticeOffset[i] + position + direction * -0.5f;
        }

        return vertices;
    }



    //Dictionary<int, Mesh> meshes
    //(List<Mesh> meshes




    public void CombineWaterMeshes(Dictionary<int, Mesh> meshes)
    {
        GameObject g_chunk = chunk.chunk;
        GameObject g_water = (GameObject)Instantiate(Resources.Load("Water"), g_chunk.transform);
        g_water.transform.position = g_chunk.transform.position;


        CombineInstance[] combine = new CombineInstance[meshes.Count];

        Vector3 position = g_water.transform.position;
        g_water.transform.position = Vector3.zero;

        MeshFilter filter = g_water.GetComponent<MeshFilter>();
        filter.mesh.Clear();



        int i = 0;
        while (i < meshes.Count)
        {
            filter.mesh = meshes[i];
            combine[i].mesh = filter.sharedMesh;
            combine[i].transform = g_water.transform.worldToLocalMatrix;
            i++;
        }

        //Set to chunk

        Mesh mesh = new Mesh();

        mesh.CombineMeshes(combine);

        //MeshWelder welder = new MeshWelder(mesh);
        //mesh = welder.Weld();

        g_water.GetComponent<MeshFilter>().mesh = mesh;
        //g_water.GetComponent<MeshCollider>().sharedMesh = mesh;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        g_water.transform.position = position;
    }


    public void CombineBlocksMeshes(Dictionary<int, Mesh> meshes)
    {
        GameObject g_chunk = chunk.chunk;
        
        CombineInstance[] combine = new CombineInstance[meshes.Count];

        Vector3 position = g_chunk.transform.position;
        g_chunk.transform.position = Vector3.zero;

        MeshFilter filter = g_chunk.GetComponent<MeshFilter>();
        filter.mesh.Clear();



        int i = 0;
        while (i < meshes.Count)
        {


            filter.mesh = meshes[i];
            combine[i].mesh = filter.sharedMesh;
            combine[i].transform = g_chunk.transform.worldToLocalMatrix;
            i++;
        }



        //Set to chunk

        Mesh mesh = new Mesh();

        mesh.CombineMeshes(combine);

        //MeshWelder welder = new MeshWelder(mesh);
        //mesh = welder.Weld();

        g_chunk.GetComponent<MeshFilter>().mesh = mesh;
        g_chunk.GetComponent<MeshCollider>().sharedMesh = mesh;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        g_chunk.transform.position = position;
    }
}