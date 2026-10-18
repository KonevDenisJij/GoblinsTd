using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManag : MonoBehaviour
{
    /*[SerializeField]
    private Vector2Int _boardSize = new Vector2Int(11, 11);
    [SerializeField]
    private GameBoard _board;
    [SerializeField]
    private GameTileFactory _tileContentFactory;
    [SerializeField]
    private EnemyFactory _enemyFactory;
    [SerializeField]
    private TowerFactory _towerFactory;
    [SerializeField]
    private ItemFactory _itemFactory;
    [SerializeField]
    private WarFactory _warFactory;
    [SerializeField]
    private Material _selectedTowerMaterial;
    public float PlayerGold => _playerGold;
    [SerializeField]
    private float _playerGold = 1000;
    public float PlayerHealth => _playerHealth;
    [SerializeField]
    private float _playerHealth = 30;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _ammunition = new GameBehaviorCollection();

    public List<Tower> Towers => _towers;
    private List<Tower> _towers = new List<Tower>();
    private Tower _selectedTower;
    private Item _selectedItem;

    private Dictionary<int, TowerType> _towerTypes = new Dictionary<int, TowerType>();
    private Dictionary<int, ItemType> _itemTypes = new Dictionary<int, ItemType>();

    private Charecteristic _charecteristicUi => GUI.Instance.Charecteristic;

    [DefaultValue(false)]
    private bool _allowEnemySpawn;

    private static GameManag _instance;
    public static GameManag Instance => _instance;

    public PauseManager _pauseManager;
    private bool _isPaused;

    private GameTileContent _clickedObject;
    private float _clickTime = 0;
    private float _clickDelay = 0.5f;

    private Ray TouchRay
    {
        get {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return default;
            };

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }    

    void Awake()
    {
        _pauseManager = PauseManager.GetInstance(); //перенести в контекст проекта потомw
        _pauseManager.Register(this);
        _board.Initialize();
        WaveManager.Initialize(1);
    }

    void OnEnable()
    {
        _instance = this;
        _towerTypes.Add(1, TowerType.Laser);
        _towerTypes.Add(2, TowerType.Mortar);
        _itemTypes.Add(3, ItemType.Bomb);
        _itemTypes.Add(4, ItemType.ColdBomb);
        _towerTypes.Add(5, TowerType.ThornFloor);
        _towerTypes.Add(6, TowerType.Balloon);
        _towerTypes.Add(7, TowerType.KnightsBarrack);

        WaveManager.Instance.StartWaves();
    }

    void OnValidate()
    {
        if (_boardSize.x < 2)
        {
            _boardSize.x = 2;
        }
        if (_boardSize.y < 2)
        {
            _boardSize.y = 2;
        }
    }
    void Update()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPath();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _allowEnemySpawn = !_allowEnemySpawn;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectTower(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectTower(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectTower(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectTower(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectTower(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectTower(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectTower(7);
        }

        if (_selectedTower != null)
        {
            TrackPlacement(_selectedTower);
        }

        if (_selectedItem != null)
        {
            TrackPlacement(_selectedItem);
        }

        GameUpdate();
    }

    public static WarEntity SpawnWarEntity(AmmunitionType type)
    {
        WarEntity warEntity = _instance._warFactory.Get(type);
        _instance._ammunition.Add(warEntity);
        return warEntity;
    }

    public void SelectTower(int number)
    {
        var prevTowerType = _selectedTower?.TowerType;
        var prevItemType = _selectedItem?.ItemType;

        float checkPrice = 0;

        if (_towerTypes.ContainsKey(number))
        {
            checkPrice = _towerFactory.GetTowerPrice(_towerTypes[number]);
        }
        else if (_itemTypes.ContainsKey(number))
        {
            checkPrice = _itemFactory.GetItemPrice(_itemTypes[number]);
        }
        else
        {
            Debug.Log("МДА НАХУЙ");
            return;
        }

        if (_playerGold - checkPrice < 0)
        {
            Debug.Log("Не хватает зоита");
            return;
        }

        if (_selectedTower != null)
        {
            _selectedTower.Recycle();
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
            _selectedTower = _towerFactory.Get(_towerTypes[number]);

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
            _selectedItem = _itemFactory.Get(_itemTypes[number]);
        }
        else
        {
            Debug.Log("МДА НАХУЙ");
            return;
        }
    }

    public void UseAbility(int abilityNumber)
    {
        if (ObjectSelection.SelectedObject is Tower)
        {
            var selectedTower = ObjectSelection.SelectedObject as Tower;
            selectedTower.UseAbility(abilityNumber);
        }
    }

    public void ChangeAbilities()
    {
        if (ObjectSelection.SelectedObject is Tower)
        {
            GUI.Instance.AbilitiesLayout.ChangeIcons((ObjectSelection.SelectedObject as Tower).Abilities.Select(a => a as AbilityBase).ToList());
        }
        else
        {
            GUI.Instance.AbilitiesLayout.ClearIcons();
        }
    }

    private void TrackPlacement(GameTileContent selected)
    {
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(TouchRay, out var position))
        {
            selected.transform.position = TouchRay.GetPoint(position);

            if (_selectedTower != null && _selectedTower.FlyingType)
            {
                _selectedTower.transform.position += new Vector3(0f, 2f, 0f);
            }
        }
    }

    private void HandleTouch()
    {
        GameTileContent tile = _board.GetTile(TouchRay);
        Enemy enemy = _board.GetEnemy(TouchRay);

        if (_clickedObject != null && tile == _clickedObject && Time.time - _clickTime < _clickDelay && (_selectedTower == null || _selectedItem == null))
        {
            _clickTime = 0;
            HandleDoubleClick(tile);
        }
        else
        {
            _clickTime = Time.time;
            _clickedObject = tile;
            HandleSingleClick(tile, enemy);
        }
    }

    private void HandleDoubleClick(GameTileContent tile)
    {
        if (tile != null)
        {
            if (tile is Tower)
            {
                _charecteristicUi.OpenTab();
            }
            else if (tile is InteractableObject)
            {
                _charecteristicUi.OpenTab();
            }
        }
    }

    private async void HandleSingleClick(GameTileContent tile, Enemy enemy)
    {
        if (tile != null)
        {
            if (_selectedTower != null)
            {
                if (tile is Tower || !_selectedTower.CheckPlacement(tile))
                {
                    foreach (var mat in _selectedTower.GetComponentsInChildren<Renderer>())
                    {
                        mat.materials[1].color = Color.red;
                    }
                    await Task.Delay(200);
                    foreach (var mat in _selectedTower.GetComponentsInChildren<Renderer>())
                    {
                        mat.materials[1].color = Color.green;
                    }
                    return;
                }

                _selectedTower.OriginalTile = tile as GameTile;
                _selectedTower.transform.position = tile.transform.position;

                if (_selectedTower.FlyingType)
                {
                    _selectedTower.transform.position += new Vector3(0f, 2f, 0f);
                }

                _playerGold -= _selectedTower.Price;
                GUI.Instance.UpdateGold();

                _selectedTower.InBuildingMode = false;
                _towers.Add(_selectedTower);
                _selectedTower.OriginalTile.Content = _selectedTower;
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
            }
            else if (_selectedItem != null)
            {
                _playerGold -= _selectedItem.Price;
                GUI.Instance.UpdateGold();

                _selectedItem.gameObject.layer = 0;
                _selectedItem.UseItem();
                GUI.Instance.ItemsLayout.RemoveHighlight();
                _selectedItem.Recycle();
                _selectedItem = null;
            }
            else if (tile is Tower)
            {
                ObjectSelection.SelectObject(tile as Tower);
            }
            else if (tile is InteractableObject)
            {
                ObjectSelection.SelectObject(tile as InteractableObject);
            }
        }
        else if (enemy != null)
        {
            ObjectSelection.SelectObject(enemy);
            _charecteristicUi.OpenTab();
        }
    }

    public void SpawnEnemy(EnemyType type, int gateNumber)
    {
        Enemy enemy = _enemyFactory.Get(type);
        enemy.SpawnOn(1);

        _enemies.Add(enemy);
    }

    private void GameUpdate()
    {
        if (_isPaused)
        {
            return;
        }

        if (_allowEnemySpawn)
        {
            WaveManager.Instance.UpdateWaves();
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _towers.ForEach(a => a.GameUpdate());
        _towers = _towers.Where(a => a.TowerHealth > 0f).ToList();
        _selectedItem?.GameUpdate();
        _ammunition.GameUpdate();
    }

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;
    }

    public void ChangeHealth(int value)
    {
        _playerHealth += value;
        GUI.Instance.UpdateHealth();
    }*/
}