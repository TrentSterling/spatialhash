using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash.Examples
{
    public class SimpleCollider : MonoBehaviour
    {
        public float radius = 0.5f;
        [HideInInspector] public bool isColliding = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = isColliding ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    public class CollisionSystem : MonoBehaviour
    {
        public float cellSize = 2f;
        public int objectCount = 100;
        public float spawnRadius = 20f;
        public SimpleCollider prefab; // Assign a sphere prefab here

        private SpatialHash<SimpleCollider> _spatialHash;
        private List<SimpleCollider> _colliders = new List<SimpleCollider>();
        private List<SimpleCollider> _queryResults = new List<SimpleCollider>();

        private void Start()
        {
            _spatialHash = new SpatialHash<SimpleCollider>(cellSize);

            // Spawn random objects
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 pos = Random.insideUnitSphere * spawnRadius;
                pos.y = 0; // Keep it 2D-ish for easier visualization, works in 3D too
                var obj = Instantiate(prefab, pos, Quaternion.identity, transform);
                _colliders.Add(obj);
                _spatialHash.Insert(obj, pos);
            }
        }

        private void Update()
        {
            // Reset states
            foreach (var col in _colliders)
            {
                col.isColliding = false;
                
                // Update position in hash if it moved (simulating movement)
                // In a real game, only update if transform.hasChanged
                _spatialHash.Update(col, col.transform.position);
            }

            // Check collisions efficiently
            foreach (var col in _colliders)
            {
                _queryResults.Clear();
                // We only need to check objects within 2x radius (max possible distance for intersection)
                float searchRadius = col.radius * 2f;
                
                _spatialHash.QueryRadius(col.transform.position, searchRadius, _queryResults);

                foreach (var other in _queryResults)
                {
                    if (col == other) continue;

                    float distSq = (col.transform.position - other.transform.position).sqrMagnitude;
                    float combinedRadius = col.radius + other.radius;
                    
                    if (distSq < combinedRadius * combinedRadius)
                    {
                        col.isColliding = true;
                        other.isColliding = true;
                        // Resolve collision or apply damage here
                    }
                }
            }
        }
    }
}
