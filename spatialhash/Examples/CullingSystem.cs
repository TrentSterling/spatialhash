using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash.Examples
{
    public class SimpleObject : MonoBehaviour
    {
        public MeshRenderer renderer;
    }

    public class CullingSystem : MonoBehaviour
    {
        public Camera mainCamera;
        public float cullingRadius = 50f;
        public int objectCount = 500;
        public SimpleObject prefab; // Ensure it has a renderer

        private SpatialHash<SimpleObject> _cullingHash;
        private List<SimpleObject> _allObjects = new List<SimpleObject>();
        private List<SimpleObject> _visibleObjects = new List<SimpleObject>();

        private void Start()
        {
            _cullingHash = new SpatialHash<SimpleObject>(10f);
            
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 pos = Random.insideUnitSphere * 100f;
                SimpleObject obj = Instantiate(prefab, pos, Quaternion.identity);
                obj.renderer = obj.GetComponent<MeshRenderer>();
                _allObjects.Add(obj);
                _cullingHash.Insert(obj, pos);
            }
        }

        private void LateUpdate()
        {
            // Reset visibility (could optimize by only disabling those that left view)
            foreach (var obj in _visibleObjects)
            {
                if (obj != null && obj.renderer != null)
                    obj.renderer.enabled = false;
            }
            _visibleObjects.Clear();

            // Only query objects roughly in front of camera or within radius
            // Here we use radius for simplicity, but could use QueryBounds with camera frustum bounds
            List<SimpleObject> potentialVisible = new List<SimpleObject>();
            _cullingHash.QueryRadius(mainCamera.transform.position, cullingRadius, potentialVisible);

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            foreach (var obj in potentialVisible)
            {
                if (obj == null || obj.renderer == null) continue;

                // Check if object bounds are within frustum
                if (GeometryUtility.TestPlanesAABB(planes, obj.renderer.bounds))
                {
                    obj.renderer.enabled = true;
                    _visibleObjects.Add(obj);
                }
            }
        }
    }
}
