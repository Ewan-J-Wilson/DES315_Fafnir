using UnityEngine;
// used to make it so the boat sways in the water 
public enum Dir // direction of movement 
{
    X,
    Y,
}

public class SineMove : MonoBehaviour
{
    // basis for sine wive (amplitude, frequency and phase)
    private Vector3 StartPos;
    public float Amp;
    public float Freq;
    private float Phase;
    public Dir dir; 
    // Start is called before the first frame update
    void Start()
    {
        //Store starting position to retain a point of reference
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Phase += Time.deltaTime;
        //Do Sine movement on dir based on direction in script
        if (dir == Dir.X)
        {
            transform.position = new Vector3(StartPos.x + ((Amp * Mathf.Sin(Phase * Freq))), StartPos.y, StartPos.z);
        }
        else
        {
            transform.position = new Vector3(StartPos.x, StartPos.y + ((Amp * Mathf.Sin(Phase * Freq))), StartPos.z);
        }
    }
}
