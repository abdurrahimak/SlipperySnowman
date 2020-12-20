using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Level1Manager : BaseLevelManager
{
    public GameObject LevelEnd;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnPlayerTriggerEvent(PlayerController playerController, TriggerType triggerType, Collider collider)
    {
        base.OnPlayerTriggerEvent(playerController, triggerType, collider);
        if (triggerType == TriggerType.Stay && _playerRigidbody.velocity.magnitude < 0.1f && collider.gameObject.tag.Equals("WinnerArea"))
        {
            UnregisterEvents();
            LevelEnd.SetActive(true);
            AudioSourceManager.Instance.PlayWin();
        }
    }
}
