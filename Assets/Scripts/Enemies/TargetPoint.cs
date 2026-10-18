using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    const int enemyLayerMask = 1 << 7;
    public Enemy Enemy { get; private set; }

    public Vector3 Position => transform.position;

    private static Collider[] _buffer = new Collider[100];

    public static int BufferedCount { get; private set; }
    public static TargetPoint RandomBuffered => GetBuffered(Random.Range(0, BufferedCount));

    public static bool FillBuffer(Vector3 position, float range)
    {
        Vector3 top = new Vector3(position.x, 3f, position.z);
        position.y = -1f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(
            position, top, range, _buffer, enemyLayerMask
        );
        return BufferedCount > 0;
    }

    public static TargetPoint GetBuffered(int index)
    {
        var target = _buffer[index].GetComponent<TargetPoint>();
        return target;
    }
    void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();

        Debug.Assert(Enemy != null, 
            "Target point without Enemy root!", 
            this); 
        Debug.Assert(GetComponent<SphereCollider>() != null,
            "Target point without sphere collider!",
            this);
        Debug.Assert(gameObject.layer == 7, 
            "Target point on wrong layer!", 
            this);
    }
}