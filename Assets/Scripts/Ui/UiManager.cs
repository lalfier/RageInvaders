using UnityEngine;
using Zenject;
using System.Collections.Generic;

public enum UiTypes
{
    PlayingUi,
    GameOverUi,
    MainMenuUi,
    ScoresUi
}

public class UiManager : MonoBehaviour
{
    [SerializeField]
    List<UiScreen> screenUiList;

    SignalBus _signalBus;
    GameObject _rowPrefab;

    [Inject]
    public void Construct(SignalBus signalBus, AssetRefs assetRefs)
    {
        _signalBus = signalBus;
        _rowPrefab = assetRefs.highScoreRow;
    }

    public GameObject GetRowPrefab()
    {
        return _rowPrefab;
    }

    public void OnStartButtonClicked()
    {
        _signalBus.Fire<StartButtonSignal>();
    }

    public void OnMenuButtonClicked()
    {
        _signalBus.Fire<MenuButtonSignal>();
    }

    public void OnScoresButtonClicked()
    {
        _signalBus.Fire<ScoresButtonSignal>();
    }

    public UiScreen ActivateUiPanel(UiTypes type)
    {
        // Set active and return Ui screen (disable all others)
        foreach (UiScreen uiPanel in screenUiList)
        {
            uiPanel.gameObject.SetActive(false);
        }
        screenUiList[((int)type)].gameObject.SetActive(true);
        return screenUiList[((int)type)];
    }
}
