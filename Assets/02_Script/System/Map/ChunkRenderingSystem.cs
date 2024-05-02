using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct ChunkLoadJob : IJob
{

    [ReadOnly] public NativeArray<int2> chunks;
    public NativeList<int2> loadingChunk;
    public int2 playerPos;

    public void Execute()
    {

        for(int index = 0; index < chunks.Length; ++index)
        {

            if (math.distance(playerPos, chunks[index]) < 30)
            {

                loadingChunk.Add(chunks[index]);

            }

        }

    }

}

public class ChunkRenderingSystem : MonoBehaviour
{

    private Dictionary<int2, Renderer[]> chunkContainer = new();
    private NativeArray<int2> chunks;
    private NativeList<int2> loadingChunk;
    private JobHandle jobHandle;
    private int fpsCount;
    private bool isSchedule;
    public static ChunkRenderingSystem Instance;

    private void Awake()
    {
        
        Instance = this;

    }

    private void ApplyChunkRender(in NativeList<int2> loadingChunk)
    {

        foreach (var item in chunkContainer)
        {

            var inc = Include(loadingChunk, item.Key);

            if (item.Value[0].enabled == inc) continue;

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

            var x = item == value;

            if (x.y && x.x) return true;

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

    private void Update()
    {

        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.localController == null ||
            chunkContainer.Count == 0)
        {

            return;
        }


        if (!isSchedule)
        {


            chunks = new NativeArray<int2>(chunkContainer.Keys.ToArray(), Allocator.TempJob);
            loadingChunk = new NativeList<int2>(Allocator.TempJob);
            var originPos = PlayerManager.Instance.localController.transform.position;
            int2 playerPos = new int2(Mathf.FloorToInt(originPos.x), Mathf.FloorToInt(originPos.z));

            var job = new ChunkLoadJob
            {

                chunks = chunks,
                loadingChunk = loadingChunk,
                playerPos = playerPos,

            };

            jobHandle = job.Schedule();

            isSchedule = true;

        }

    }

    private void LateUpdate()
    {

        fpsCount++;


        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.localController == null ||
            chunkContainer.Count == 0)
        {

            return;
        }


        if (fpsCount != 4 && !isSchedule) return;


        jobHandle.Complete();

        ApplyChunkRender(loadingChunk);

        chunks.Dispose();
        loadingChunk.Dispose();

        isSchedule = false;


    }


    private void OnDestroy()
    {

        if (chunks.IsCreated)
        {

            chunks.Dispose();

        }

        if (loadingChunk.IsCreated)
        {

            loadingChunk.Dispose();

        }

    }

}