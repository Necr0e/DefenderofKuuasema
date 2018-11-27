using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //References to components
    Rigidbody2D rb2d;
    [Header("Prefabs")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject deathSprite;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip playerFireSound;
    [SerializeField] AudioClip playerMoveSound;

    //Speed & movement settings
    float deltaX;
    float deltaY;
    float accel = 3f;
    float lockPos = 0f;
    float rotationSpeed = 300f;
    //Player stats
    [Header("Player Stats")]
    public int health = 300;
    public int lives = 3;
    //Firing components
    Coroutine firingCoroutine;
    float firingDelay = 0.3f;
    float projectileVelocty = 10f;
    public Vector3 movement;
    public bool isInvul = false;

    public Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;
    float screenWidth;
    float screenHeight;
    public bool isVisible = true;
    AudioSource audioSrc;
    GameManager gm;

    void Start ()
    {
        //Get components
        rb2d = GetComponent<Rigidbody2D>();
        renderers = GetComponentsInChildren<Renderer>();
        audioSrc = GetComponent<AudioSource>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

        var cam = Camera.main;
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1,1,transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.x - screenBottomLeft.x;

    }

	void Update ()
    {
        ScreenWrap();
        Fire();
    }

    void FixedUpdate()
    {
        Move();
    }

    //Move character
    //TODO: make player change directions when tapping.
    void Move()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        deltaY = Input.GetAxisRaw("Vertical");
        movement = new Vector3(deltaX,deltaY, transform.rotation.eulerAngles.z);
        audioSrc.clip = playerMoveSound;

        //turn ship depending on x axis
        //TODO: hitting a border causing rotation

        if(deltaX == -1)
        {
            audioSrc.Play();
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(lockPos,lockPos,90),rotationSpeed * Time.deltaTime);
            projectileVelocty = -10f;
        }
        else if (deltaX == 1)
        {
            audioSrc.Play();
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(lockPos,lockPos,-90),rotationSpeed * Time.deltaTime);
            projectileVelocty = 10f;
        }

        rb2d.AddForce(movement * accel);

    }
    //Hold key to fire in rapid succession
    void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            audioSrc.PlayOneShot(playerFireSound);
            firingCoroutine = StartCoroutine(RepeatFire());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator RepeatFire()
    {
        while(true)
        {

            GameObject laserProjectile = Instantiate(laserPrefab,transform.position,Quaternion.AngleAxis(90,Vector3.forward)) as GameObject;
            laserProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileVelocty,0);

            yield return new WaitForSeconds(firingDelay);
        }
    }

    // Take damage if colliding with component damagedealer
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageTaken = other.gameObject.GetComponent<DamageDealer>();
        if(!damageTaken)
        {
            return;
        }
        ProcessHit(damageTaken);
    }

    //Calculate damage and remove life.
    private void ProcessHit(DamageDealer damageTaken)
    {
        //If player is not invulnerable, take damage.
        if(!isInvul)
        {
            health -= damageTaken.GetDamage();
            if(health <= 0 && lives >= 0)
            {
                lives = lives - 1;
                StartCoroutine(SetInvul());
                StartCoroutine(Respawn());
            }
        }

    }

    //Respawn after death
    IEnumerator Respawn()
    {
        audioSrc.PlayOneShot(playerDeathSound);
        GetComponent<Renderer>().enabled = false;
        RemoveLifePip();
        GameObject playerDeath = Instantiate(deathSprite,transform.position,Quaternion.identity);
        transform.position = new Vector2(0,0);

        health = 300;
        yield return new WaitForSeconds(0.5f);
        Destroy(playerDeath);
        GetComponent<Renderer>().enabled = true;
    }

    void RemoveLifePip()
    {
        if(lives == 2)
        {
            GameObject life = GameObject.Find("Life 3");
            Destroy(life);
        }
        if(lives == 1)
        {
            GameObject life = GameObject.Find("Life 2");
            Destroy(life);
        }
        if(lives == 0)
        {
            GameObject life = GameObject.Find("Life 1");
            Destroy(life);
            Destroy(gameObject);
            //Go to game over screen
            gm.GameOver();
        }
    }

    //Make player invulnerable after dying.
    IEnumerator SetInvul()
    {
        isInvul = true;
        yield return new WaitForSeconds(2);
        isInvul = false;
    }

    //TODO: bug. sometimes player goes through wrapping
    //wrap code
    void ScreenWrap()
    {
        foreach(var renderer in renderers)
        {
            if(renderer.isVisible)
            {
                isWrappingX = false;
                isWrappingY = false;
                return;
            }
        }

        if(isWrappingX && isWrappingY)
        {
            return;
        }

        var newPosition = transform.position;
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);

        if(!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            isVisible = false;
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }

        if(!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;
    }

}
