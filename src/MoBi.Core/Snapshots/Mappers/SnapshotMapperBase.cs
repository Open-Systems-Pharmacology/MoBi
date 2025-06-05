﻿using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public abstract class SnapshotMapperBase<TModel, TSnapshot> : SnapshotMapperBase<TModel, TSnapshot, SnapshotContext> where TSnapshot : new()
{
}