using UnityEngine;

public class ItemRadiusComponent : ItemComponentBase
{
    private const float numberOfPoints = 60f;
    private int _size;
    private LineRenderer _lineRenderer;
    private Item _item;
    public ItemRadiusComponent(Item instance, float radius, Material lineMaterial)
    {
        instance.Radius = radius;

        _size = (int)numberOfPoints - 1;
        _size++;

        _lineRenderer = instance.gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = lineMaterial;
        _item = instance;

        GameAwake();
    }

    public override void GameAwake()
    {
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.positionCount = _size;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lineRenderer.receiveShadows = false;
        _lineRenderer.loop = true;

        Vector3 pos;
        float theta = 0f;
        for (int i = 0; i < _size; i++)
        {
            theta += (2.0f * Mathf.PI / numberOfPoints);
            float x = _item.Radius * Mathf.Cos(theta);
            float z = _item.Radius * Mathf.Sin(theta);
            pos = new Vector3(x, 0.5f, z);
            _lineRenderer.SetPosition(i, pos);
        }
    }

    public override void GameUpdate()
    {
    }

    public override void ItemUse()
    {
    }
}