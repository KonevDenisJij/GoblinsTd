using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PathHelper
{
    public static float[] CheckMapSize(float[] size, WalkableTile tile)
    {
        if (tile.transform.position.z > size[0])
        {
            size[0] = tile.transform.position.z;
        }

        if (tile.transform.position.x > size[1])
        {
            size[1] = tile.transform.position.x;
        }

        if (tile.transform.position.z < size[2])
        {
            size[2] = tile.transform.position.z;
        }

        if (tile.transform.position.x < size[3])
        {
            size[3] = tile.transform.position.x;
        }

        return size;
    }

    public static int CheckCollision(float x, float z, int bufferSize, out Collider[] buffer)
    {
        buffer = new Collider[bufferSize];

        Physics.SyncTransforms();

        return Physics.OverlapCapsuleNonAlloc(
                   new Vector3(x, 0f, z),
                   new Vector3(x, 3f, z),
                   0.125f,
                   buffer);
    }
}