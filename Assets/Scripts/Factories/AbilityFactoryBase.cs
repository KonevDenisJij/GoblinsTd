using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityFactoryBase : GameObjectFactory
{
    public void Reclaim(AbilityBase content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        if (content.TowerParent != null)
        {
            content.TowerParent.Abilities.Where(a => a == content).Select(a => a = null);
            content.TowerParent = null;
        }
        /*else if (content.HeroParent != null)
        {
            content.HeroParent.Abilities = null;
            content.HeroParent = null;
        }*/
        content = null;
    }
}
