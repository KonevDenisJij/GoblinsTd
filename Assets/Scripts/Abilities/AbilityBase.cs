using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBase
{
    public List<AbilityComponentBase> Components { get => _components; set => _components = value; }
    private List<AbilityComponentBase> _components;

    public Tower TowerParent { get => _towerParent; set => _towerParent = value; }
    private Tower _towerParent;

    public Sprite AbilityIcon { get => _abilityIcon; set => _abilityIcon = value; }
    private Sprite _abilityIcon;

    //public Hero HeroParent => { get => _heroParent; set => _heroParent = value; }
    //private Hero _heroParent;

    public AbilityFactoryBase OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    private AbilityFactoryBase _originFactory;

    public virtual void UseAbility()
    {
        _components.ForEach(a => a.AbilityUse());
    }

    public virtual void GameUpdate()
    {
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }
}
