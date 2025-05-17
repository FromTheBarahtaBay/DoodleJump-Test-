using DG.Tweening;
using UnityEngine;

public class PlayerController
{
    private float _moveSpeed = 5f;
    private float _jumpForce = 10f;
    private Rigidbody2D _playerRb;
    private Transform _playerTransform;
    private SpriteRenderer _playerSpriteRenderer;
    private int _groundLayer;
    private Collider2D[] _groundHits = new Collider2D[1];
    private float _leftBound;
    private float _rightBound;
    private Sequence _jumpSequence, _fallSequence;

    private bool _isGameStart = false;
    private bool _playerIsHited = false;

    public PlayerController(Bootstrap bootstrap) {
        _playerRb = bootstrap.DataPlayer.PlayerRB;
        _playerTransform = bootstrap.DataPlayer.PlayerTransform;
        _playerSpriteRenderer = bootstrap.DataPlayer.PlayerSpriteRenderer;
        _groundLayer = LayerMask.GetMask("Platform");
        bootstrap.AddActionToUpdate(OnUpdate);
        bootstrap.AddActionToGizmos(OnDrawGizmosSelected);
        bootstrap.AddActionToDisable(OnDisable);
        _leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        _rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        Events.IsGameStart += StartGame;
        Events.IsPlayerHited += PlayerIsHited;
    }

    private void OnUpdate() {
        PlayerMoveControl();
        CheckGroundCollision();
    }

    private void PlayerMoveControl() {

        if (_playerTransform.position.x > _rightBound)
            _playerTransform.position = new Vector2(_leftBound, _playerTransform.position.y);
        else if (_playerTransform.position.x < _leftBound)
            _playerTransform.position = new Vector2(_rightBound, _playerTransform.position.y);

        if (!_isGameStart) return;

        float move = Input.GetAxis("Horizontal");

        ChangePlayerDirectionOnMove(move);

        _playerRb.velocity = new Vector2(move * _moveSpeed, _playerRb.velocity.y);
    }

    private void ChangePlayerDirectionOnMove(float move) {
        if (Input.anyKey) {
            if (move > 0) {
                _playerSpriteRenderer.flipX = false;
            } else {
                _playerSpriteRenderer.flipX = true;
            }
        }
    }

    private void CheckGroundCollision() {

        if (_playerIsHited) return;

        if (_playerRb.velocity.y < -0.1f) {
            FallAnimation();
            Vector2 checkPos = _playerTransform.position + Vector3.down * 0.15f;
            var hit = Physics2D.OverlapCircleNonAlloc(checkPos, 0.2f, _groundHits, _groundLayer);
            if (hit != 0) {
                if (_groundHits[0].CompareTag("PlatformBroke")) {
                    if (_groundHits[0].TryGetComponent<Animator>(out Animator animator)) {
                        animator.speed = 1f;
                        animator.enabled = true;
                        animator.Play("platformBroke");
                    }
                } else {
                    _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpForce);
                }
            }
        } else {
            JumpAnimation();
        }
    }

    private void OnDrawGizmosSelected() {
        if (_playerTransform != null) {
            Gizmos.color = Color.red;
            Vector2 checkPos = _playerTransform.position + Vector3.down * 0.5f;
            Gizmos.DrawWireSphere(_playerTransform.position, 0.2f);
        }
    }

    private void JumpAnimation() {

        _fallSequence?.Kill();
        _jumpSequence?.Kill();

        _jumpSequence = DOTween.Sequence();

        _jumpSequence
            .Append(_playerTransform.DOScale(new Vector3(1f, 1.2f, 1f), .4f))
            .SetAutoKill(true)
            .SetLink(_playerTransform.gameObject)
            .Play();
    }

    private void FallAnimation() {

        _jumpSequence?.Kill();
        _fallSequence?.Kill();

        _fallSequence = DOTween.Sequence();

        _fallSequence
            .Append(_playerTransform.DOScale(new Vector3(1f, 0.8f, 1f), .5f))
            .SetAutoKill(true)
            .SetLink(_playerTransform.gameObject)
            .Play();
    }

    private void StartGame() {
        _isGameStart = true;
    }

    private void OnDisable() {
        Events.IsGameStart -= StartGame;
        Events.IsPlayerHited -= PlayerIsHited;
    }

    private void PlayerIsHited() {
        _playerRb.velocity = new Vector2(0, _playerRb.velocity.y);
        _isGameStart = false;
        _playerIsHited = true;
    }
}