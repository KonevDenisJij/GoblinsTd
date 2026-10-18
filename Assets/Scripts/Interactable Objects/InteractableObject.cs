using System.Collections.Generic;

public class InteractableObject : GameTileContent
{
    public List<InteractableObjectComponentBase> Components { get => _components; set => _components = value; }
    private List<InteractableObjectComponentBase> _components;

    public void Init(GameTile2 tile)
    {
        _originalTile = tile;
        _originalTile.CreateContent(this);
        transform.position = tile.transform.position;
    }
}