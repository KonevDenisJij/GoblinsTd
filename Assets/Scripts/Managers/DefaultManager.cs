using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultManager : MonoBehaviour
{
    public abstract void GameUpdate();
    public abstract void GameAwake();
}
