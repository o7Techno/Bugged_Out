using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> music;
    public AudioSource source;

    int playID = 1;

    public void PlaySound(string name)
    {
        Sound sound = music.Find(x => x.name == name);

        source.clip = sound.clip;
        source.Play();
    }

    private void Start()
    {
        PlaySound("Theme2");
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            int newID = Random.Range(0, music.Count);
            while (newID == playID)
            {
                newID = Random.Range(0, music.Count);
            }
            playID = newID;
            source.clip = music[newID].clip;
            source.Play();
        }
    }

}
