using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableObjectManager : DefaultManager
{
    [SerializeField]
    private InteractableObjectFactory _ioFactory;

    public List<InteractableObject> InteractableObjects => _objects;
    private List<InteractableObject> _objects = new List<InteractableObject>();

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        foreach (var tower in _objects)
        {
            tower.GameUpdate();
        }
    }

    public InteractableObject CreateObject(InteractableObjectType type, GameTile2 tile)
    {
        var io = _ioFactory.Get(type);
        io.Init(tile);
        _objects.Add(io);
        return io;
    }

    public void DestroyTower(InteractableObject io)
    {
        GUI.Instance.Charecteristic.CloseTab();
        io.OriginalTile.DestroyContent();
        _objects = _objects.Where(a => a != io).ToList();
        io.OriginFactory.Reclaim(io);
    }
}
