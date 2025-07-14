using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Movement Variables
    [Header("Movement")]
    public float speed;
    public float sprintSpeed;
    public float rotationSpeed = 0.1f;
    private float rotationVelocity;

    public float jumpPower;
    private bool isGrounded;
    #endregion

    private Rigidbody rb;

    private Vector3 velocity;

    private Transform mainCam;

    #region Health Variables
    [Header("Health")]
    public float maxHealth;
    public float health;
    public Image[] hearts;
    private PlayerDamageEffects damageEffects;
    public float invulnerabilityTime;
    private bool isInvulnerable;
    #endregion

    #region Bullet Variables
    [Header("Bullet")]
    public GameObject bullet;
    public Transform shotPoint;
    public float fireRate;
    float nextShot;
    #endregion

    [Header("Items")]
    public float souls;
    public float stamps;
    public float keys;

    public bool canDeliver;
    public bool canShoot;

    public Animator animator;

    private Vector3 spawnPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main.GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        spawnPos = GameObject.FindGameObjectWithTag("Start").transform.position;

        transform.position = new Vector3(spawnPos.x, 5, spawnPos.z);

        damageEffects = GetComponent<PlayerDamageEffects>();
    }

    void Update()
    {
        Movement();
        
        if (canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Moves the player depending on velocity
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Movement()
    {
        if (transform.position.y < -100)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #region Movement

        // Gets the movement input directions
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // Movement directions relative to the camera, sets y to zero to avoid vertical rotation
        Vector3 moveDir = mainCam.forward * direction.z + mainCam.right * direction.x;
        moveDir.y = 0f;

        // Rotates the player towards the cameras forward direction
        Vector3 camFoward = mainCam.forward;
        camFoward.y = 0f;
        transform.rotation = Quaternion.LookRotation(camFoward);

        if (direction != Vector3.zero)
        {
            animator.SetBool("isWalking", true);

            // Sets velocity to the direction of the movement input
            if (Input.GetKey(KeyCode.LeftShift)) // If left shift is being held down, the player sprints
            {
                velocity = moveDir.normalized * sprintSpeed;
            }
            else
            {
                velocity = moveDir.normalized * speed;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);

            // Sets velocity to zero
            velocity = Vector3.zero;
        }
        #endregion

        #region Jumping
        // Checks if the player has pressed space and is on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Applies vertical velocity to the player(other velocities are the same)
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
            isGrounded = false;
        }
        
        // Checks if the player lets go of space
        if (Input.GetButtonUp("Jump"))
        {
            // Halves vertical velocity making the jump cut short
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
        }
        #endregion
    }

    void Shoot()
    {
        if (Time.time > nextShot)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(bullet, shotPoint.position, shotPoint.rotation);
                nextShot = Time.time + fireRate;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            health -= damage;
            damageEffects.TakeDamageEffects();

            // Loops through each item in the hearts array
            for (int i = 0; i < hearts.Length; i++)
            {
                // Clamps the value of the current health minus the current index between 0 and 1
                // E.g: Health = 1.5, first iteration 1.5-0 = 0 which clamped is 1 so the first heart is full
                // second iteration 1.5-1 = 0.5 so clamped to 0.5 meaning the second heart is half full
                float heartAmount = Mathf.Clamp(health - i, 0f, 1f);

                // Sets the fill amount of the current heart
                hearts[i].fillAmount = heartAmount;
            }

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            StartCoroutine(InvulnerabilityCountdown());
        }
    }

    private IEnumerator InvulnerabilityCountdown()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false;
    }

    public void Pickup(string item)
    {
        if (item == "soul") souls++;
        else if (item == "stamp") stamps++;
        else if (item == "key") keys++;
    }

    void OnTriggerEnter(Collider obj)
    {
        /*if (obj.tag == "Enemy")
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            TakeDamage(enemy.damage);
        }*/

        if (obj.tag == "DeliveryRoom")
        {
            canDeliver = true;
        }

        if (obj.tag == "Exit")
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "DeliveryRoom")
        {
            canDeliver = false;
        }
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
