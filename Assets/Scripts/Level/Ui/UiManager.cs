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
        foreach (var uiPanel in screenUiList)
        {
            uiPanel.gameObject.SetActive(false);
        }
        screenUiList[((int)type)].gameObject.SetActive(true);
        return screenUiList[((int)type)];
    }
}
