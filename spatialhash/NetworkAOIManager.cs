using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash
{
    public class NetworkEntity : MonoBehaviour
    {
        public string entityId;
        public bool isLocalPlayer;
        
        // Data that needs to be synced
        [HideInInspector] public Vector3 lastSyncedPosition;
    }

    public class NetworkAOIManager : MonoBehaviour
    {
        public float cellSize = 5f;
        public float syncRadius = 20f;

        private SpatialHash<NetworkEntity> _entityHash;
        private List<NetworkEntity> _allEntities = new List<NetworkEntity>();
        private List<NetworkEntity> _queryResults = new List<NetworkEntity>();

        private void Awake()
        {
            _entityHash = new SpatialHash<NetworkEntity>(cellSize);
        }

        public void RegisterEntity(NetworkEntity entity)
        {
            _allEntities.Add(entity);
            _entityHash.Insert(entity, entity.transform.position);
        }

        public void UnregisterEntity(NetworkEntity entity)
        {
            _allEntities.Remove(entity);
            _entityHash.Remove(entity);
        }

        private void Update()
        {
            // Update all entities in the spatial hash
            foreach (var entity in _allEntities)
            {
                _entityHash.Update(entity, entity.transform.position);
            }

            // Example of local player sync logic
            HandleLocalPlayerSync();
        }

        private void HandleLocalPlayerSync()
        {
            foreach (var entity in _allEntities)
            {
                if (entity.isLocalPlayer)
                {
                    _queryResults.Clear();
                    _entityHash.QueryRadius(entity.transform.position, syncRadius, _queryResults);

                    // Logic to determine who should receive updates for this player
                    // or which other entities this player should see.
                    foreach (var nearby in _queryResults)
                    {
                        if (nearby == entity) continue;
                        
                        // In a real netcode (FishNet/Mirror), you'd call:
                        // nearby.AddViewer(entity) or similar
                    }
                }
            }
        }

        public List<NetworkEntity> GetNearbyEntities(Vector3 position, float radius)
        {
            List<NetworkEntity> results = new List<NetworkEntity>();
            _entityHash.QueryRadius(position, radius, results);
            return results;
        }
    }
}
