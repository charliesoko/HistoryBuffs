using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    private AudioSource musicSource;
    public float menuVolume;
    public float battleVolume;

    public static AudioHandler instance;

    public static AudioHandler GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        musicSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == ("Start"))
        {
            musicSource.volume = menuVolume;
        }
        else if (SceneManager.GetActiveScene().name == ("Battle"))
        {
            musicSource.volume = battleVolume;
        }
    }
}
