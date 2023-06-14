using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private bool _isPause;

    public void OnTogglePause()
    {
        _isPause = GameManager.Instance.State == GameState.Pause;
        _isPause = !_isPause;
        SetActivePause(_isPause);
    }

    private void SetActivePause(bool isPause)
    {
        if (isPause)
        {
            GameManager.Instance.UpdateGameState(GameState.Pause);
            return;
        }
        GameManager.Instance.TogglePausing(isPause);
        UIManager.Instance.ActivePausePanel(isPause);
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void OnResetButton()
    {
        _isPause = !_isPause;
        SetActivePause(false);
        GameManager.Instance.UpdateGameState(GameState.SelectToPlay);
    }
}
