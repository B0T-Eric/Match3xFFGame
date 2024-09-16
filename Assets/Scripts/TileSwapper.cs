using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public class TileSwapper
{
    [SerializeField, Range(0.1f, 10f)]
    float duration = 0.25f;
    [SerializeField, Range(0f, 1f)]
    float maxDepthOffset = 0.5f;

    Tile tileA, tileB;
    Vector3 positionA, positionB;
    float progress = -1f;
    bool pingPong;

    public float Swap (Tile a, Tile b, bool pingPong)
    {
        tileA = a;
        tileB = b;
        positionA = a.transform.localPosition;
        positionB = b.transform.localPosition;
        this.pingPong = pingPong;
        progress = 0f;
        return pingPong ? 2f * duration : progress;
    }
    public void Update()
    {
        if (progress < 0f)
        {
            return;
        }
        progress += Time.deltaTime;
        if(progress >= duration)
        {
            if (pingPong)
            {
                progress -= duration;
                pingPong = false;
                (tileA,tileB) = (tileA,tileB);
            }
            else
            {
                progress = -1f;
                tileA.transform.localPosition = positionB;
                tileB.transform.localPosition = positionA;
                return;
            }
        }
        //interpolate tiles
        float t = progress / duration;
        float z = Mathf.Sin(Mathf.PI * t) * maxDepthOffset;
        Vector3 p = Vector3.Lerp(positionA, positionB, t);
        p.z = -z;
        tileA.transform.localPosition = p;
        p = Vector3.Lerp(positionA, positionB, 1f - t);
        p.z = z;
        tileB.transform.localPosition = p;


    }
}
