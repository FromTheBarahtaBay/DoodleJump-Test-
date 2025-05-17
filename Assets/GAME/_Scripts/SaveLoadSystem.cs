using TMPro;
using UnityEngine;

public class SaveLoadSystem
{
    private float _recordScore = 0;
    private TextMeshProUGUI _currentScoreText;
    private TextMeshProUGUI _recordScoreText;

    public SaveLoadSystem(Bootstrap bootstrap) {
        Events.IsGameOver += SaveData;
        _currentScoreText = bootstrap.DataUI.HightScoreTextMeshPro;
        _recordScoreText = bootstrap.DataUIEndGame.RecordScoreText;
        bootstrap.AddActionToDisable(OnDisable);
        LoadData();
    }

    private void LoadData() {
        if (PlayerPrefs.HasKey("recordScore")) {
            _recordScore = PlayerPrefs.GetInt("recordScore");
            _recordScoreText.SetText($"{_recordScore}");
        }
    }

    private void SaveData() {
        if (int.TryParse(_currentScoreText.text, out int result)) {
            if (result > _recordScore) {
                PlayerPrefs.SetInt("recordScore", result);
            }
        }
    }

    private void OnDisable() {
        Events.IsGameOver -= SaveData;
    }
}