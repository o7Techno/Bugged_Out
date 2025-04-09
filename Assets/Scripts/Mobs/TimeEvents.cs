using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEvents : MonoBehaviour
{
    [Range(0, 24)] public float timeOfDay;
    public bool pauseTime;
    MobSpawner spawner;
    public float mobCooldown;
    public GameObject[] mobs;
    bool mobSpawnOnCooldown;

    private void Start()
    {
        spawner = GetComponent<MobSpawner>();
        mobSpawnOnCooldown = false;
    }
    void Update()
    {
        if (Application.isPlaying && !pauseTime)
        {
            timeOfDay += Time.deltaTime/20;
            timeOfDay %= 24;
        }
        if (timeOfDay > 5 && timeOfDay < 6)
        {
            mobSpawnOnCooldown = false;
        }
        if (timeOfDay > 19 || timeOfDay < 5)
        {
            if (!mobSpawnOnCooldown)
            {
                mobSpawnOnCooldown = true;
                SelectAndSpawnMob();
                Invoke(nameof(CooldownChanger), mobCooldown);
            }
        }
    }

    void SelectAndSpawnMob()
    {
        int mobId = Random.Range(0, mobs.Length);
        spawner.SpawnMob(mobs[mobId]);
    }

    void CooldownChanger()
    {
        mobSpawnOnCooldown = false;
    }
}
