using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Explosion : WarEntity
{
    [SerializeField, Range(0f, 1f)]
    private float _duration = 0.5f;
    [SerializeField]
    private AnimationCurve _opacityCurve = default;
    [SerializeField]
    private AnimationCurve _scaleCurve = default;

    private static int _colorPropertyID = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;

    private float _age, _scale;
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        Debug.Assert(_meshRenderer != null, "Explosion without renderer!");
    }

    public void Initialize(Vector3 position, float blastRadius, float damage, Tower fatherTower)
    {
        TargetPoint.FillBuffer(position, blastRadius);
        for (int i = 0; i < TargetPoint.BufferedCount; i++)
        {
            TargetPoint.GetBuffered(i)?.Enemy.ApplyDamage(damage, fatherTower);
        }
        transform.localPosition = position;
        _scale = 2f * blastRadius;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        if (_age >= _duration)
        {
            return false;
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
        float t = _age / _duration;
        Color c = Color.clear;
        c.a = _opacityCurve.Evaluate(t);
        _propertyBlock.SetColor(_colorPropertyID, c);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
        transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));
        return true;
    }
}