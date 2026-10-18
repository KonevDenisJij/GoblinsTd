using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameBehavior, ISelectableObject
{
    public UnitFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    private UnitFactory _originFactory;

    //ISelectableObject
    public string Name => _unitName;
    [SerializeField]
    private string _unitName;

    public CustomStringBuilder Characteristics
    {
        get
        {
            if (_characteristics == null)
            {
                _characteristics = new CustomStringBuilder();
            }
            return _characteristics;
        }
        set => _characteristics = value;
    }
    private CustomStringBuilder _characteristics;

    public string Description => _description;
    [SerializeField]
    private string _description;

    private float _progress, _progressFactor, _pathOffset;
    private GameTile2 _tile;

    public void Initialize(float pathOffset, GameTile2 tile)
    {
        _pathOffset = pathOffset;
        _tile = tile;

        transform.position = tile.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
