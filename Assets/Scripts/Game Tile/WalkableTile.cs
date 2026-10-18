using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WalkableTile : GameTile
{
    [SerializeField]
    private Transform _arrow;
    [SerializeField]
    private List<SpriteRenderer> _roads;

    private List<GameObject> _arrows;

    public Dictionary<int, Direction> Directions;


    public void SetArrowRotation()
    {
        if (_arrows == null)
        {
            int i = 0;
            _arrows = new List<GameObject>();
            _arrows.Add(_arrow.gameObject);
            foreach (var dir in Directions)
            {
                if (i == 0)
                {
                    _arrow.rotation = SetRotation(dir.Value);
                }
                else
                {
                    var newArrow = Instantiate(_arrow, _arrow.transform.position, SetRotation(dir.Value));
                    newArrow.SetParent(transform);
                    _arrows.Add(newArrow.gameObject);
                }
                i++;
            }
        }
    }

    public void SetRoad(List<Direction> dirs)
    {        
        var ownDirs = Directions.Select(a => a.Value).ToList();
        foreach (var dir in dirs)
        {
            if (dir == Direction.North)
            {
                ownDirs.Add(Direction.South);
            }
            else if (dir == Direction.East)
            {
                ownDirs.Add(Direction.West);
            }
            else if (dir == Direction.South)
            {
                ownDirs.Add(Direction.North);
            }
            else
            {
                ownDirs.Add(Direction.East);
            }
        }

        ownDirs = ownDirs.Distinct().ToList();

        foreach(var dir in ownDirs)
        {
            _roads[(int)dir].gameObject.SetActive(true);
        }        
    }

    public void ToggleArrows(bool showPath)
    {
        _arrows.ForEach(a => a.SetActive(showPath));
    }
    private Quaternion SetRotation(Direction rotation)
    {
        return Quaternion.Euler(90f, (int)rotation * 90f, 0f);
    }
}
