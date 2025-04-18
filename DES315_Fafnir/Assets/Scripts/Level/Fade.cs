using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Fade : MonoBehaviour
{

    public static Fade instance;
    public static float FadeTime = 0.75f;
    private static SpriteRenderer _sprite;
    [HideInInspector]
    public static float alpha = 1f;
    private static float targetAlpha = 0f;
    
    public void Start() 
    { 

        if (instance == null)
        { instance = this; }
        if (instance != this) { 
            Destroy(gameObject); 
            return;
        }
        DontDestroyOnLoad(gameObject); 
        _sprite = GetComponent<SpriteRenderer>();
        SceneManager.sceneLoaded += FadeIn;
    }

    // Fade to black
    public void FadeOut() {

        _sprite.enabled = true;
        alpha = 0f;
        targetAlpha = 1f;
    }

    // Fade into the scene
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

    // Update the alpha value of the fade box
    public void Update() {

        if (!_sprite.enabled)
        { return; }

        alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.unscaledDeltaTime / FadeTime);
        
        _sprite.color = new(_sprite.color.r, _sprite.color.b, _sprite.color.g, alpha);

        if (alpha <= 0)
        { _sprite.enabled = false; }

    }

}
