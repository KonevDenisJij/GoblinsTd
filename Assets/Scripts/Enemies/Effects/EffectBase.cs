using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public abstract class EffectBase
{
    public float Duration { get => _duration; set => _duration = value; }
    protected float _duration;

    public abstract void GameUpdate(Enemy enemy);
    public abstract void EffectStart(Enemy enemy, float duration);
    public abstract void EffectEnd(Enemy enemy);
}
