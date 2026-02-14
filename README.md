# ⚡ SpatialHash Masterclass

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Live Tutorial](https://img.shields.io/badge/Live-Tutorial-blue.svg)](http://tront.xyz/spatialhash/)
[![Unity 2021.3+](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)](#)

A high-performance, generic **Spatial Hash** for Unity. Architected for O(1) performance, zero allocations, and data-oriented simplicity.

## 🚀 Live Masterclass & Interactive Demos
Explore the 5-part technical deep-dive, interactive visualizers, and the **Pixel Pickup** stress-test arena at:
**[http://tront.xyz/spatialhash/](http://tront.xyz/spatialhash/)**

---

## 📦 The Toolkit

This repository contains a production-ready spatial partitioning suite:

### 1. [SpatialHash.cs](spatialhash/SpatialHash.cs) (Core API)
The generic, non-allocating 3D hash engine. 
- **Generic**: Works with any component or class.
- **Bucket Pooling**: Zero-allocation internal memory recycling.
- **Query Types**: High-speed Sphere and AABB checks.

### 2. [NetworkAOI.cs](spatialhash/NetworkAOIManager.cs) (Networking)
Plug-and-play Interest Management for **PurrNet**, FishNet, and Mirror.
- Sync only what matters to each player.
- Save 90% of your network bandwidth.

### 3. [RoomManager.cs](spatialhash/RoomManager.cs) (World State)
Portal-based visibility and room synchronization logic (Inspired by **CellGen**).

### 4. [Samples Gallery](spatialhash/Examples/)
Complete implementations for:
- **CollisionBroadphase**: 10x faster trigger systems.
- **PathfindingLookup**: Fast nearest-node search.
- **WorldStreamer**: Dynamic level/chunk loading.
- **FrustumCulling**: Broad-phase visibility filtering.

---

## 🛠️ Quick Start

```csharp
using SpatialHash;

// 1. Initialize (Cell size should be ~2x your query radius)
var hash = new SpatialHash<Transform>(cellSize: 10f);

// 2. Register/Update (O(1))
hash.Update(myEntity, myEntity.position);

// 3. Query (Zero-Allocation)
List<Transform> results = new List<Transform>();
hash.QueryRadius(playerPosition, 20f, results);
```

---

## 📜 License
MIT - Created for the community by **Trent Sterling**. 
Come say hi on [Discord](https://discord.gg/0hyoWZyM6y7kkFCN)!
