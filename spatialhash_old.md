# Generic Spatial Hash

## Objective
Implement a high-performance, generic spatial hash system for efficient spatial partitioning, proximity queries, and network interest management (AOI - Area of Interest).

## Project Goals
- **Generic Implementation:** Support arbitrary object types and dimensions (2D and 3D).
- **Network Optimization:** Provide a foundation for network condition optimizations, similar to those used in FishNet and Mirror, to manage data syncing between "rooms" or based on proximity.
- **Performance:** Optimized for fast insertions, updates, and queries.
- **Room Connectivity:** (Optional/Future) Integrate with a room connectivity graph for more complex visibility and synchronization logic.

## High-Level Architecture
- **Spatial Hash Grid:** A hash-based data structure that maps spatial coordinates to grid cells.
- **Cell Management:** Dynamic or fixed-size cells to house entity references.
- **Query Interface:** Methods to find entities within a certain radius or within specific grid cells.
- **Update Mechanism:** Efficiently move entities between cells as their positions change.

## Implementation Plan
1. **Phase 1: Core Spatial Hash (C++)**
   - Define the `SpatialHash` template class.
   - Implement cell mapping and entity registration.
   - Basic proximity queries.
2. **Phase 2: Advanced Features**
   - Support for 3D coordinates.
   - Bucket/Cell optimization to avoid excessive memory allocation.
3. **Phase 3: Integration/Testing**
   - Provide examples for network "room" fading and interest management.
   - Benchmarking and performance validation.

## Tech Stack
- **Primary Language:** C++ (for core performance)
- **Optional Wrappers:** Python (for high-level scripting/testing)
