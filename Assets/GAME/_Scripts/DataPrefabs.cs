using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPrefabs
{
    [field:SerializeField] public Platform PlatformDefault { get; private set; }
    [field: SerializeField] public Platform PlatformBroken { get; private set; }
    [field: SerializeField] public Platform PlatformMove { get; private set; }
    [field: SerializeField] public Platform PlatformSpring { get; private set; }
    [field: SerializeField] public Platform PlatformEnemy { get; private set; }
}