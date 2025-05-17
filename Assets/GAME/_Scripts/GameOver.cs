using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver
{
    private Transform _playerTransform;
    private Camera _camera;
    private Rigidbody2D _playerRB2;

    private RectTransform _uiGamePlay;
    private RectTransform _uiGameEnd;
    private TextMeshProUGUI _textMeshCurrentScoreUI;
    private TextMeshProUGUI _textMeshCurrentScoreUIEndGame;
    private Button _buttonRestart;

    private bool _gameOverOnce = true;

    public GameOver(Bootstrap bootstrap) {
        _playerTransform = bootstrap.DataPlayer.PlayerTransform;
        _playerRB2 = bootstrap.DataPlayer.PlayerRB;
        _camera = Camera.main;
        _uiGamePlay = bootstrap.DataUIEndGame.UIGamePlay;
        _uiGameEnd = bootstrap.DataUIEndGame.UIGameEnd;
        _textMeshCurrentScoreUI = bootstrap.DataUI.HightScoreTextMeshPro;
        _textMeshCurrentScoreUIEndGame = bootstrap.DataUIEndGame.CurrentScoreText;
        _buttonRestart = bootstrap.DataUIEndGame.ButtonRestart;
        _buttonRestart.onClick.AddListener(Restart);
        bootstrap.AddActionToUpdate(OnUpdate);
    }

    private void OnUpdate() {
        float bottomOfCamera = _camera.ScreenToWorldPoint(Vector3.zero).y;
        if (_playerTransform.position.y < bottomOfCamera && _gameOverOnce) {
            _gameOverOnce = false;
            SetCurrenScoreToUIEndGame();
            Debug.Log("Game Over");
            Events.OnIsGameOver();
        }
    }

    private void SetCurrenScoreToUIEndGame() {
        _playerRB2.drag = 2;
        _uiGamePlay.gameObject.SetActive(false);
        var position = _uiGameEnd.position;
        _uiGameEnd.SetParent(null);
        _uiGameEnd.position = position;
        _textMeshCurrentScoreUIEndGame.SetText(_textMeshCurrentScoreUI.text);
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}