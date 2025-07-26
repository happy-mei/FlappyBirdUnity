using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private BirdControls controls; // Input system controls

    private void Awake()
    {
        Debug.Log("Awake");
        controls = new BirdControls();
        controls.Gameplay.Flap.performed += ctx => OnFlap();
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.Gameplay.Enable();
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Gameplay.Disable();
        }
    }

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
        // Check if the bird is within screen bounds
        float margin = Camera.main.orthographicSize * 0.2f; // 20% of screen height
        float topBound = Camera.main.orthographicSize + margin;
        float bottomBound = -Camera.main.orthographicSize - margin;

        float birdY = transform.position.y;
        if (birdY < bottomBound || birdY > topBound)
        {
            birdIsAlive = false;
            logic.gameOver();
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && birdIsAlive)
    //     {
    //         myRigidBody.velocity = Vector2.up * flapStrength;
    //         ChangeTailSprite(0); // Change to upward sprite
    //         // Cancel any existing coroutine before starting a new one
    //         if (tailResetCoroutine != null)
    //         {
    //             StopCoroutine(tailResetCoroutine);
    //         }
    //         tailResetCoroutine = StartCoroutine(ResetTailSprite());
    //     }
    // }

    private void OnFlap()
    {
        Debug.Log("onflap called");

        if (!birdIsAlive) return;

        myRigidBody.velocity = Vector2.up * flapStrength;
        ChangeTailSprite(0);

        if (tailResetCoroutine != null)
        {
            StopCoroutine(tailResetCoroutine);
        }
        tailResetCoroutine = StartCoroutine(ResetTailSprite());
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
