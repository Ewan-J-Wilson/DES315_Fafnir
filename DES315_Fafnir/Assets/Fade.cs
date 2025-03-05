using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Fade : MonoBehaviour
{

    public static float FadeTime = 0.75f;
    private SpriteRenderer _sprite;
    [HideInInspector]
    public float alpha = 0f;
    private float targetAlpha = 1f;
    public float alphaThreshold = 1f;
    
    public void Start() 
    { 
        DontDestroyOnLoad(gameObject); 
        _sprite = GetComponent<SpriteRenderer>();
        
    }

    public void FadeOut() {

        _sprite.enabled = true;
        alpha = 0f;
        targetAlpha = 1f;

    }

    public void FadeIn() {

        alpha = 1f;
        targetAlpha = 0f;
        Debug.Log("hit here");

    }

    public void Update() {

        if (!_sprite.enabled)
        { return; }

        alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime / FadeTime);
        
        _sprite.color = new(_sprite.color.r, _sprite.color.b, _sprite.color.g, alpha);
        Debug.Log("Updating" + _sprite.color);

        if (alpha <= 0)
        { _sprite.enabled = false; }

    }

}
