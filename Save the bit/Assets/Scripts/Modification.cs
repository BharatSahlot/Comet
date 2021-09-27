using System;
using UnityEngine;

public enum ModificationType
{
    None,
    InvertX,
    InvertY,
    InvertXY,
    TurnOffShield,
    ChangeTarget,
    SelfDestruct
}

[Serializable]
public class Modification
{
    public ModificationType modificationType;
    [Range(0, 1)] public float probability;
}