using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : IPauseHandler
{
    private List<IPauseHandler> _handlers = new List<IPauseHandler> ();

    public static PauseManager Instance => _instance;
    private static PauseManager _instance;

    private PauseManager() { }

    public bool IsPaused => IsPaused;
    private bool _isPaused;

    public static PauseManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PauseManager();
        }
        return _instance;
    }

    public void Register(IPauseHandler newHanlder)
    {
        _handlers.Add(newHanlder);
    }

    public void UnRegister(IPauseHandler oldHandler)
    {
        _handlers.Remove(oldHandler);
    }

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;
        foreach(var handler in _handlers)
        {
            handler.SetPaused(_isPaused);
        }
    }
}
