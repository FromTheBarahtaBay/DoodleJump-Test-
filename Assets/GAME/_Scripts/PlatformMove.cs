using DG.Tweening;
using UnityEngine;

public class PlatformMove : Platform
{
    private Sequence _sequence;
    private float _leftBorder, _rightBorder;

    private void Awake() {
        _leftBorder = Random.Range(-1.3f, .1f);
        _rightBorder = Random.Range(-.1f, 1.3f);
        var random = Random.Range(.5f, 3f);
        MovePlatform(random);
    }

    private void MovePlatform(float duration) {
        _sequence = DOTween.Sequence();

        _sequence
            .Append(transform.DOMoveX(_rightBorder, duration).From(_leftBorder))
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(transform.gameObject)
            .SetAutoKill(true)
            .Play();
    }
}