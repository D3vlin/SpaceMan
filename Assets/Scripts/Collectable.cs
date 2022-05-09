using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType{
    healthPotion,
    manaPotion,
    deadPotion,
    money
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    public int value = 1;

    GameObject player;
    SpriteRenderer sprite;
    CircleCollider2D collectableCollider;

    bool hasBeenCollected = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collectableCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Show()
    {
        sprite.enabled = true;
        collectableCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        collectableCollider.enabled = false;
    }

    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;

            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;

            case CollectableType.deadPotion:
                player.GetComponent<PlayerController>().Die();
                break;

            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!hasBeenCollected)
            {
                Collect();
            }
        }
        
    }
}
