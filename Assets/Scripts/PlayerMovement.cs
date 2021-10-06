using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator ac;
    public GameManager GM;
    CapsuleCollider selfCollider;
    Rigidbody rb;

    public float JumpSpeed = 12;

    int laneNumber = 1, lanesCount = 2;

    public float FirstLanePos, LaneDistance, SideSpeed;

    bool isRolling = false;

    Vector3 ccCenterNorm = new Vector3(0, .96f, 0), ccCenterRoll = new Vector3(0, .4f, .85f);
    float ccHeightNorm = 2, ccHeightRoll = .4f;
    bool wannaJump = false;
    Vector3 startPosition;
    void Start()
    {
       
        
        selfCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, Physics.gravity.y * 4, 0), ForceMode.Acceleration);
        if (wannaJump && isGrounded())
        {
            ac.SetTrigger("jumping");
            rb.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }

    }


    void Update()
    {
        if (isGrounded())
        {
            ac.ResetTrigger("fallin");
            if (GM.CanPlay)
            {
                if (!isRolling)
                {

                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        wannaJump = true;
                    }
                    else if (Input.GetAxisRaw("Vertical") < 0)
                        StartCoroutine(DoRoll());


                }

            }

        }
        else if (rb.velocity.y < -8)
        {
            ac.SetTrigger("fallin");

        }
            CheckInput();
        

     
        Vector3 newPos = transform.position;
        newPos.z = Mathf.Lerp(newPos.z, FirstLanePos + (laneNumber * LaneDistance), Time.deltaTime * SideSpeed);

        transform.position = newPos;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.05f);
    }


    void CheckInput()
    {
        int sign = 0;
        if (!GM.CanPlay || isRolling) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            sign = -1;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            sign = 1;
        else return;

        laneNumber += sign;
        laneNumber = Mathf.Clamp(laneNumber, 0, lanesCount);

    }
    IEnumerator DoRoll() {
        isRolling = true;
        ac.SetBool("rolling", true);
        selfCollider.center = ccCenterRoll;
        selfCollider.height = ccHeightRoll;

        yield return new WaitForSeconds(1.5f);
               
        ac.SetBool("rolling", false);
        selfCollider.center = ccCenterNorm;
        selfCollider.height = ccHeightNorm;
        yield return new WaitForSeconds(.3f);
        isRolling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((!collision.gameObject.CompareTag("Trap") && !collision.gameObject.CompareTag("DeathPlane")) || !GM.CanPlay)
                return;
        StartCoroutine(Death());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin")) return;
        GM.AddCoins(1);
        Destroy(other.gameObject);
    }



    IEnumerator Death()
    {
        GM.CanPlay = false;
        ac.SetTrigger("death");
        yield return new WaitForSeconds(2);
        ac.ResetTrigger("death");
        GM.ShowResult();

    }

    public void ResetPosition()
    {

        transform.position = startPosition;
        laneNumber = 1;

    }
}
