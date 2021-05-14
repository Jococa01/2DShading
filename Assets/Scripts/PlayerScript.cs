using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerScript : MonoBehaviour
{
    private Player PlayerInputs;
    public float speed = 3f;
    public Rigidbody2D rb;
    public RoomChanger RoomChanger;

    public GameObject Proyectile;
    public bool canshoot = true;
    public Vector2 Direction, AttackDir, lastpos;
    public Animator animator;
    public bool attacking;

    public float dashspeed;
    public bool candash=true;

    public CinemachineVirtualCamera virtualCamera;
    public Volume Volume;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerInputs = new Player();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        PlayerInputs.PlayerMov.Movement.performed += context => Direction = context.ReadValue<Vector2>();
        PlayerInputs.PlayerMov.Movement.canceled += context => lastpos = Direction;
        PlayerInputs.PlayerMov.Movement.canceled += context => Direction = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (attacking == false)
        {
            Vector2 m = new Vector2(Direction.x, Direction.y) * Time.deltaTime;
            transform.Translate(m * speed, Space.World);
        }


        AttackDir = PlayerInputs.PlayerMov.Attack.ReadValue<Vector2>();

        animator.SetFloat("Xdir", Direction.x);
        animator.SetFloat("Ydir", Direction.y);

        animator.SetFloat("IdleX", lastpos.x);
        animator.SetFloat("IdleY", lastpos.y);

        animator.SetFloat("ShootX", AttackDir.x);
        animator.SetFloat("ShootY", AttackDir.y);

        attacking = animator.GetBool("Attack");

        if (Direction == Vector2.zero && attacking==false)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Move", false);
        }
        else if (attacking == true)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
        }
        else if (Direction != Vector2.zero)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Move", true);
        }
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

    public void Change()
    {
        rb.velocity = Vector2.zero;
    }

    public void ShootDir(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("He disparado");
            if (canshoot == true && Direction == Vector2.zero)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("Idle", false);
            }
        }
        if (context.performed && canshoot == true && Direction == Vector2.zero)
        {
            GameObject Clone = Instantiate(Proyectile, transform.position, Quaternion.identity);
            Clone.SetActive(true);
            StartCoroutine(Timepass());
        }
        if (context.canceled)
        {
            Debug.Log("finalizo ataque");
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (candash == true && Direction != Vector2.zero)
            {
                Debug.Log("Dasheo");
                StartCoroutine(Dashdelay());

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Floor")
        {
            int RN = collision.GetComponent<RoomDetection>().RoomNumber;
            RoomChanger.RoomNumber = RN;
        }
    }

    IEnumerator Timepass()
    {
        canshoot = false;
        yield return new WaitForSeconds(.4f);
        canshoot = true;
    }

    IEnumerator Dashdelay()
    {
        candash = false;
        rb.velocity = Vector2.zero;
        rb.velocity = Direction * dashspeed;
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2;
        yield return new WaitForSeconds(.1f);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        candash = true;
    }
}
