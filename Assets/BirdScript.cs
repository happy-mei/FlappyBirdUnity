using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength = 10;
    public LogicScript logic;
    public bool birdIsAlive = true;
    // Flap Tail Logic
    public GameObject tail;
    private SpriteRenderer tailRenderer;
    public Sprite[] tailSprites; // Array of tail sprites
    private float resetDelay = 0.2f; // Delay before changing tail back
    private Coroutine tailResetCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody.gravityScale = 2F;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        // Get the SpriteRenderer component of the tail
        if (tail != null)
        {
            tailRenderer = tail.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidBody.velocity = Vector2.up * flapStrength;
            ChangeTailSprite(0); // Change to upward sprite
            // Cancel any existing coroutine before starting a new one
            if (tailResetCoroutine != null)
            {
                StopCoroutine(tailResetCoroutine);
            }
            tailResetCoroutine = StartCoroutine(ResetTailSprite());
        }
    }

    private IEnumerator ResetTailSprite()
    {
        yield return new WaitForSeconds(resetDelay);
        ChangeTailSprite(1); // Change back to default sprite
    }

    private void ChangeTailSprite(int index)
    {
        if (tailRenderer != null && tailSprites.Length > index)
        {
            tailRenderer.sprite = tailSprites[index];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdIsAlive = false;
        logic.gameOver();

        // Stop any ongoing sprite reset when the game is over
        if (tailResetCoroutine != null)
        {
            StopCoroutine(tailResetCoroutine);
        }
    }
}
