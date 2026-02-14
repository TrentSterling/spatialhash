using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash.Examples
{
    public class PathfindingNode : MonoBehaviour
    {
        public List<PathfindingNode> neighbors = new List<PathfindingNode>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            foreach (var n in neighbors)
            {
                if (n != null)
                    Gizmos.DrawLine(transform.position, n.transform.position);
            }
        }
    }

    public class PathfindingManager : MonoBehaviour
    {
        public float nodeSearchRadius = 5f;
        public float gridSpacing = 2f;
        public Vector2Int gridSize = new Vector2Int(20, 20);

        private SpatialHash<PathfindingNode> _nodeHash;
        private List<PathfindingNode> _allNodes = new List<PathfindingNode>();

        private void Start()
        {
            _nodeHash = new SpatialHash<PathfindingNode>(gridSpacing);
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            // Simple grid generation
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 pos = new Vector3(x * gridSpacing, 0, y * gridSpacing);
                    GameObject go = new GameObject($"Node_{x}_{y}");
                    go.transform.position = pos;
                    go.transform.parent = transform;
                    
                    PathfindingNode node = go.AddComponent<PathfindingNode>();
                    _allNodes.Add(node);
                    _nodeHash.Insert(node, pos);
                }
            }

            // Connect neighbors using the hash
            List<PathfindingNode> potentialNeighbors = new List<PathfindingNode>();
            foreach (var node in _allNodes)
            {
                potentialNeighbors.Clear();
                // Instead of iterating all nodes, check only nearby ones
                _nodeHash.QueryRadius(node.transform.position, gridSpacing * 1.5f, potentialNeighbors);

                foreach (var neighbor in potentialNeighbors)
                {
                    if (neighbor == node) continue;
                    float dist = Vector3.Distance(node.transform.position, neighbor.transform.position);
                    if (dist <= gridSpacing * 1.1f) // slightly more than spacing to account for float errors
                    {
                        node.neighbors.Add(neighbor);
                    }
                }
            }
        }

        public PathfindingNode GetNearestNode(Vector3 position)
        {
            // Instead of looping all nodes (O(N)), we query the hash (O(1) usually)
            List<PathfindingNode> nearby = new List<PathfindingNode>();
            // Search radius depends on node density
            _nodeHash.QueryRadius(position, nodeSearchRadius, nearby);

            PathfindingNode closest = null;
            float minDst = float.MaxValue;

            foreach (var node in nearby)
            {
                float dst = (position - node.transform.position).sqrMagnitude;
                if (dst < minDst)
                {
                    minDst = dst;
                    closest = node;
                }
            }
            return closest;
        }
    }
}
