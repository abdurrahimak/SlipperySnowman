using System;
public interface ILevelManager
{
    event Action LevelCompleted;
    event Action LevelFailed;
}
