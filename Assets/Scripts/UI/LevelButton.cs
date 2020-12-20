using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int Level = 1;
    public string LevelName;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (String.IsNullOrEmpty(LevelName))
            {
                GameController.Instance.OpenLevel(Level);
            }
            else
            {
                GameController.Instance.OpenScene(LevelName);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
