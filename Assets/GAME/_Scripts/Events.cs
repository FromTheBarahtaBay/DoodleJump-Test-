using System;

public class Events
{
    public static event Action IsGameStart;
    public static void OnIsGameStart() => IsGameStart?.Invoke();

    public static event Action IsGameOver;
    public static void OnIsGameOver() => IsGameOver?.Invoke();

    public static event Action IsPlayerHited;
    public static void OnIsPlayerHited() => IsPlayerHited?.Invoke();
}