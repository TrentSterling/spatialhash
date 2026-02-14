using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash.Examples
{
    public class WorldStreamer : MonoBehaviour
    {
        public float chunkSize = 50f;
        public float loadRadius = 150f;
        public Transform player;

        private SpatialHash<GameObject> _chunkHash;
        private List<GameObject> _allChunks = new List<GameObject>();
        private List<GameObject> _loadedChunks = new List<GameObject>();

        private void Start()
        {
            _chunkHash = new SpatialHash<GameObject>(chunkSize);
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            // Simulate generating 10x10 chunks
            for (int x = -5; x < 5; x++)
            {
                for (int z = -5; z < 5; z++)
                {
                    Vector3 pos = new Vector3(x * chunkSize, 0, z * chunkSize);
                    GameObject chunk = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    chunk.name = $"Chunk_{x}_{z}";
                    chunk.transform.position = pos;
                    chunk.transform.localScale = new Vector3(chunkSize, 1, chunkSize);
                    chunk.SetActive(false); // Start unloaded

                    _allChunks.Add(chunk);
                    _chunkHash.Insert(chunk, pos);
                }
            }
        }

        private void Update()
        {
            if (player == null) return;

            // Only update occasionally or when player moves significantly
            if (Time.frameCount % 10 == 0) 
            {
                UpdateChunks();
            }
        }

        private void UpdateChunks()
        {
            // Disable previously loaded chunks first (or verify they are still in range)
            foreach (var chunk in _loadedChunks)
            {
                if (chunk != null) chunk.SetActive(false);
            }
            _loadedChunks.Clear();

            // Query chunks around player
            List<GameObject> nearbyChunks = new List<GameObject>();
            _chunkHash.QueryRadius(player.position, loadRadius, nearbyChunks);

            foreach (var chunk in nearbyChunks)
            {
                // Verify exact distance (optional, query already roughly filters)
                if (Vector3.Distance(player.position, chunk.transform.position) <= loadRadius)
                {
                    chunk.SetActive(true);
                    _loadedChunks.Add(chunk);
                }
            }
        }
    }
}
