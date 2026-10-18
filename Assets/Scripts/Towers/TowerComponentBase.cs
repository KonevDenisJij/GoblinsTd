using UnityEngine;

public abstract class TowerComponentBase : GameComponentBase
{
    public Tower Tower => _tower;
    protected Tower _tower;

    public bool IsActive => _isActive;
    protected bool _isActive = true;

    public abstract void UpgradeTower();
    public abstract void UpdateCharacteristics();

    public virtual void ChangeDamage(float damage)
    {
        Debug.Log("Хуйня какая-то :(((");
    }

    public virtual void StopActivity()
    {
        _isActive = false;
    }

    public virtual void StartActivity()
    {
        _isActive = true;
    }
}
