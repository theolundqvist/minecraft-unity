using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heightMapGenerator : MonoBehaviour
{

    [Header("Update settings")]
    public bool update = true;
    public bool activeUpdate = false;

    [Header("Generation Properties")]
    public short octaves = 3;

    public float lacunarity = 2f; //frequency = lacunarity ^ octave
    float frequency = 1;

    public float frequencyMultiplier = 4f;

    public float persistance = 0.5f; //amplitude = persistance ^ octave
    float amplitude = 5f;

    public float amplitudeMultiplier = 10f;


    [Header("Position Settings")]
    public Vector3 startPosition = new Vector3(0, 0, 0);
    float offsetX;
    float offsetZ;
    float offsetX2;
    float offsetZ2;
    public float offsetY;

    [Header("object HeightMap settings")]
    int objectOffsetZ;
    int objectOffsetX;
    float[][] objectHeights;
    public List<Texture2D> objectHeightMaps = new List<Texture2D>();
    public float heightInfluence = 2;
    public float influenceRange = 2;
    Texture2D objectHeightMap;
    bool reverseColors = false;
    //public int Height = 200;



    public static heightMapGenerator heightMap;

    void Awake()
    {
        if (heightMap == null) heightMap = this;


    }

    private void Start()
    {
        //craterTexture = (Texture2D)Resources.Load("cratermap3");

    }

    public void RandomSeed()
    {
        int index = Random.Range(0, objectHeightMaps.Count);
        Debug.Log(index);
        objectHeightMap = objectHeightMaps[index];

        objectHeights = new float[objectHeightMap.width][];
        int height = objectHeightMap.height;
        for (int i = 0; i < objectHeightMap.width; i++)
        {
            objectHeights[i] = new float[height];
        }

        if (Random.Range(0, 2) == 1) reverseColors = true;
        else reverseColors = false;

        heightInfluence += Random.Range(-influenceRange, influenceRange);

        System.Random rand = new System.Random();
        offsetX = rand.Next(0, 9999);
        offsetZ = rand.Next(0, 9999);
        offsetX2 = rand.Next(0, 9999);
        offsetZ2 = rand.Next(0, 9999);

        objectOffsetZ = rand.Next(-15, 15) + objectHeightMap.height/2;
        objectOffsetX = rand.Next(-15, 15) + objectHeightMap.height / 2;
        CalculateCraterHeights();
    }

    void CalculateCraterHeights()
    {
        for (int x = 0; x < objectHeights.Length; x++)
        {
            for (int y = 0; y < objectHeights[x].Length; y++)
            {
                Color clr = objectHeightMap.GetPixel(x, y);
                float height = 0;
                float heightDevider = 200 / 255f;
                //if (clr.r > heightDevider) 
                //height = -clr.r;
                if(reverseColors)
                {
                    height -= clr.r;
                    height += clr.b;
                }
                else
                {
                    height += clr.r;
                    height -= clr.b;
                }
                //else height = heightDevider - clr.r;
                objectHeights[x][y] = height * heightInfluence;
            }
        }
    }


    public int GenerateHeights(int x, int z, Vector2 chunkKey, Vector3 chunkCorner)
    {
        float height = 0;
        
        float worldX = chunkCorner.x + x;
        float worldZ = chunkCorner.z + z;
        float xCoord;
        float zCoord;


        for (int octave = 1; octave <= octaves; octave++)
        {
            frequency = Mathf.Pow(lacunarity, octave) * frequencyMultiplier;
            amplitude = Mathf.Pow(persistance, octave) * amplitudeMultiplier;

            xCoord = (float)worldX / 1024 * frequency + offsetX;
            zCoord = (float)worldZ / 1024 * frequency + offsetZ;

            height += Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

        }

        frequency = frequencyMultiplier;
        amplitude = amplitudeMultiplier;

        xCoord = worldX / 1024 * frequency + offsetX2;
        zCoord = worldZ / 1024 * frequency + offsetZ2;

        height += Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

        try
        {
            height += objectHeights[objectOffsetX + (int)worldX][objectOffsetZ + (int)worldZ];
        }
        catch { }

        return Mathf.Clamp(Mathf.FloorToInt(Mathf.Round(Mathf.Pow(height, 1.3f))), 1, 255);
    }
}
