using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    float disappearDuration = 0.25f;

    [Serializable]
    struct FallingState
    {
        public float fromY, toY, duration, progress;
    }

    FallingState falling;
    [SerializeField]
    SpriteRenderer enhancementRenderer;
    Enhancement enhancement;
    PrefabInstancePool<Tile> pool;
    float disappearProgress;
    public Tile Spawn (Vector3 position)
    {
        Tile instance = pool.GetInstance(this);
        instance.pool = pool;
        //instance.transform.SetParent(GameObject.Find("GridBack").transform);
        instance.transform.localPosition = position;
        instance.transform.localScale = Vector3.one;
        instance.disappearProgress = -1f;
        instance.falling.progress = -1f;
        instance.enabled = false;
        instance.enhancement = EnhancementHandler.TryGetEnhancement();
        if (instance.enhancement != null)
        {
            instance.enhancementRenderer.sprite = instance.enhancement.GetSprite();
            if (enhancement is IOnSpawnAdd)
            {
                enhancement.ApplyEnhancementEffect();
            }
            if (enhancement is IOnSpawnRemove)
            {
                enhancement.RemoveEnhancementEffect();
            }
        }
        else
        {
            enhancementRenderer.gameObject.SetActive(false);
        }
        return instance;
    }
    public void Despawn()
    {
        if(enhancement != null)
        {
            if(enhancement is IOnClearAdd && enhancement != null)
            {
                enhancement.ApplyEnhancementEffect();
            }
            if(enhancement is IOnClearRemove && enhancement != null)
            {
                enhancement.RemoveEnhancementEffect();
            }
        }
        pool.Recycle(this);
    }
    public float Disappear()
    {
        disappearProgress = 0f;
        enabled = true;
        return disappearDuration;
    }
    private void Update()
    {
        if(disappearProgress >= 0f) 
        {
            disappearProgress += Time.deltaTime;
            if(disappearProgress >= disappearDuration)
            {
                Despawn();
                return;
            }
            transform.localScale = Vector3.one * (1f-disappearProgress/disappearDuration);
        }
        if (falling.progress >= 0f)
        {
            Vector3 position = transform.localPosition;
            falling.progress += Time.deltaTime;
            if (falling.progress >= falling.duration)
            {
                falling.progress = -1f;
                position.y = falling.toY;
                enabled = disappearProgress >= 0f;
            }
            else
            {
                position.y = Mathf.Lerp(falling.fromY, falling.toY, falling.progress / falling.duration);
            }
            transform.localPosition = position;
        }
    }
    public float Fall(float toY, float speed)
    {
        falling.fromY = transform.localPosition.y;
        falling.toY = toY;
        falling.duration = (falling.fromY - toY) / speed;
        falling.progress = 0f;
        enabled = true;
        return falling.duration;
    }
}
