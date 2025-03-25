using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Fade : MonoBehaviour
{

    public bool isMenu = false;
    public static float FadeTime = 0.75f;
    private SpriteRenderer _sprite;
    [HideInInspector]
    public float alpha = 1f;
    private float targetAlpha = 0f;
    
    public void Start() 
    { 

        if ((isMenu ? MenuButtons._fade : GameManager._fade) != this) { 
            Destroy(gameObject); 
            return;
        }
        DontDestroyOnLoad(gameObject); 
        _sprite = GetComponent<SpriteRenderer>();
        SceneManager.sceneLoaded += FadeIn;
    }

    public void FadeOut() {

        _sprite.enabled = true;
        alpha = 0f;
        targetAlpha = 1f;
    }

    public void FadeIn(Scene scene, LoadSceneMode mode) {

        alpha = 1f;
        targetAlpha = 0f;

    }

    public bool IsFading()
    { 
        if (_sprite != null)
        { return _sprite.enabled; }
        return false;
    }

    public bool IsNotFading() { 
        if (_sprite != null)
        { return !_sprite.enabled; }
        return true; 
    }

    public void Update() {

        if (!_sprite.enabled)
        { return; }

        alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.unscaledDeltaTime / FadeTime);
        
        _sprite.color = new(_sprite.color.r, _sprite.color.b, _sprite.color.g, alpha);

        if (alpha <= 0)
        { _sprite.enabled = false; }

    }

}
