using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataUIEndGame
{
    [field: SerializeField] public RectTransform UIGamePlay { get; private set; }
    [field: SerializeField] public RectTransform UIGameEnd { get; private set; }
    [field: SerializeField] public TextMeshProUGUI RecordScoreText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CurrentScoreText { get; private set; }
    [field: SerializeField] public Button ButtonRestart { get; private set; }
}