using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ScriptableObject, ICollectable
{
    public new string name;
    public Sprite icon;
    public string description;
}
