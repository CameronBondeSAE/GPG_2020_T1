using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip music;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager.startGameEvent += GameManagerOnstartGameEvent;
    }

    private void GameManagerOnstartGameEvent()
    {
        GetComponent<AudioSource>().clip = music;
        GetComponent<AudioSource>().Play();
    }

}
