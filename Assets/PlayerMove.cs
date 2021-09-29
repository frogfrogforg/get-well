using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed;
    public float walkAnimLength;

    public Sprite playerIdle;
    public Sprite playerWalk0;
    public Sprite playerWalk1;

    SpriteRenderer sprite;

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * Time.deltaTime * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * Time.deltaTime * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * Time.deltaTime * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * Time.deltaTime * speed;
            isMoving = true;
        }

        if (isMoving) {
            sprite.sprite = playerWalk0;
            if (Mathf.Repeat(Time.time, walkAnimLength) > (walkAnimLength/2f)) {
                sprite.sprite = playerWalk1;
            }
        } else {
            sprite.sprite = playerIdle;
        }

        
    }
}
