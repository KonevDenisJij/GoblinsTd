using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : DefaultManager
{
    private List<Wave> _waves = new List<Wave>(); //сколько спавн поинтов столько и волн

    public static WaveManager Instance => _instance;
    private static WaveManager _instance;


    public void StartWaves()
    {
        foreach(var wave in _waves)
        {
            wave.StartWave();
        }
    }

    public void UpdateWaves()
    {
        foreach(var wave in _waves)
        {
            wave.Update();
        }
    }

    public override void GameUpdate()
    {
        UpdateWaves();
    }

    public override void GameAwake()
    {
        _waves.Add(new Wave());
        StartWaves();
    }
}
