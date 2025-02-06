using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeReference] private GameObject security;

    public Material closedMaterial;
    public Material openMaterial;
    private bool cameraOn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cameraOn)
        {
            Debug.Log("You're'nt winner!");
                   
        }
    }
    // Update is called once per frame
    void Update()
    {
     

    }
}
