using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseLevelManager
{
    public Toggle Toggle;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        GameController.Instance.SoundToggle = Toggle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
