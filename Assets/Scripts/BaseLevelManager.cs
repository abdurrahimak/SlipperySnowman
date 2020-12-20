using System;
using UnityEngine;

public class BaseLevelManager : MonoBehaviour, ILevelManager
{
    public event Action LevelCompleted;
    public event Action LevelFailed;

    public PlayerController PlayerController;
    protected Rigidbody _playerRigidbody;

    private void Awake()
    {
        gameObject.name = "LevelManager";
    }

    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        var listener = Camera.main.GetComponent<AudioListener>();
        if(listener != null)
        {
            listener.enabled = GameController.Instance.GameDataManager.GetSound();
        }
        _playerRigidbody = PlayerController.GetComponent<Rigidbody>();
        RegisterEvents();
    }

    protected void RegisterEvents()
    {
        PlayerController.CollisionEvent += OnPlayerCollisionEvent;
        PlayerController.TriggerEvent += OnPlayerTriggerEvent;
        PlayerController.SnowBallCreated += OnPlayerSnowBallCreated;
    }

    protected void UnregisterEvents()
    {
        PlayerController.CollisionEvent -= OnPlayerCollisionEvent;
        PlayerController.TriggerEvent -= OnPlayerTriggerEvent;
        PlayerController.SnowBallCreated -= OnPlayerSnowBallCreated;
    }

    protected virtual void OnPlayerSnowBallCreated(PlayerController playerController, GameObject snowBallGO)
    {
        SnowBall snowBall = snowBallGO.GetComponent<SnowBall>();
        if (snowBall)
        {
            snowBall.DestroyInterval(5f);
        }
    }

    protected virtual void OnPlayerCollisionEvent(PlayerController playerController, TriggerType triggerType, Collision collision)
    {
    }

    protected virtual void OnPlayerTriggerEvent(PlayerController playerController, TriggerType triggerType, Collider collider)
    {
    }

    protected void LevelComplete()
    {
        LevelCompleted?.Invoke();
    }

    protected void LevelFail()
    {
        LevelFailed?.Invoke();
    }

    public void ReturnMainMenu()
    {
        GameController.Instance.OpenMainScene();
    }

    public void Retry()
    {
        GameController.Instance.ReOpenLevel();
    }

    public void NextLevel()
    {
        GameController.Instance.OpenNextLevel();
    }
}