using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class chunkData{

    public GameObject chunk;
    public Vector3 chunkCorner;
    Vector2 key;
    int size = 16;


    int[][][] block;

    public int GetBlockID(Vector3 position)
    {
        return block[(int)position.x][(int)position.y][(int)position.z];
    }

    public int[][][] GetBlockArray()
    {
        return block;
    }


    public void RecalculateMeshing()
    {
        meshCombiner.BeginMeshing(key);
    }

    MeshCombiner meshCombiner;

	public chunkData (Vector2 chunkKey, int width, GameObject parent) {

        chunk = parent;
        size = width;
        key = chunkKey;
        block = new int[size][][];

        chunkCorner = chunk.transform.position - new Vector3(size / 2 - 0.5f, -0.5f, size / 2 - 0.5f);
        GenerateList();

        meshCombiner = chunk.AddComponent<MeshCombiner>();
        meshCombiner.BeginMeshing(key);
    }


    void GenerateList()
    {
        Dictionary<Vector2, int> heights = new Dictionary<Vector2, int>();
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                heights.Add(new Vector2(x, z), heightMapGenerator.heightMap.GenerateHeights(x, z, key, chunkCorner));
                //Debug.Log(heights[new Vector2(x, z)]);
            }
        }


        int height = 10;
        for (int x = 0; x < block.Length; x++)
        {
            block[x] = new int[worldData.world.chunkHeight][];
            for (int y = 0; y < block[x].Length; y++)
            {
                block[x][y] = new int[size];
                for (int z = 0; z < block[x][y].Length; z++)
                {
                    height = heights[new Vector2(x, z)];
                    //Here goes world generation

                    //int height = 10;



                    //TEMPORARY
                    if(height > 40 && y <= height)
                    {
                        block[x][y][z] = textureCoords.Stone;
                    }

                    else if (y == height)
                    {
                        //Grass
                        block[x][y][z] = 1;
                    }
                    else if (y < height && y > height - 3)
                    {
                        //Dirt
                        block[x][y][z] = 2;
                    }
                    else if (y <= height - 3)
                    {
                        //Stone
                        block[x][y][z] = 3;
                    }
                    else
                    {
                        //Air
                        block[x][y][z] = 0;
                        //Water
                        if (y < 25) block[x][y][z] = textureCoords.Water;
                    }
                    if (height < 27 && block[x][y][z] != textureCoords.Water && y > height - 2 && block[x][y][z] != 0)
                    {
                        block[x][y][z] = textureCoords.Sand;
                    }
                }
            }
        }
    }


    public Vector3 GetBlockLocationWorld(int x, int y, int z)
    {
        return chunkCorner + new Vector3(x, y, z);
    }



    public Vector2 GetKey()
    {
        return key;
    }


}
