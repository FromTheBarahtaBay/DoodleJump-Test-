using TMPro;
using UnityEngine;

public class ScoreSystem
{
    private Transform _playerTransform;
    private float _maxHeight = 0;
    private int _score = 0;
    private TextMeshProUGUI _scoreText;

    public ScoreSystem(Bootstrap bootstrap) {
        _playerTransform = bootstrap.DataPlayer.PlayerTransform;
        _scoreText = bootstrap.DataUI.HightScoreTextMeshPro;
        bootstrap.AddActionToUpdate(UpdateScore);
    }

    private void UpdateScore() {
        if (_playerTransform.position.y > _maxHeight) {
            _maxHeight = _playerTransform.position.y;
            _score = Mathf.FloorToInt(_maxHeight * 10);
            _scoreText.SetText($"{_score}");
        }
    }
}