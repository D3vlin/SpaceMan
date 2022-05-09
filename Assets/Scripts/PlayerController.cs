using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables de movimiento del personaje
    public float jumpForce = 6f;
    public float groundClearanceToJump = 2f;
    public float runningSpeed = 2f;
    
    public LayerMask groundMask;
    Rigidbody2D rigidBody;
    Vector3 startPosition = new Vector3(1, 0, 0);


    //Variables de animación del personaje
    Animator animator;
    const string STATE_ALIVE = "isALive";
    const string STATE_ON_THE_GROUD = "isOnTheGround";

    //Salud y mana
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 0, MIN_MANA = 0;
    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    [SerializeField]
    int healthPoints, manaPoints;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }

    public void StarGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUD, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.2f);
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;

        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump(false);
            }
            if (Input.GetButtonDown("SuperJump"))
            {
                Jump(true);
            }

            rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * runningSpeed, rigidBody.velocity.y);

            if (Input.GetAxis("Horizontal") < 0 && GameManager.sharedInstance.currentGameState == GameState.inGame)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            if (Input.GetAxis("Horizontal") > 0 && GameManager.sharedInstance.currentGameState == GameState.inGame)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }          
        }        

        animator.SetBool(STATE_ON_THE_GROUD, IsTouchingTheGround());

        //Gyzmos
        Debug.DrawRay(this.transform.position, Vector2.down * groundClearanceToJump, Color.green);
    }

    //Especie de reloj, funciona por intervalos fijos y no por frames
    void FixedUpdate()
    {
        if (rigidBody.velocity.x < runningSpeed)
        {
            //rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
        }
    }

    void Jump(bool superJump)
    {
        float jumpForceFactor = jumpForce;

        if (superJump && manaPoints >= SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (IsTouchingTheGround())
            {
                GetComponent<AudioSource>().Play();
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }
        }

    }

    bool IsTouchingTheGround() => Physics2D.Raycast(this.transform.position, Vector2.down, groundClearanceToJump, groundMask);

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxScore", 0);

        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxScore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        this.rigidBody.velocity = Vector2.zero;
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int healthPoints)
    {
        this.healthPoints += healthPoints;
        this.healthPoints = this.healthPoints >= MAX_HEALTH ? MAX_HEALTH : this.healthPoints;
        
        if (this.healthPoints <= MIN_HEALTH)
        {
             Die();
        }
    }

    public void CollectMana(int manaPoints)
    {
        this.manaPoints += manaPoints;
        this.manaPoints = this.manaPoints >= MAX_MANA ? MAX_MANA : this.manaPoints;
    }

    public int GetHealth()
    {
        return this.healthPoints;
    }

    public int GetMana()
    {
        return this.manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
