using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameStates
{
    private Sequence _sequence;
    private GameObject _scoreObject;
    private CanvasGroup _buttonsCanvasGroup;
    private Button _startButton;
    private Button _optionButton;
    private Button _exitButton;

    public GameStates(Bootstrap bootstrap) {
        _scoreObject = bootstrap.DataUI.HightScoreObject;
        _buttonsCanvasGroup = bootstrap.DataUI.ButtonsCanvasGroup;
        _startButton = bootstrap.DataUI.ButtonStart;
        _optionButton = bootstrap.DataUI.ButtonOptions;
        _exitButton = bootstrap.DataUI.ButtonExit;

        _startButton.onClick.AddListener(StartGame);
        _exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame() {
        _sequence = DOTween.Sequence();

        _sequence
            .Append(_buttonsCanvasGroup.DOFade(0, 0.5f))
            .AppendCallback(() => { _scoreObject.SetActive(true); })
            .OnComplete(() => { Events.OnIsGameStart(); })
            .SetAutoKill(true)
            .Play();
    }

    private void ExitGame() {
        Application.Quit();
    }
}