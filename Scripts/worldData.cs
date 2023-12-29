using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public class worldData : MonoBehaviour {

    public static worldData world;
    public Texture2D texture;
    public int chunkSize = 10;
    public int chunkHeight = 128;


    Dictionary<Vector2, chunkData> chunks = new Dictionary<Vector2, chunkData>();

    public void TestDictionarySpeed(int Times)
    {
        UnityEngine.Debug.Log("Starting test, " + Times + " samples");
        Stopwatch watch = new Stopwatch();
        watch.Start();



        Dictionary<int, Mesh> meshes = new Dictionary<int, Mesh>();
        for (int i = 0; i < Times; i++)
        {
            meshes.Add(i, new Mesh());
        }



        watch.Stop();
        UnityEngine.Debug.Log("Elapsed time: " + watch.ElapsedMilliseconds + " ms");
    }


    List<float> time = new List<float>();
    public void addWatchTime(float timeIn_ms)
    {
        time.Add(timeIn_ms);

        float medianTime = 0;
        for (int i = 0; i < time.Count; i++)
        {
            medianTime += time[i];
        }


        
        //UnityEngine.Debug.Log(medianTime / time.Count);
    }


    // Use this for initialization
    void Awake () {
        if (world == null) world = this;
	}

    void Start()
    {
        ResetWorld();
    }

    public int GetBlockID(Vector3 position)
    {
        chunkData chunk = chunks[new Vector2(position.x / chunkSize - position.x % chunkSize,
            position.y / chunkSize - position.y % chunkSize)];

        return chunk.GetBlockID(position);
    }

    public Vector2[] GetChunkKeys()
    {
        return chunks.Keys.ToArray();
    }

    public chunkData GetChunk(Vector2 key)
    {
        if (!chunks.ContainsKey(key)) return null;
        return chunks[key];
    }

    public void ResetWorld()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        heightMapGenerator.heightMap.RandomSeed();
        chunks = new Dictionary<Vector2, chunkData>();
        Load(new Vector2(0, 0));
        GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>().ResetPosition();
    }

    public bool CheckForChunkInMemory(Vector2 chunkPosition)
    {
        //Check for saved chunk here
        return chunks.ContainsKey(chunkPosition);
    }

    public void Load(Vector2 key)
    {

        //Load chunk from file if exsists
        GameObject chunk = (GameObject)Instantiate(Resources.Load("Chunk"), new Vector3(key.x, 0, key.y), Quaternion.LookRotation(Vector3.forward));
        
        chunks.Add(key, new chunkData(key, chunkSize, chunk));

        chunk.transform.SetParent(transform);

        //chunk.GetComponent<Renderer>().material.mainTexture = texture;

        //Vector2[] dir = new Vector2[4]
        //{
        //    new Vector2(chunkSize, 0),
        //    new Vector2(-chunkSize, 0),
        //    new Vector2(0, chunkSize),
        //    new Vector2(0, -chunkSize),
        //};

        //for (int i = 0; i < dir.Length; i++)
        //{
        //    if(chunks.ContainsKey(key + dir[i]))
        //    {
        //        chunks[dir[i]].RecalculateMeshing();
        //    }

        //}

        //Testing
        //GameObject block = (GameObject)Instantiate(Resources.Load("Block"), new Vector3(key.x, 0, key.y), Quaternion.LookRotation(Vector3.forward));
        //block.transform.SetParent(chunk.transform);
        //block.transform.localScale = new Vector3(16f, 1f, 16f);
    }
    public void UnLoad(Vector2 key)
    {
        Destroy(chunks[key].chunk);
        chunks.Remove(key);
        
    }
}