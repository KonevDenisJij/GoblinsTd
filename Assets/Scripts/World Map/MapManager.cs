using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    private int _numberOfPoints;
    [SerializeField]
    private EventPoint _pointPrefab;
    [SerializeField]
    private Transform _pointsParent;
    [Space(20f)]
    [SerializeField]
    private EventLine _linePrefab;
    [SerializeField]
    private Transform _linesParent;
    [Space(20f)]
    [SerializeField]
    private float _duplicatesRadius;

    private WorldMapGenerator _mapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        _mapGenerator = new WorldMapGenerator(_numberOfPoints, _pointPrefab, _pointsParent, _linePrefab, _linesParent, _duplicatesRadius);

        var data = SaveSystem.LoadWorldMap();
        if (data != null)
        {
            _mapGenerator.LoadMap(data);
        }
        else
        {
            _mapGenerator.GenerateMap();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mapGenerator.HandleTouch();
        }
    }

    private void OnDrawGizmos() 
    {
        if (_mapGenerator == null)
        {
            return;
        }

        _mapGenerator.OnDrawGizmos();
    }
}