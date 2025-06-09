using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Movement Variables
    public float speed;

    public float jumpPower;
    private bool isGrounded;
    #endregion

    private Rigidbody rb;

    private Vector3 velocity;

    private Camera mainCam;

    #region Health Variables
    public float maxHealth;
    public float health;
    public Image[] hearts;
    #endregion

    #region Bullet Variables
    public GameObject bullet;
    public Transform shotPoint;
    public float fireRate;
    float nextShot;
    #endregion

    public float souls;
    public float stamps;
    public float keys;

    public bool canDeliver;
    public bool canShoot;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
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
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // Sets velocity to the direction of the movement input
        velocity = moveInput.normalized * speed;

        if (moveInput != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
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

        #region Rotation
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
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
        health -= damage;

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
    }

    public void Pickup(string item)
    {
        if (item == "soul") souls++;
        else if (item == "stamp") stamps++;
        else if (item == "key") keys++;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Enemy")
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            TakeDamage(enemy.damage);
            Destroy(enemy.gameObject);
        }

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
