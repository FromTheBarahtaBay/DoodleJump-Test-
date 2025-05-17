using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlatformBroken : Platform
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
}