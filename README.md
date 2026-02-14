# ⚡ SpatialHash for Unity

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Live Docs](https://img.shields.io/badge/Live-Documentation-blue.svg)](https://trentsterling.github.io/spatialhash/)

A high-performance, generic **Spatial Hash** for Unity. Optimized for dynamic objects, network synchronization (AOI), and efficient proximity queries.

## 🚀 Live Documentation & Tutorial
View the interactive guide, use cases, and performance deep-dive at:
**[https://trentsterling.github.io/spatialhash/](https://trentsterling.github.io/spatialhash/)**

---

## 📦 Quick Start

1. **Drop `SpatialHash.cs` into your project.**
2. **Initialize and use:**

```csharp
using SpatialHash;

// Create
var hash = new SpatialHash<Transform>(cellSize: 10f);

// Update
hash.Update(transform, transform.position);

// Query
List<Transform> results = new List<Transform>();
hash.QueryRadius(playerPosition, 20f, results);
```

## 🛠️ Key Use Cases
- **Networking (PurrNet/FishNet/Mirror)**: Area of Interest (AOI) management.
- **O(N) Collisions**: Optimize thousands of projectile/unit checks.
- **World Streaming**: Dynamic chunk loading/unloading.
- **Pathfinding**: Fast nearest-node lookup.

## 📜 License
MIT - Free for all use.
