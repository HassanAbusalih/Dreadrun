using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : ScriptableObject
{
    public GameObject prefab;
    [NonSerialized] public int level;

    public abstract void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager);
    public abstract void InitializeArtifact();
}
