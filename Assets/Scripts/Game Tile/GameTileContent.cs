using UnityEngine;

[SelectionBase]
public abstract class GameTileContent : MonoBehaviour, ISelectableObject
{
    [SerializeField]
    private GameTileContentType _type;

    public int Price => _price;
    [SerializeField]
    protected int _price;

    public GameTile2 OriginalTile { get => _originalTile; set => _originalTile = value; }
    protected GameTile2 _originalTile;

    public int PathWeight => _pathWeight;
    [SerializeField]
    protected int _pathWeight = 5;

    string ISelectableObject.Name => _name;
    [SerializeField]
    protected string _name;

    public virtual GameTileContentType Type => _type;
    public virtual CustomStringBuilder Characteristics => _characteristics;
    public CustomStringBuilder _characteristics = new CustomStringBuilder();
    public string Description => _description;
    [SerializeField]
    protected string _description;

    public GameObjectFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    private GameObjectFactory _originFactory;

    public virtual void GameUpdate()
    {
    }

    public virtual void Recycle()
    {
        _originFactory.Reclaim(this);
    }
}