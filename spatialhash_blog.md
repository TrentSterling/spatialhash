# Beyond the Grid: Building a Generic Spatial Hash for Unity

*Stop looping through every object in your scene. There’s a better way.*

If you’ve ever built a game with hundreds of units, a massive open world, or a multiplayer system, you’ve hit "The Wall." You know the one: where your collision checks, pathfinding lookups, or network sync logic starts eating your frame budget for breakfast.

The culprit? **O(N²) complexity.** 

Checking every object against every other object is fine for 10 units. At 1,000 units, you’re doing 1,000,000 checks. Even on modern CPUs, that’s a death sentence.

## Enter the Spatial Hash

While many developers jump straight to **Quadtrees** or **BSP Trees**, they often overlook the simpler, faster, and more dynamic cousin: the **Spatial Hash**.

### What is it?
Imagine your game world divided into a grid. Instead of a complex tree structure, a Spatial Hash is essentially a **Dictionary where the key is a grid cell coordinate, and the value is a list of objects in that cell.**

Mapping a position to a cell is a simple division:
`cellX = floor(posX / cellSize)`

This gives us **O(1)** average-time complexity for everything. Insertion? O(1). Removal? O(1). Finding neighbors? O(1).

## Why This Implementation?

Most spatial hashes are rigid. They are built specifically for one type of object or a fixed grid size. Our implementation is **Generic** and **Agentic**:

1.  **Generic**: `SpatialHash<T>` doesn't care if you're hashing `NetworkEntities`, `PathfindingNodes`, or `Room` components.
2.  **Non-Allocating**: In Unity, Garbage Collection (GC) is the enemy. Our query methods use pre-allocated lists to keep your memory profile flat.
3.  **3D Ready**: While many hashes are 2D, this one handles `Vector3` natively, making it perfect for flight sims, underwater games, or vertical dungeons.

---

## 5 Ways This Will Save Your Project

### 1. Network AOI (Area of Interest)
Whether you’re using **FishNet**, **Mirror**, or **PurrNet**, network bandwidth is precious. You shouldn't sync a player’s weapon swap to someone 500 meters away. 
By hashing your `NetworkEntities`, your server can query a 50m radius around each player and only send updates for the entities found in that specific "bubble."

### 2. The $O(N)$ Collision Hack
Standard collision is $O(N^2)$. With a spatial hash, it becomes nearly **O(N)**. You only check collisions for objects in your own cell and the 26 neighboring cells (in 3D). This is how you handle thousands of projectiles without your GPU crying.

### 3. Smart Pathfinding
Don't loop through your entire node graph to find the "closest node" to a click. Query the hash at the click position. You’ll get the 3-4 nodes in that immediate cell instantly.

### 4. Efficient World Streaming
Building an open world? Use the hash to manage "Chunks." As the player moves, the `SpatialHash` tells you exactly which chunks should be loaded, enabled, or pooled.

### 5. Culling & Visibility
Unity’s built-in culling is great, but sometimes you need more control (like the **CellGen** system). Using a spatial hash to manage "Rooms" and their connectivity allows you to propagate visibility through doors and portals without complex raycasting every frame.

---

## Implementation Sneak Peek

```csharp
// Setup
var hash = new SpatialHash<MyEntity>(cellSize: 10f);

// Update (O(1))
hash.Update(entity, transform.position);

// Query (Non-allocating)
hash.QueryRadius(playerPosition, syncRadius, resultsBuffer);
```

## Wrapping Up

The Spatial Hash is the "Swiss Army Knife" of game dev optimization. It’s easier to understand than a BSP tree, faster to update than a Quadtree, and versatile enough to handle everything from netcode to physics.

Check out the `README.md` for the full API and the `Examples` folder to see it in action!

---
*Happy Hacking!*
