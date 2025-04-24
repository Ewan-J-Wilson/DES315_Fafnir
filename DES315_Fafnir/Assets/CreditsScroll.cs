using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{

    [SerializeField] [Range(1f, 8f)]
    private float startWait = 2f;

    private float startTimer = 0f;

    [SerializeField] [Range(1f, 10f)]
    private float speed = 80f;

    private float distance = 0f;
    private bool doLoad = false;

    private bool increaseSpeed;

    //private bool skipHeld = false;
    //[SerializeField]
    //private float skipCooldown = 1f;
    //private float skipTimer = 0f;


    public void IncreaseAction(InputAction.CallbackContext obj) {

        if (obj.performed)
        { increaseSpeed = true; }

        if (obj.canceled)
        { increaseSpeed = false; }

    }


    // Start is called before the first frame update
    void Start()
    { distance = Mathf.Abs(transform.position.y); }
    

    // Update is called once per frame
    void Update()
    {
        
        //if (skipHeld) 
        //{ skipTimer += Time.deltaTime; }

        if ((startTimer += Time.deltaTime) < startWait) 
        { return; }

        //Debug.Log(transform.position.y);

        transform.position += new Vector3(0, Time.deltaTime * speed * (increaseSpeed ? 4 : 1), 0);
        if (transform.position.y >= distance && !doLoad) { 
            doLoad = true;
            //Time.timeScale = 0;
            FindFirstObjectByType<Fade>().FadeOut();
        }

        if (doLoad && Fade.alpha >= 1) {
            doLoad = false;
            //Time.timeScale = 1;
            SceneManager.LoadSceneAsync("Main Menu");
        }

    }
}
