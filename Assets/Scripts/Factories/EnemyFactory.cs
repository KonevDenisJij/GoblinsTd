using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Factories/EnemyFactory")]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    private Enemy _smallEnemyPrefab;
    [SerializeField]
    private Enemy _normalEnemyPrefab;
    [SerializeField]
    private Enemy _bigEnemyPrefab;


    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    private FloatRange _pathOffset = new FloatRange(0f);

    public Enemy Get(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Small: return GetSmallEnemy();
            case EnemyType.Normal: return GetNoramlEnemy();
            case EnemyType.Big: return GetBigEnemy();
        }

        Debug.Log("Ошибка!!! Нет такого врага");

        return null;
    }

    private Enemy GetSmallEnemy()
    {
        var instance = CreateGameObjectInstance(_smallEnemyPrefab);
        instance.OriginFactory = this;
        //instance.Initialize(_pathOffset.RandomValueInRange);

        return instance;
    }

    private Enemy GetNoramlEnemy()
    {
        var instance = CreateGameObjectInstance(_normalEnemyPrefab);
        instance.OriginFactory = this;
        //instance.Initialize(_pathOffset.RandomValueInRange);

        return instance;
    }

    private Enemy GetBigEnemy()
    {
        var instance = CreateGameObjectInstance(_bigEnemyPrefab);
        instance.OriginFactory = this;
        //instance.Initialize(_pathOffset.RandomValueInRange);

        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }
}
