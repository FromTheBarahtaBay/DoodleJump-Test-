using DG.Tweening;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Sequence _sequence;
    private float _jumpForce = 20f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
                if (rb.velocity.y < -.1) {
                    SpringAnimation();
                    rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
                }
            }
        }
    }

    private void SpringAnimation() {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        _sequence
            // Сжата: от 0.7 к 1.2 (резкое распрямление)
            .Append(transform.DOScaleY(1.5f, 0.25f).From(0.7f).SetEase(Ease.OutQuad))

            // Колебания пружины — чуть меньше, чем основной удар
            .Append(transform.DOScaleY(0.5f, 0.15f).SetEase(Ease.InOutSine))
            .Append(transform.DOScaleY(1.05f, 0.1f).SetEase(Ease.InOutSine))
            .Append(transform.DOScaleY(.7f, 0.1f).SetEase(Ease.OutQuad))

            .OnComplete(() => {
                transform.localScale = new Vector3(.8f, .7f, 1);
            })

            .SetAutoKill(true)
            .Play();

    }
}