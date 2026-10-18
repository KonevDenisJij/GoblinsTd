using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjectManager : DefaultManager
{
    [SerializeField]
    private Material _selectedTowerMaterial;

    private TowerManager _towerManager => DefenseController.Instance.TowerManager;
    private ItemManager _itemManager => DefenseController.Instance.ItemManager;
    private PlayerManager _playerManager => DefenseController.Instance.PlayerManager;
    private TowerManager _ioManager => DefenseController.Instance.TowerManager;

    private Tower _selectedTower;
    private Item _selectedItem;

    private Dictionary<int, TowerType> _towerTypes = new Dictionary<int, TowerType>();
    private Dictionary<int, ItemType> _itemTypes = new Dictionary<int, ItemType>();

    public static CreateObjectManager Instance => _instance;
    private static CreateObjectManager _instance;

    void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public override void GameAwake()
    {
        _towerTypes.Add(1, TowerType.Laser);
        _towerTypes.Add(2, TowerType.Mortar);
        _itemTypes.Add(3, ItemType.Bomb);
        _itemTypes.Add(4, ItemType.ColdBomb);
        _towerTypes.Add(5, TowerType.ThornFloor);
        _towerTypes.Add(6, TowerType.Balloon);
        _towerTypes.Add(7, TowerType.KnightsBarrack);
    }

    public override void GameUpdate()
    {
        if (_selectedTower != null)
        {
            TrackPlacement(_selectedTower);
        }

        if (_selectedItem != null)
        {
            TrackPlacement(_selectedItem);
        }
    }

    public void SelectTower(int number)
    {
        var prevTowerType = _selectedTower?.TowerType;
        var prevItemType = _selectedItem?.ItemType;

        float checkPrice = 0;

        if (_towerTypes.ContainsKey(number))
        {
            checkPrice = _towerManager.GetTowerPrice(_towerTypes[number]);
        }
        else if (_itemTypes.ContainsKey(number))
        {
            checkPrice = _itemManager.GetItemPrice(_itemTypes[number]);
        }
        else
        {
            Debug.Log("Ãƒ¿ Õ¿’”…");
            return;
        }

        if (_playerManager.PlayerGold - checkPrice < 0)
        {
            Debug.Log("ÕÂ ı‚‡Ú‡ÂÚ ÁÓËÚ‡");
            return;
        }

        if (_selectedTower != null)
        {
            DefenseController.Instance.TowerManager.DestroyTower(_selectedTower);
            _selectedTower = null;
            GUI.Instance.ItemsLayout.RemoveHighlight();
        }
        else if (_selectedItem != null)
        {
            _selectedItem.Recycle();
            _selectedItem = null;
            GUI.Instance.ItemsLayout.RemoveHighlight();
        }

        if (prevTowerType != null && _towerTypes.ContainsKey(number) && prevTowerType == _towerTypes[number])
        {
            return;
        }
        else if (prevItemType != null && _itemTypes.ContainsKey(number) && prevItemType == _itemTypes[number])
        {
            return;
        }

        GUI.Instance.ItemsLayout.HighlightIcon(number);

        if (_towerTypes.ContainsKey(number))
        {
            _selectedTower = _towerManager.GetTower(_towerTypes[number]);

            _selectedTower.InBuildingMode = true;

            foreach (var mat in _selectedTower?.GetComponentsInChildren<MeshRenderer>())
            {
                var newColors = new Material[2];
                newColors[0] = mat.material;
                newColors[1] = _selectedTowerMaterial;
                mat.materials = newColors;

                mat.gameObject.TryGetComponent<Collider>(out var comp);
                if (comp != null)
                {
                    comp.enabled = false;
                }
            }
        }
        else if (_itemTypes.ContainsKey(number))
        {
            _selectedItem = _itemManager.GetItem(_itemTypes[number]);
        }
        else
        {
            Debug.Log("Ãƒ¿ Õ¿’”…");
            return;
        }
    }

    public bool PlaceTower(GameTile2 tile)
    {
        if (_selectedTower == null)
        {
            return false;
        }

        if (tile.Content != null || !_selectedTower.CheckPlacement(tile))
        {
            foreach (var mat in _selectedTower.GetComponentsInChildren<Renderer>())
            {
                mat.materials[1].color = Color.red;
            }
            //await Task.Delay(200);
            foreach (var mat in _selectedTower.GetComponentsInChildren<Renderer>())
            {
                mat.materials[1].color = Color.green;
            }
            Debug.Log(3);
            return true;
        }

        _selectedTower.OriginalTile = tile;
        _selectedTower.transform.position = tile.transform.position;

        if (_selectedTower.FlyingType)
        {
            _selectedTower.transform.position += new Vector3(0f, 2f, 0f);
        }

        _playerManager.ChangeGold(-_selectedTower.Price);

        _selectedTower.InBuildingMode = false;
        _towerManager.AddTower(_selectedTower);
        _selectedTower.OriginalTile.CreateContent(_selectedTower);
        _selectedTower.gameObject.layer = 0;

        foreach (var mat in _selectedTower.GetComponentsInChildren<MeshRenderer>())
        {
            var newColors = new Material[1];
            newColors[0] = mat.material;
            mat.materials = newColors;

            mat.gameObject.TryGetComponent<Collider>(out var comp);
            if (comp != null)
            {
                comp.enabled = true;
            }
        }

        _selectedTower.Components.ForEach(a => a.GameAwake());

        _selectedTower = null;
        GUI.Instance.ItemsLayout.RemoveHighlight();

        DefenseController.Instance.EnemyManager.UpdatePath();

        return true;
    }

    public bool PlaceItem(GameTile2 tile2)
    {
        if (_selectedItem == null || tile2 == null)
        {
            return false;
        }

        _playerManager.ChangeGold(-_selectedItem.Price);

        _selectedItem.gameObject.layer = 0;
        _selectedItem.UseItem();
        GUI.Instance.ItemsLayout.RemoveHighlight();
        _selectedItem.Recycle();
        _selectedItem = null;

        return true;
    }

    private void TrackPlacement(GameTileContent selected)
    {
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(DefenseController.TouchRay, out var position))
        {
            selected.transform.position = DefenseController.TouchRay.GetPoint(position);

            if (_selectedTower != null && _selectedTower.FlyingType)
            {
                _selectedTower.transform.position += new Vector3(0f, 2f, 0f);
            }
        }
    }
}
