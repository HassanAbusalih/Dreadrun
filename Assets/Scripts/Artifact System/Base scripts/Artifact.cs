using System;
using UnityEngine;

public abstract class Artifact
{
    public GameObject prefab;
    public int level;
    public ArtifactManager manager;
    public ScriptableObject settings;

    public abstract void Initialize();
    public abstract void ApplyArtifactEffects();
}
