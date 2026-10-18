using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoosterAbilityComponent : AbilityComponentBase
{
    private TowerComponentBase _attackComponent;
    private float _damageBoost;
    private float _attackBoostTime;
    private float _attackDelay;
    private float _abilitieRechargeTime;

    private float _timer = 0;
    private bool _abilityUsed = false;
    private bool _damageBoostActive = false;
    private bool _abilityActive = false;

    private Material _boostMaterial;
    private Material _normalMaterial;

    public DamageBoosterAbilityComponent(TowerComponentBase attackComponent, float damageBoost, float attackBoostTime, float attackDelay, float abilitieRechargeTime)
    {
        _attackComponent = attackComponent;
        _damageBoost = damageBoost;
        _attackBoostTime = attackBoostTime;
        _attackDelay = attackDelay;
        _abilitieRechargeTime = abilitieRechargeTime;
    }

    public DamageBoosterAbilityComponent(TowerComponentBase attackComponent, float damageBoost, float attackBoostTime, float attackDelay, float abilitieRechargeTime, Material boostMaterial, Material normalMaterial)
    {
        _attackComponent = attackComponent;
        _damageBoost = damageBoost;
        _attackBoostTime = attackBoostTime;
        _attackDelay = attackDelay;
        _abilitieRechargeTime = abilitieRechargeTime;

        _boostMaterial = boostMaterial;
        _normalMaterial = normalMaterial;
    }

    public override void AbilityUse()
    {
        if (_abilityUsed)
        {
            Debug.Log("Способность на кд");
            return;
        }
        
        if (_boostMaterial != null)
        {
            Debug.Log("ХУЙ");
            _attackComponent
                .Tower.transform
                .GetChild(3).gameObject
                .GetComponent<MeshRenderer>()
                .material = _boostMaterial;
        }

        Debug.Log("aa");

        _attackComponent.ChangeDamage(_damageBoost);
        _abilityActive = true;
        _damageBoostActive = true;
        _timer = 0;
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        if (_abilityUsed)
        {
            _timer += Time.deltaTime;
            if (_timer > _abilitieRechargeTime)
            {
                _abilityUsed = false;
                _abilityActive = false;
                _timer = 0;
            }
        }
        else if (_abilityActive)
        {
            _timer += Time.deltaTime;

            if (_timer > _attackBoostTime && _damageBoostActive)
            {
                _attackComponent.ChangeDamage(-_damageBoost);
                _damageBoostActive = false;
                _attackComponent.StopActivity();
                _timer = 0;
                return;
            }

            if (_timer > _attackDelay)
            {
                if (_normalMaterial != null)
                {
                    _attackComponent
                        .Tower.transform
                        .GetChild(3).gameObject
                        .GetComponent<MeshRenderer>()
                        .material = _normalMaterial;
                }

                _attackComponent.StartActivity();
                _abilityActive = false;
                _abilityUsed = true;
                _timer = 0;
            }
        }
    }
}
