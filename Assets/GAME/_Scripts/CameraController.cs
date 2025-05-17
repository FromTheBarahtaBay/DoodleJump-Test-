using System.Collections;
using UnityEngine;

public class CameraController
{
    private Bootstrap _bootstrap;
    private Transform _cameraTransform;
    private Transform _targetTransform;
    private RectTransform _uIGameEndRect;
    private float _highestY;
    private bool _isGamePlayOn = true;
    private bool _endOnce = true;

    public CameraController(Bootstrap bootstrap) {
        _bootstrap = bootstrap;
        _targetTransform = bootstrap.DataPlayer.PlayerTransform;
        _cameraTransform = Camera.main.transform;
        _uIGameEndRect = bootstrap.DataUIEndGame.UIGameEnd;
        bootstrap.AddActionToUpdate(OnUpdate);
        Events.IsGameOver += StartEndEvent;
        bootstrap.AddActionToDisable(OnDisable);
    }

    private void OnUpdate() {
        if (_isGamePlayOn)
            CameraOnPlay();
        else if (_endOnce)
            CameraOnEnd();
    }

    private void CameraOnPlay() {
        if (_targetTransform.position.y > _highestY) {
            _highestY = _targetTransform.position.y;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                new Vector3(_cameraTransform.position.x, _highestY, _cameraTransform.position.z), 3f * Time.deltaTime);
        }
    }

    private void CameraOnEnd() {
        _endOnce = false;
        _bootstrap.StartCoroutine(CoruotineForEndAnimation());
    }

    private IEnumerator CoruotineForEndAnimation() {
        while (_targetTransform.position.y > _uIGameEndRect.anchoredPosition.y) {
            yield return null;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                new Vector3(_cameraTransform.position.x, _targetTransform.position.y, _cameraTransform.position.z), 3f * Time.deltaTime);
        }

        while (Vector3.Distance(_cameraTransform.position, _uIGameEndRect.position) > 0.01f) {
            yield return null;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                new Vector3(_cameraTransform.position.x, _uIGameEndRect.anchoredPosition.y, _cameraTransform.position.z), 3f * Time.deltaTime);
        }

        _cameraTransform.position = new Vector3(_cameraTransform.position.x, _uIGameEndRect.position.y, _cameraTransform.position.z);
    }

    private void StartEndEvent() {
        _isGamePlayOn = false;
    }

    private void OnDisable() {
        Events.IsGameOver -= StartEndEvent;
    }
}