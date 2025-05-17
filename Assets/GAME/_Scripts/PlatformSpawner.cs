using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner
{
    private Platform _platformPrefabDefault;
    private Platform _platformPrefabBroken;
    private Platform _platformPrefabMove;
    private Platform _platformPrefabSpring;
    private Platform _platformPrefabEnemy;
    private float _levelWidth = 1.8f;
    private float _minY = 0.5f;
    private float _maxY = 1.5f;
    private float _spawnY = -2f;
    private Camera _camera;
    private Queue<Platform> _platformsDefault = new Queue<Platform>();
    private Queue<Platform> _platformsBroken = new Queue<Platform>();
    private Queue<Platform> _platformsMoved = new Queue<Platform>();
    private Queue<Platform> _platformsSpring = new Queue<Platform>();
    private Queue<Platform> _platformsEnemy = new Queue<Platform>();

    private Queue<Platform> _freePlatform = new Queue<Platform>();

    private PlatformTypes _previousePlatformTypes;

    private bool _isGameRun = false;

    public PlatformSpawner(Bootstrap bootstrap) {
        _camera = Camera.main;
        _platformPrefabDefault = bootstrap.DataPrefs.PlatformDefault;
        _platformPrefabBroken = bootstrap.DataPrefs.PlatformBroken;
        _platformPrefabMove = bootstrap.DataPrefs.PlatformMove;
        _platformPrefabSpring = bootstrap.DataPrefs.PlatformSpring;
        _platformPrefabEnemy = bootstrap.DataPrefs.PlatformEnemy;
        bootstrap.AddActionToUpdate(OnUpdate);
        bootstrap.AddActionToDisable(OnDisable);
        Events.IsGameStart += Start;
        Events.IsGameOver += EndGame;
    }

    private void Start() {
        _isGameRun = true;
        for (int i = 0; i < 10; i++) {
            SpawnPlatform();
        }
    }

    private void OnUpdate() {

        if (!_isGameRun) return;

        if (_camera.transform.position.y + 10 > _spawnY) {
            SpawnPlatform();
        }

        int count = _freePlatform.Count;
        for (int i = 0; i < count; i++) {
            Platform platform = _freePlatform.Peek();
            float bottom = _camera.ScreenToWorldPoint(Vector3.zero).y;
            if (platform.transform.position.y < bottom) {
                ReturnPlatformToQueue(platform);
                _freePlatform.Dequeue();
            } else {
                break;
            }
        }
    }

    private void SpawnPlatform() {
        float x = Random.Range(-_levelWidth, _levelWidth);
        float y = _spawnY + Random.Range(_minY, _maxY);

        PlatformTypes platformType = PlatformTypes.Default;
        var random = Random.Range(0, 100);
        switch (random) {
            case < 5:
                if (_previousePlatformTypes != PlatformTypes.Enemy)
                        platformType = PlatformTypes.Enemy;
                else platformType = PlatformTypes.Default;
                break;
            case < 10:
                platformType = PlatformTypes.Spring;
                break;
            case < 20:
                platformType = PlatformTypes.Moved; // Moved
                break;
            case < 40:
                if (_previousePlatformTypes != PlatformTypes.Broken)
                    platformType = PlatformTypes.Broken; // Broken
                else platformType = PlatformTypes.Default;
                break;
        }

        _previousePlatformTypes = platformType;

        var platform = TakePlatformFromQuere(platformType);
        platform.transform.position = new Vector3(x, y, 0f);
        _freePlatform.Enqueue(platform);
        _spawnY = y;
    }

    private void ReturnPlatformToQueue(Platform platform) {
        switch (platform.PlatformType) {
            case PlatformTypes.Default:
                _platformsDefault.Enqueue(platform);
                break;
            case PlatformTypes.Broken:
                if (platform.gameObject.activeInHierarchy && platform.TryGetComponent<Animator>(out Animator animator)) {
                    animator.speed = 0f;
                    animator.Play("platformBroke", 0, 0f);
                }
                _platformsBroken.Enqueue(platform);
                break;
            case PlatformTypes.Moved:
                _platformsMoved.Enqueue(platform);
                break;
            case PlatformTypes.Spring:
                _platformsSpring.Enqueue(platform);
                break;
            case PlatformTypes.Enemy:
                _platformsEnemy.Enqueue(platform);
                break;
        }
        platform.gameObject.SetActive(false);
    }

    private Platform TakePlatformFromQuere(PlatformTypes platformType) {

        Platform value = null;

        switch (platformType) {
            case PlatformTypes.Default:
                value = TakePlatform(_platformsDefault, _platformPrefabDefault);
                break;
            case PlatformTypes.Broken:
                value = TakePlatform(_platformsBroken , _platformPrefabBroken);
                break;
            case PlatformTypes.Moved:
                value = TakePlatform(_platformsMoved, _platformPrefabMove);
                break;
            case PlatformTypes.Spring:
                value = TakePlatform(_platformsSpring, _platformPrefabSpring);
                break;
            case PlatformTypes.Enemy:
                value = TakePlatform(_platformsEnemy, _platformPrefabEnemy);
                break;
        }
        value.gameObject.SetActive(true);
        return value;
    }

    private Platform TakePlatform(Queue<Platform> quere, Platform prefab) {
        if (quere.Count <= 0) {
            var platrormNew = GameObject.Instantiate(prefab, new Vector3(0, 0, 0f), Quaternion.identity);
            quere.Enqueue(platrormNew);
        }   
        return quere.Dequeue();
    }

    private void EndGame() {
        foreach(var platform in _freePlatform) {
            ReturnPlatformToQueue(platform);
        }
        _freePlatform.Clear();
    }

    private void OnDisable() {
        Events.IsGameStart -= Start;
        Events.IsGameOver -= EndGame;
    }
}