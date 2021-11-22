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
    List<UiScreen> ScreenUiList;

    SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
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
        foreach (var uiPanel in ScreenUiList)
        {
            uiPanel.gameObject.SetActive(false);
        }
        ScreenUiList[((int)type)].gameObject.SetActive(true);
        return ScreenUiList[((int)type)];
    }
}
