using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformTypes {
    Default,
    Broken,
    Moved,
    Spring,
    Enemy
}

public class Platform : MonoBehaviour
{
    public PlatformTypes PlatformType;
}