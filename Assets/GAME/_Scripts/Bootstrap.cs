using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour {
    // === ДАННЫЕ ===
    [SerializeField] private DataPlayer _dataPlayer;
    public DataPlayer DataPlayer => _dataPlayer;

    [SerializeField] private DataPrefabs _dataPrefs;
    public DataPrefabs DataPrefs => _dataPrefs;

    [SerializeField] private DataUI _dataUI;
    public DataUI DataUI => _dataUI;

    [SerializeField] private DataUIEndGame _dataUIEndGame;
    public DataUIEndGame DataUIEndGame => _dataUIEndGame;

    // === ЭКЗЕМПЛЯРЫ КЛАССОВ ===


    // === ИХ СВОЙСТВА ===


    // === ПЕРЕМЕННЫЕ ===
    private bool _gameIsRunning = false;
    private List<Action> _actionsOnUpdate = new List<Action>();
    private List<Action> _actionsOnFixedUpdate = new List<Action>();
    private List<Action> _actionsOnDisable = new List<Action>();
    private List<Action> _actionsOnRectTransform = new List<Action>();
    private List<Action> _actionsOnGizmos = new List<Action>();

    // === ЖИЗНЕННЫЙ ЦИКЛ ===
    #region LIFE CYCLES

    private void OnRectTransformDimensionsChange() {
        if (!_gameIsRunning) return;
        foreach (var action in _actionsOnRectTransform) {
            action?.Invoke();
        }
    }

    private void Awake() {
        GameIsReadyToRun(false);
        StartCoroutine(Loader());
    }

    private void Update() {
        if (!_gameIsRunning) return;
        foreach(var action in _actionsOnUpdate) {
            action?.Invoke();
        }
    }

    private void FixedUpdate() {
        if (!_gameIsRunning) return;
        foreach (var action in _actionsOnFixedUpdate) {
            action?.Invoke();
        }
    }

    private void OnEnable() {

    }

    private void OnDisable() {
        foreach (var action in _actionsOnDisable) {
            action?.Invoke();
        }
    }

    private void OnDrawGizmos() {
        
        foreach (var action in _actionsOnGizmos) {
            action?.Invoke();
        }
        //Vector2 checkPos = _dataPlayer.PlayerTransform.position + Vector3.down * 0.15f;
        //Gizmos.DrawWireSphere(checkPos, 0.2f);
    }

    #endregion

    // === МЕТОДЫ ===
    #region METHODS

    private IEnumerator Loader() {
        var data = StartCoroutine(LoaderData());
        var progressbar = StartCoroutine(LoaderProgressbar());
        yield return data;
        yield return progressbar;
        GameIsReadyToRun(true);
    }

    private IEnumerator LoaderProgressbar() {
        float progress = 0f;
        while (progress <= 1) {
            progress += Time.deltaTime / .4f;
            //_dataUIProgress.Progressbar.value = progress;
            yield return null;
        }
    }

    private IEnumerator LoaderData() {
        yield return LoadHeavyData(() => { new GameStates(this); });
        yield return LoadHeavyData(() => { new SaveLoadSystem(this); });
        yield return LoadHeavyData(() => { new PlayerController(this); });
        yield return LoadHeavyData(() => { new CameraController(this); });
        yield return LoadHeavyData(() => { new PlatformSpawner(this); });
        yield return LoadHeavyData(() => { new GameOver(this); });
        yield return LoadHeavyData(()=> { new ScoreSystem(this); });
    }

    private IEnumerator LoadHeavyData(Action action) {
        action?.Invoke();
        yield return null;
    }

    private void GameIsReadyToRun(bool value) {
        _gameIsRunning = value;
        if (value) {
            //_dataUIProgress.Progressbar.gameObject.SetActive(false);
            // ЗАГРУЗИЛОСЬ
        }
    }

    public void AddActionToUpdate(Action action) {
        if (action != null) _actionsOnUpdate.Add(action);
    }

    public void AddActionToFixedUpdate(Action action) {
        if (action != null) _actionsOnFixedUpdate.Add(action);
    }

    public void AddActionToDisable(Action action) {
        if (action != null) _actionsOnDisable.Add(action);
    }

    public void AddActionToRectTransform(Action action) {
        if (action != null) _actionsOnRectTransform.Add(action);
    }

    public void AddActionToGizmos(Action action) {
        if (action != null) _actionsOnGizmos.Add(action);
    }

    #endregion
}