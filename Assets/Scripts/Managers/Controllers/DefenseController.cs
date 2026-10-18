using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenseController : MonoBehaviour
{
    [SerializeField]
    private NewGameBoard _board;
    public NewGameBoard Board => _board;

    [SerializeField]
    private GameTileFactory _tileContentFactory;

    public AbilityManager AbilityManager => _abilityManager;
    private AbilityManager _abilityManager;
    public AmmunitionManager AmmunitionManager => _ammunitionManager;
    private AmmunitionManager _ammunitionManager;
    public CreateObjectManager CreateManager => _createManager;
    private CreateObjectManager _createManager;
    public EnemyManager EnemyManager => _enemyManager;
    private EnemyManager _enemyManager;
    public ItemManager ItemManager => _itemManager;
    private ItemManager _itemManager;
    public MaterialsManager MaterialsManager => _materialsManager;
    private MaterialsManager _materialsManager;
    public PlayerManager PlayerManager => _playerManager;
    private PlayerManager _playerManager;
    public TowerManager TowerManager => _towerManager;
    private TowerManager _towerManager;
    public InteractableObjectManager InteractableObjectManager => _interactableObjectManager;
    private InteractableObjectManager _interactableObjectManager;
    public WaveManager WaveManager => _waveManager;
    private WaveManager _waveManager;

    private Charecteristic _charecteristicUi => GUI.Instance.Charecteristic;

    private bool _allowEnemySpawn;

    public static DefenseController Instance => _instance;
    private static DefenseController _instance;

    private GameTile2 _clickedObject;
    private float _clickTime = 0;
    private float _clickDelay = 0.5f;

    public bool Active => _active;
    private bool _active;

    public static Ray TouchRay
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return default;
            };

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    public void StartState()
    {
        _active = true;
        _instance = this;
        _abilityManager = GetComponentInChildren<AbilityManager>();
        _ammunitionManager = GetComponentInChildren<AmmunitionManager>();
        _createManager = GetComponentInChildren<CreateObjectManager>();
        _enemyManager = GetComponentInChildren<EnemyManager>();
        _itemManager = GetComponentInChildren<ItemManager>();
        _materialsManager = GetComponentInChildren<MaterialsManager>();
        _playerManager = GetComponentInChildren<PlayerManager>();
        _towerManager = GetComponentInChildren<TowerManager>();
        _interactableObjectManager = GetComponentInChildren<InteractableObjectManager>();
        _waveManager = GetComponentInChildren<WaveManager>();

        _board.Init();
        _abilityManager.GameAwake();
        _ammunitionManager.GameAwake();
        _createManager.GameAwake();
        _enemyManager.GameAwake();
        _itemManager.GameAwake();
        _playerManager.GameAwake();
        _towerManager.GameAwake();
        _interactableObjectManager.GameAwake();
        _waveManager.GameAwake();
    }

    public void StopState()
    {
        _active = false;
    }

    void OnEnable()
    {
        _instance = this;
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

        /*if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPath();
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _allowEnemySpawn = !_allowEnemySpawn;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _createManager.SelectTower(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _createManager.SelectTower(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _createManager.SelectTower(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _createManager.SelectTower(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _createManager.SelectTower(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _createManager.SelectTower(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _createManager.SelectTower(7);
        }

        GameUpdate();
    }

    private void HandleTouch()
    {
        var tile = _board.GetTile(TouchRay);
        var enemy = _board.GetEnemy(TouchRay);

        if (_clickedObject != null && _clickedObject.Content != null && tile == _clickedObject && Time.time - _clickTime < _clickDelay)
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

    private void HandleDoubleClick(GameTile2 tile)
    {
        if (tile != null)
        {
            _charecteristicUi.OpenTab();
        }
    }

    private void HandleSingleClick(GameTile2 tile, Enemy enemy)
    {
        if (_createManager.PlaceItem(tile ?? (enemy?.CurrentTile ?? null)))
        {
            return;
        }
        if (tile != null)
        {
            if (_createManager.PlaceTower(tile))
            {
                return;
            }
            ObjectSelection.SelectObject(tile.Content);
        }
        else if (enemy != null)
        {
            ObjectSelection.SelectObject(enemy);
            _charecteristicUi.OpenTab();
        }
    }

    private void GameUpdate()
    {
        _createManager.GameUpdate();
        _abilityManager.GameUpdate();
        _ammunitionManager.GameUpdate();
        _waveManager.GameUpdate();
        _enemyManager.GameUpdate();
        _itemManager.GameUpdate();
        _playerManager.GameUpdate();
        _towerManager.GameUpdate();

        /*if (_allowEnemySpawn)
        {
            WaveManager.Instance.UpdateWaves();
        }*/
    }
}
