using System;

public static class GameEvents
{
    public static event Action OnGameStarted;
    public static event Action OnGameOver;
    public static event Action OnPlayerRedirect;
    public static event Action<int> OnWallActivated;

    public static void CallOnGameStarted() => OnGameStarted?.Invoke();
    public static void CallOnGameOver() => OnGameOver?.Invoke();
    public static void CallOnPlayerRedirect() => OnPlayerRedirect?.Invoke();
    public static void CallOnWallActivated(int colorIndex) => OnWallActivated?.Invoke(colorIndex);
}
