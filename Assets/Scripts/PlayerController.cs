using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winImageObject;
    public GameObject finishImage;
    public float jumpForce = 5f; 

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded;  

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winImageObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue jumpValue)
    {
        if (isGrounded)
        {

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SoundEffectManager.playJumpSound();
        }
    }

    void SetCountText()
    {
        countText.text = "Contador: " + count.ToString();
        if (count >= 10)
        {
            winImageObject.SetActive(true);
            SoundEffectManager.playGameWinSound();
            MusicManager.PauseMusic();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(gameObject);
            finishImage.gameObject.SetActive(true);
            SoundEffectManager.playGameLostSound();
            MusicManager.PauseMusic();
        }

        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;  
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Thor"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SoundEffectManager.playGemCollectedSound();
            SetCountText();
        }
    }
}
