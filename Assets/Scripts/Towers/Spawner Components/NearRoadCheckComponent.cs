using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NearRoadCheckComponent : PositionCheckComponentBase
{
    public NearRoadCheckComponent(Tower tower)
    {
        _tower = tower;
    }

    public override bool CheckPosition(GameTile2 tile)
    {
        for (int i = 0; i < 4; i++)
        {
            int x = 0;
            int z = 0;

            switch (i)
            {
                case 0:
                    x = 1;
                    break;
                case 1:
                    x = -1;
                    break;
                case 2:
                    z = 1;
                    break;
                case 3:
                    z = -1;
                    break;
            }

            var colliders = Physics.OverlapCapsule(
                   new Vector3(tile.transform.position.x + x, 0f, tile.transform.position.z + z),
                   new Vector3(tile.transform.position.x + x, 3f, tile.transform.position.z + z),
                   0.125f);

            if (colliders.Any(a => a.gameObject.GetComponentInParent<WalkableTile>()) || tile is WalkableTile)
            {
                _tile = colliders.First(a => a.gameObject.GetComponentInParent<WalkableTile>()).GetComponentInParent<GameTileContent>();
                return true;
            }
        }

        return false;
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }

    public override void UpdateCharacteristics()
    {
    }

    public override void UpgradeTower()
    {
    }
}
