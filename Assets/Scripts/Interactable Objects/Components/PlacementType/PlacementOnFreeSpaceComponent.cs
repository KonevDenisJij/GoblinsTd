using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementOnFreeSpaceComponent : InteractableObjectPlacementComponentBase
{
    private InteractableObject _iObject;

    public PlacementOnFreeSpaceComponent(InteractableObject iObject)
    {
        _iObject = iObject;
    }


    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }



    public override GameTile PlaceObject(List<GameTile> map, System.Random rand)
    {
        var mapSize = map.Count;

        var randPos = rand.Next(mapSize);

        while (map[randPos] is WalkableTile || map[randPos].Occupied)
        {
            randPos = rand.Next(mapSize);
        }

        _iObject.transform.position = map[randPos].transform.position;

        return map[randPos];
    }
}
