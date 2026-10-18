using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectPlacementComponentBase : InteractableObjectComponentBase
{
    public abstract GameTile PlaceObject(List<GameTile> map, System.Random rand);
}
