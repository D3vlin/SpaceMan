using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection
{
    horizontal,
    vertical
}

public class Enemy : MonoBehaviour
{
    public int enemyDamage = 10;
    public MovementDirection movementDirection = MovementDirection.horizontal;
    public bool facingRight = false;
    public bool facingUp = false;
    public float runningSpeed = 1.5f;

    Rigidbody2D enemyRigidbody;
    AudioSource enemyAudioSource;
    Vector3 startPosition;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        enemyAudioSource = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //enemyAudioSource.Play();
    }

    private void FixedUpdate()
    {
        float currentRunnningSpeed = runningSpeed;

        if (movementDirection == MovementDirection.horizontal)
        {
            enemyRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;

            if (facingRight)
            {
                currentRunnningSpeed = runningSpeed;
                this.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                currentRunnningSpeed = -runningSpeed;
                this.transform.eulerAngles = Vector3.zero;
            }
        }

        if (movementDirection == MovementDirection.vertical)
        {
            enemyRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

            if (facingUp)
            {
                currentRunnningSpeed = runningSpeed;
                this.transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                currentRunnningSpeed = -runningSpeed;
                this.transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (movementDirection == MovementDirection.horizontal)
            {
                enemyRigidbody.velocity = new Vector2(currentRunnningSpeed, enemyRigidbody.velocity.y);
            }

            if (movementDirection == MovementDirection.vertical)
            {
                enemyRigidbody.velocity = new Vector2(enemyRigidbody.velocity.x, currentRunnningSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin" || collision.tag == "ExitZone" || collision.tag == "StartZone")
        {
            return;
        }

        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }

        if (movementDirection == MovementDirection.horizontal)
        {
            facingRight = !facingRight;
            enemyAudioSource.Play();
            return;
        }

        if (movementDirection == MovementDirection.vertical)
        {
            facingUp = !facingUp;
            enemyAudioSource.Play();
        }
    }
}
