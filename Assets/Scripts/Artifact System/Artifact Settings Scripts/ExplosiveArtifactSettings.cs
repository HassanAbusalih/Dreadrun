using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "ExplosiveArtifactSettings", menuName = "ArtifactSettings/ExplosiveArtifactSettings")]
public class ExplosiveArtifactSettings : ArtifactSettings
{
    public float explosionDamagePerLevel;
    public float pushBackForcePerLevel;
    public float explosionRadius;
    public int enemiesRequired;
    public int explosionCooldown;
}
