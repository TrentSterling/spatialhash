using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash
{
    /// <summary>
    /// A generic spatial hash implementation for 3D spatial partitioning.
    /// </summary>
    /// <typeparam name="T">The type of object to store.</typeparam>
    public class SpatialHash<T>
    {
        private readonly float _cellSize;
        private readonly Dictionary<int, List<T>> _grid;
        private readonly Dictionary<T, Vector3> _positionMap;
        private readonly Stack<List<T>> _pool;

        public SpatialHash(float cellSize)
        {
            _cellSize = cellSize;
            _grid = new Dictionary<int, List<T>>();
            _positionMap = new Dictionary<T, Vector3>();
            _pool = new Stack<List<T>>();
        }

        private List<T> GetNewList()
        {
            if (_pool.Count > 0) return _pool.Pop();
            return new List<T>();
        }

        private void RecycleList(List<T> list)
        {
            list.Clear();
            _pool.Push(list);
        }

        private int GetCellKey(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / _cellSize);
            int y = Mathf.FloorToInt(position.y / _cellSize);
            int z = Mathf.FloorToInt(position.z / _cellSize);

            // Simple hash combine using primes
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                hash = hash * 23 + z;
                return hash;
            }
        }

        public void Insert(T item, Vector3 position)
        {
            int key = GetCellKey(position);
            if (!_grid.TryGetValue(key, out List<T> cell))
            {
                cell = GetNewList();
                _grid[key] = cell;
            }

            cell.Add(item);
            _positionMap[item] = position;
        }

        public void Remove(T item)
        {
            if (_positionMap.TryGetValue(item, out Vector3 position))
            {
                int key = GetCellKey(position);
                if (_grid.TryGetValue(key, out List<T> cell))
                {
                    cell.Remove(item);
                    if (cell.Count == 0)
                    {
                        _grid.Remove(key);
                        RecycleList(cell);
                    }
                }
                _positionMap.Remove(item);
            }
        }

        public void Update(T item, Vector3 newPosition)
        {
            if (_positionMap.TryGetValue(item, out Vector3 oldPosition))
            {
                int oldKey = GetCellKey(oldPosition);
                int newKey = GetCellKey(newPosition);

                if (oldKey != newKey)
                {
                    // Move between cells
                    if (_grid.TryGetValue(oldKey, out List<T> oldCell))
                    {
                        oldCell.Remove(item);
                        if (oldCell.Count == 0)
                        {
                            _grid.Remove(oldKey);
                            RecycleList(oldCell);
                        }
                    }

                    if (!_grid.TryGetValue(newKey, out List<T> newCell))
                    {
                        newCell = GetNewList();
                        _grid[newKey] = newCell;
                    }
                    newCell.Add(item);
                }
                
                _positionMap[item] = newPosition;
            }
            else
            {
                Insert(item, newPosition);
            }
        }

        public void QueryRadius(Vector3 position, float radius, List<T> results)
        {
            int minX = Mathf.FloorToInt((position.x - radius) / _cellSize);
            int maxX = Mathf.FloorToInt((position.x + radius) / _cellSize);
            int minY = Mathf.FloorToInt((position.y - radius) / _cellSize);
            int maxY = Mathf.FloorToInt((position.y + radius) / _cellSize);
            int minZ = Mathf.FloorToInt((position.z - radius) / _cellSize);
            int maxZ = Mathf.FloorToInt((position.z + radius) / _cellSize);

            float sqrRadius = radius * radius;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        int key = GetCellKeyFromCoords(x, y, z);
                        if (_grid.TryGetValue(key, out List<T> cell))
                        {
                            foreach (var item in cell)
                            {
                                if ((_positionMap[item] - position).sqrMagnitude <= sqrRadius)
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void QueryBounds(Bounds bounds, List<T> results)
        {
            int minX = Mathf.FloorToInt(bounds.min.x / _cellSize);
            int maxX = Mathf.FloorToInt(bounds.max.x / _cellSize);
            int minY = Mathf.FloorToInt(bounds.min.y / _cellSize);
            int maxY = Mathf.FloorToInt(bounds.max.y / _cellSize);
            int minZ = Mathf.FloorToInt(bounds.min.z / _cellSize);
            int maxZ = Mathf.FloorToInt(bounds.max.z / _cellSize);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        int key = GetCellKeyFromCoords(x, y, z);
                        if (_grid.TryGetValue(key, out List<T> cell))
                        {
                            foreach (var item in cell)
                            {
                                if (bounds.Contains(_positionMap[item]))
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int GetCellKeyFromCoords(int x, int y, int z)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                hash = hash * 23 + z;
                return hash;
            }
        }

        public void Clear()
        {
            foreach (var cell in _grid.Values)
            {
                RecycleList(cell);
            }
            _grid.Clear();
            _positionMap.Clear();
        }
    }
}
