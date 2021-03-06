using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer;

    [FMODUnity.EventRef]
    [SerializeField]
    private string bumpEvent;

    private Vector3 lastPos;
    public bool isGrounded;
    public bool isFalling;
    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        isFalling = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GroundCheck());
        CheckFalling();
        lastPos = transform.position;
        if(lastPos.y <= transform.position.y + 0.2f && isGrounded)
        {
            isFalling = false;
        }
    }

    void CheckFalling()
    {
        if (isFalling)
        {
            animator.SetBool("Fallen", true);
        }
    }

    IEnumerator GroundCheck()
    {
        lastPos = transform.position;
        yield return new WaitForSeconds(0.1f);
        if(lastPos == transform.position)
        {
            isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), 0.4f, groundLayer);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(bumpEvent);
        }
        /*if (!isGrounded && isFalling)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                FMODUnity.RuntimeManager.PlayOneShot(bumpPlayerEvent);
                PlayerMovement.Instance.TakeDamageSound();
                LevelManager.Instance.RespawnPlayer();
                Debug.Log("Carrot Hit");
            }
        }*/
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x,transform.position.y - 0.4f), 0.4f);
    }
}
