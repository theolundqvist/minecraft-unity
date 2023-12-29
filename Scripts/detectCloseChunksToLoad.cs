using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class detectCloseChunksToLoad : MonoBehaviour {


    int viewDistance = 48;
    //int range = 24;

    worldData world;


    void Start()
    {
        world = worldData.world;
        //InvokeRepeating("IncreaseRange", 0f, 0.01f);
        InvokeRepeating("CheckForUnloadedChunks", 0, 0.025f);
        InvokeRepeating("CheckForChunksToUnload", 10, 5f);
        SetViewDistance(48);
    }

    public void changeRenderDistance(UnityEngine.UI.Slider slider)
    {
        viewDistance = (int)slider.value;
        SetViewDistance(viewDistance);
    }

    void SetViewDistance(int Distance)
    {
        viewDistance = Distance;
        Camera.main.farClipPlane = viewDistance * 2;
        float[] distances = new float[32];
        distances[0] = viewDistance * 1.8f; //smaller than your camera clipping planes's far value 
        Camera.main.layerCullDistances = distances;
        Camera.main.layerCullSpherical = true;
    }

    //void IncreaseRange()
    //{
    //    if (range < viewDistance)
    //    {
    //        range++;
    //    }
    //    else CancelInvoke("IncreaseRange");
    //}

    void CheckForChunksToUnload()
    {
        Vector2[] keys = world.GetChunkKeys();
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);

        for (int i = 0; i < keys.Length; i++)
        {
            if ((keys[i].x - pos.x) * (keys[i].x - pos.x) + 
                (keys[i].y - pos.y) * (keys[i].y - pos.y) > viewDistance * viewDistance)
            {
                world.UnLoad(keys[i]);
            }
        }
    }


    void CheckForUnloadedChunks()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        float magnitude; //Distance to chunk
        //var chunksToLoad = new Dictionary<float, Vector2>();

        //Search through each integer point
        for (int x = (int)pos.x - viewDistance; x <= (int)pos.x + viewDistance; x++)
        {
            //check if our x is on the chunk grid
            if (x % world.chunkSize == 0 || x == 0)
            {
                for (int y = (int)pos.y - viewDistance; y <= (int)pos.y + viewDistance; y++)
                {
                    //check if our y is on the chunk grid
                    if (y % world.chunkSize == 0 || y == 0)
                    {
                        magnitude = (x - pos.x) * (x - pos.x) + (y - pos.y) * (y - pos.y);
                        //Using pythagoras theorem to conclude that the point is inside our range
                        if (magnitude <= viewDistance * viewDistance)
                        {
                            var chunkToLoad = new Vector2(x, y);
                            //Check if our point already is used (chunk already generated)
                            if (!world.CheckForChunkInMemory(chunkToLoad))
                            {
                                world.Load(chunkToLoad);
                                //try
                                //{
                                //    chunksToLoad.Add(magnitude, new Vector2(x, y));
                                //}
                                //catch { }
                            }
                        }
                    }
                }
            }
        }

        //if(chunksToLoad != null)
        //{
        //    var list = chunksToLoad.Keys.ToList();
        //    list.Sort();
        //    if(chunksToLoad.Count > 1)
        //    {
        //        list.RemoveRange(1, list.Count - 1);
        //    }
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        world.Load(chunksToLoad[list[i]]);
        //    }
        //}
    }
}
