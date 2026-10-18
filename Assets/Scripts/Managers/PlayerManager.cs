using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : DefaultManager
{
    public static PlayerManager Instance => _instance;
    private static PlayerManager _instance;
    public float PlayerGold => _playerGold;
    [SerializeField]
    private float _playerGold = 1000;
    public float PlayerHealth => _playerHealth;
    [SerializeField]
    private float _playerHealth = 30;
    public float HumanResource => _humanResource;
    [SerializeField]
    private float _humanResource = 3;
    public float MaxHumanResource => _maxHumanResource;
    [SerializeField]
    private float _maxHumanResource = 3;

    void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }

    public void ChangeHealth(int value)
    {
        _playerHealth += value;
        GUI.Instance.UpdateHealth();
    }

    public void ChangeGold(int value)
    {
        _playerGold += value;
        GUI.Instance.UpdateGold();
    }

    public void ChangeHumanResource(int value)
    {
        _humanResource += value;
        GUI.Instance.UpdateHumanResource();
    }

    public void ChangeMaxHumanResource(int value)
    {
        _maxHumanResource += value;
        GUI.Instance.UpdateHumanResource();
    }
}
