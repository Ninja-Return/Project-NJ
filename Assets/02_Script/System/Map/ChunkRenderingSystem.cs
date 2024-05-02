using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct ChunkLoadJob : IJobParallelFor
{

    public NativeArray<int2> chunks;
    public NativeList<int2> loadingChunk;
    public int2 playerPos;

    public void Execute(int index)
    {

        if(math.distance(playerPos, chunks[index]) < 30)
        {

            loadingChunk.Add(chunks[index]);

        }

    }

}

public class ChunkRenderingSystem : MonoBehaviour
{

    private Dictionary<int2, Renderer[]> chunkContainer = new();

    private void Start()
    {

        StartCoroutine(ChunkCalculateLoop());

    }

    private void ApplyChunkRender(in NativeList<int2> loadingChunk)
    {

        foreach (var item in chunkContainer)
        {

            var inc = Include(loadingChunk, item.Key);

            foreach (var renderer in item.Value)
            {

                renderer.enabled = inc;

            }

        }

    }

    private bool Include(in NativeList<int2> array, int2 value)
    {

        foreach(var item in array)
        {

            if(item.x == value.x && item.y == value.y) return true;

        }

        return false;

    }
    public void AddChunk(Vector3 pos, Renderer[] renderers)
    {

        int2 chunkPos = new int2 { x = Mathf.FloorToInt(pos.x), y = Mathf.FloorToInt(pos.z) };

        if (!chunkContainer.ContainsKey(chunkPos))
        {

            chunkContainer.Add(chunkPos, renderers);

        }

    }

    private IEnumerator ChunkCalculateLoop()
    {

        while (true)
        {

            if (PlayerManager.Instance == null ||
                PlayerManager.Instance.localController == null ||
                chunkContainer.Count == 0)
            {

                yield return null;
                continue;

            }

            NativeArray<int2> chunks = new NativeArray<int2>(chunkContainer.Keys.ToArray(), Allocator.TempJob);
            NativeList<int2> loadingChunk = new NativeList<int2>(Allocator.TempJob);
            var originPos = PlayerManager.Instance.localController.transform.position;
            int2 playerPos = new int2(Mathf.FloorToInt(originPos.x), Mathf.FloorToInt(originPos.z));

            var job = new ChunkLoadJob 
            { 
                
                chunks = chunks,
                loadingChunk = loadingChunk,
                playerPos = playerPos,
            
            };

            var handle = job.Schedule(chunks.Length, 1);

            for(int i = 0; i < 4; i++)
            {

                yield return null;

            }

            handle.Complete();

            ApplyChunkRender(loadingChunk);

            chunks.Dispose();
            loadingChunk.Dispose();

            yield return null;

        }

    }

}