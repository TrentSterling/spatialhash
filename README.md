# ⚡ Generic Spatial Hash for Unity

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity 2021.3+](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)](#)

A high-performance, generic **Spatial Hash** for Unity. Optimized for dynamic objects, network synchronization (AOI), and efficient proximity queries.

> "Stop looping through every object. Start hashing them." 🚀

---

## 📦 Quick Start (Copy-Paste Ready)

1. **Drop `SpatialHash.cs` into your project.**
2. **Initialize and use:**

```csharp
using SpatialHash;

// 1. Create (Cell size should be ~2x your average object size)
var hash = new SpatialHash<Transform>(cellSize: 10f);

// 2. Insert/Update
hash.Update(transform, transform.position);

// 3. Query (Non-allocating!)
List<Transform> results = new List<Transform>();
hash.QueryRadius(playerPosition, 20f, results);
```

---

## 🛠️ Key Use Cases

### 🌐 Networking (PurrNet / FishNet / Mirror)
Perfect for **Area of Interest (AOI)**. Only sync data to players within a specific "bubble." 
*Shoutout to the PurrNet crew—this is how you keep those packets lean!*

### 💥 O(N) Collisions
Check collisions only against nearby objects. Turn 1,000,000 checks into 1,000.

### 🗺️ World Streaming
Automatically enable/disable chunks based on where the player is standing.

### 🤖 Pathfinding
Instantly find the nearest nav-node without searching the entire scene.

---

## 📖 Tutorials & Docs
- **[Deep Dive Blog Post](BLOG.md)** - Why Spatial Hashing beats Quadtrees for dynamic games.
- **[Example Scripts](Scripts/Examples/)** - Real-world code for collisions, culling, and more.

---

## 💬 Discord & Community
I built this for my Discord friends and the gamedev community. 
- **Simple to understand.**
- **No bloat.**
- **Pure performance.**

Feel free to fork, hack, and share!

## 📜 License
MIT - Do whatever you want with it.
