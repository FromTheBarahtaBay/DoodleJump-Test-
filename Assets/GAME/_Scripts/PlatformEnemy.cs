using DG.Tweening;
using UnityEngine;

public class PlatformEnemy : Platform
{
    private Sequence _sequence;
    private Sequence _sequencePlayer;
    private float _leftBorder, _rightBorder;
    private SpriteRenderer _enemySpriteRenderer;

    private void Awake() {
        _leftBorder = Random.Range(-1.3f, .1f);
        _rightBorder = Random.Range(-.1f, 1.3f);
        var random = Random.Range(2f, 5f);
        MovePlatform(random);
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void MovePlatform(float duration) {
        _sequence = DOTween.Sequence();

        _sequence = DOTween.Sequence()
            .Append(transform.DOMoveX(_rightBorder, duration).From(_leftBorder))
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(transform.gameObject)
            .SetAutoKill(false)
            .OnStepComplete(() => {
                _enemySpriteRenderer.flipX = !_enemySpriteRenderer.flipX;
            })
            .Play();

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerHited(other);
            Events.OnIsPlayerHited();
        }
    }

    private void PlayerHited(Collider2D other) {

        if (other.gameObject.TryGetComponentInChildren<SpriteRenderer>(out SpriteRenderer spriteRenderer)) {

            _sequencePlayer = DOTween.Sequence();

            _sequencePlayer
                .Append(spriteRenderer.DOFade(.2f, .1f).From(1))
                .Append(spriteRenderer.DOFade(1f, 0f))
                .SetLink(transform.gameObject)
                .SetAutoKill(true)
                .SetLoops(10, LoopType.Yoyo)
                .Play();
        }
    }
}