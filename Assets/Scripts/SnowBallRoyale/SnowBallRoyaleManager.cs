using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowBallRoyaleManager : BaseLevelManager
{
    [Space]
    public int AICount;
    public Vector3 MinSpawnPoint;
    public Vector3 MaxSpawnPoint;
    public LevelEnd LevelEnd;

    [Header("Prefabs")]
    public GameObject AIPrefab;

    [Header("UI")]
    public Slider HealthSlider;
    public Text HealthText;

    private List<PlayerController> AIControllers;
    private Dictionary<PlayerController, float> _playerHealts;
    void Start()
    {
        Initialize();
        AIControllers = new List<PlayerController>(AICount);
        _playerHealts = new Dictionary<PlayerController, float>();
        _playerHealts.Add(PlayerController, 100f);
        SetHealthText(100f);
        for (int i = 0; i < AICount; i++)
        {
            GameObject aiObject = GameObject.Instantiate(AIPrefab);
            aiObject.name = $"AI_{i}";
            aiObject.transform.position = new Vector3(Random.Range(MinSpawnPoint.x, MaxSpawnPoint.x), 1.6f, Random.Range(MinSpawnPoint.z, MaxSpawnPoint.z));
            PlayerController playerController = aiObject.GetComponent<PlayerController>();
            RegisterAIController(playerController);
            AIControllers.Add(playerController);
            _playerHealts.Add(playerController, 100f);
        }
    }

    private void OnAIPlayerCollisionEvent(PlayerController playerController, TriggerType triggerType, Collision collision)
    {
        if (triggerType == TriggerType.Enter)
        {
            if (collision.gameObject.tag.Equals("SnowBall"))
            {
                PlayerGetDamage(playerController);
            }
        }
    }

    protected override void OnPlayerCollisionEvent(PlayerController playerController, TriggerType triggerType, Collision collision)
    {
        base.OnPlayerCollisionEvent(playerController, triggerType, collision);
        if (triggerType == TriggerType.Enter)
        {
            if (collision.gameObject.tag.Equals("SnowBall"))
            {
                PlayerGetDamage(playerController);
            }
        }
    }

    protected override void OnPlayerSnowBallCreated(PlayerController playerController, GameObject snowBallGO)
    {

    }

    private void PlayerGetDamage(PlayerController playerController)
    {
        if (_playerHealts.ContainsKey(playerController))
        {
            float health = UnityEngine.Random.Range(5f, 15f);
            _playerHealts[playerController] -= health;
            if (playerController == PlayerController)
            {
                SetHealthText(_playerHealts[playerController]);
            }

            if (_playerHealts[playerController] < float.Epsilon)
            {
                if (playerController == PlayerController)
                {
                    PlayerDead();
                }
                else
                {
                    AIDead(playerController);
                }
            }
        }
    }

    private void SetHealthText(float health)
    {
        int h = (int)Mathf.Clamp(health, 0f, 100f);
        HealthSlider.value = h;
        HealthText.text = h.ToString();
    }

    private void PlayerDead()
    {
        GameEnd(false);
    }

    private void AIDead(PlayerController playerController)
    {
        UnregisterAIController(playerController);
        _playerHealts.Remove(playerController);
        AIControllers.Remove(playerController);
        GameObject.Destroy(playerController.gameObject);
        if (AIControllers.Count == 0)
        {
            GameEnd(true);
        }
    }

    private void GameEnd(bool playerWin)
    {
        UnregisterEvents();
        foreach (var aiController in AIControllers)
        {
            UnregisterAIController(aiController);
        }

        LevelEnd.gameObject.SetActive(true);
        if (playerWin)
        {
            AudioSourceManager.Instance.PlayWin();
        }
        else
        {
            AudioSourceManager.Instance.PlayLose();
        }
        LevelEnd.Text.text = playerWin ? "YOU ARE WIN !" : "GAME OVER";
    }

    private void RegisterAIController(PlayerController playerController)
    {
        playerController.CollisionEvent += OnAIPlayerCollisionEvent;
    }

    private void UnregisterAIController(PlayerController playerController)
    {
        playerController.CollisionEvent -= OnAIPlayerCollisionEvent;
    }
}
