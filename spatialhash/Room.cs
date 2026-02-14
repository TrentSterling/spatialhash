using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash
{
    public class Room : MonoBehaviour
    {
        public string roomId;
        public Bounds bounds;
        public List<Room> connectedRooms = new List<Room>();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
