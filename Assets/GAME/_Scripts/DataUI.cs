using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataUI
{
    [field: SerializeField] public GameObject HightScoreObject { get; private set; }
    [field: SerializeField] public TextMeshProUGUI HightScoreTextMeshPro { get; private set; }
    [field: SerializeField] public CanvasGroup ButtonsCanvasGroup { get; private set; }
    [field: SerializeField] public Button ButtonStart { get; private set; }
    [field: SerializeField] public Button ButtonOptions { get; private set; }
    [field: SerializeField] public Button ButtonExit { get; private set; }
}