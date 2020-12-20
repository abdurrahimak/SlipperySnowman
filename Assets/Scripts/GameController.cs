using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameDataManager
{
    public bool IsSoundActive;
    public int Level;

    public GameDataManager()
    {
        Level = GetLevel();
        IsSoundActive = GetSound();
    }

    public bool GetSound()
    {
        IsSoundActive = Convert.ToBoolean(PlayerPrefs.GetInt("Sound", 1));
        return IsSoundActive;
    }

    public void SetSound(bool soundActive)
    {
        IsSoundActive = soundActive;
        PlayerPrefs.SetInt("Sound", soundActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetLevel(int level)
    {
        Level = level;
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
    }

    public int GetLevel()
    {
        Level = PlayerPrefs.GetInt("Level", 1);
        return Level;
    }
}

public class GameController : MonoBehaviour
{
    private string currentSceneName = "Main";

    #region Singleton

    private static GameController _instance;
    public static GameController Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(this);
            _instance = this;
            StartGameController();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private Toggle _soundToggle;
    public Toggle SoundToggle
    {
        get => _soundToggle;
        set
        {
            _soundToggle = value;
            _soundToggle.isOn = GameDataManager.GetSound();
            _soundToggle.onValueChanged.AddListener(SoundChange);
        }
    }

    public GameDataManager GameDataManager;

    void StartGameController()
    {
        GameDataManager = new GameDataManager();
    }

    private void Start()
    {
        SoundChange(GameDataManager.GetSound());
    }

    void Update()
    {

    }

    internal void OpenMainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    internal void ReOpenLevel()
    {
        OpenScene(currentSceneName);
    }

    public void OpenScene(string sceneName)
    {
        currentSceneName = sceneName;
        SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
    }

    public void SoundChange(bool value)
    {
        Camera.main.GetComponent<AudioListener>().enabled = value;
        GameDataManager.SetSound(value);
    }

    public void OpenLevel(int level)
    {
        GameDataManager.SetLevel(level);
        currentSceneName = $"Level{level}";
        OpenScene(currentSceneName);
    }

    internal void OpenNextLevel()
    {
        int level = GameDataManager.GetLevel() + 1;
        if (level % 4 == 0)
        {
            level = 1;
        }
        OpenLevel(level);
    }
}
