using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash
{
    public class RoomManager : MonoBehaviour
    {
        public float cellSize = 10f;
        private SpatialHash<Room> _roomHash;
        private List<Room> _allRooms = new List<Room>();

        private void Awake()
        {
            _roomHash = new SpatialHash<Room>(cellSize);
            InitializeRooms();
        }

        private void InitializeRooms()
        {
            var rooms = FindObjectsOfType<Room>();
            foreach (var room in rooms)
            {
                RegisterRoom(room);
            }
        }

        public void RegisterRoom(Room room)
        {
            _allRooms.Add(room);
            // We use the center of the room for the hash, 
            // but QueryBounds can be used for more precise checks
            _roomHash.Insert(room, room.bounds.center);
        }

        public Room GetRoomAtPosition(Vector3 position)
        {
            // Query a small area around the position
            List<Room> nearbyRooms = new List<Room>();
            _roomHash.QueryRadius(position, cellSize, nearbyRooms);

            foreach (var room in nearbyRooms)
            {
                if (room.bounds.Contains(position))
                {
                    return room;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all rooms that should be synchronized based on the player's position.
        /// Includes the current room and its direct neighbors.
        /// </summary>
        public HashSet<Room> GetSyncSet(Vector3 playerPosition)
        {
            HashSet<Room> syncSet = new HashSet<Room>();
            Room currentRoom = GetRoomAtPosition(playerPosition);

            if (currentRoom != null)
            {
                syncSet.Add(currentRoom);
                foreach (var neighbor in currentRoom.connectedRooms)
                {
                    syncSet.Add(neighbor);
                }
            }

            return syncSet;
        }
    }
}
