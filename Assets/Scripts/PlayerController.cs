using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;

    public GameObject winImageObject;   
    public GameObject finishImage;      
    public GameObject nivelImage;       
    public float jumpForce = 5f;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded;
    private bool gameOverSoundPlayed = false; 
    private GameObject enemigo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winImageObject.SetActive(false);
        finishImage.SetActive(false);
        
        if (nivelImage != null) nivelImage.SetActive(false);

        enemigo = GameObject.FindGameObjectWithTag("Enemigo");
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
        countText.text = "Contador: " + count.ToString() +"/10";

        if (count >= 10 && SceneManager.GetActiveScene().name == "Nivel 2")
        {
            winImageObject.SetActive(true);
            SoundEffectManager.playGameWinSound();
            MusicManager.PauseMusic();
            
            StopEnemyMovement();

            StartCoroutine(EndGameAfterWin());
        }
        else if (count >= 10 && SceneManager.GetActiveScene().name == "Minigame")
        {
            ShowTransitionImageAndChangeScene();
        }
    }

    private IEnumerator EndGameAfterWin()
    {
        yield return new WaitForSeconds(3); 
        Application.Quit(); 
    }

    private void ShowTransitionImageAndChangeScene()
    {
        if (nivelImage != null)
        {
            nivelImage.SetActive(true);

            StartCoroutine(ChangeSceneAfterDelay());
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(3); 
        SceneManager.LoadScene("Nivel 2");  
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
            if (!gameOverSoundPlayed)
            {
                gameOverSoundPlayed = true; 
                finishImage.SetActive(true);
                SoundEffectManager.playGameLostSound();
                MusicManager.PauseMusic();
            }
            
            StartCoroutine(RestartScene());
        }

        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;  
        }
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(3); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
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
            count++; 
            SoundEffectManager.playGemCollectedSound();
            SetCountText(); 
        }
    }
    
    private void StopEnemyMovement()
    {
        if (enemigo != null)
        {
            Rigidbody enemigoRb = enemigo.GetComponent<Rigidbody>();
            if (enemigoRb != null)
            {
                enemigoRb.velocity = Vector3.zero; 
                enemigoRb.angularVelocity = Vector3.zero; 
                enemigoRb.constraints = RigidbodyConstraints.FreezeAll; 
            }

            enemigo.SetActive(false);
        }
    }
}
