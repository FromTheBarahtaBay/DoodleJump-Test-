using UnityEngine;

[System.Serializable]
public class DataPlayer
{
    [field: SerializeField] public Transform PlayerTransform { get; private set; }
    [field: SerializeField] public Rigidbody2D PlayerRB { get; private set; }
    [field: SerializeField] public SpriteRenderer PlayerSpriteRenderer { get; private set; }
}