using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime; // * Time.deltaTime ensures the multiplication happens the same, no matter the frame rate. We didn't need it for the velocity code because physics runs on its own little clock, but otherwise we will need it.

        // To free up memory
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
