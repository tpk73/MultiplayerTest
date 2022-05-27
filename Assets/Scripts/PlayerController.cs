using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] GameObject camHolder;
    [SerializeField] float mouseSens, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    bool IsGrounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    float verticalLookRotation;

    PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!PV.IsMine) 
            return;

            Look();
            Move();
            Jump(); // not working

        }

        void Look()
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSens);
            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSens;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

            camHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }

        void Move()
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);


        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.F) && IsGrounded)
            {
                rb.AddForce(transform.up * jumpForce);
                Debug.Log("Jump");
            }
        }

        public void SetGroundedState(bool _IsGrounded)
        {
            IsGrounded = _IsGrounded;
        }

        void FixedUpdate()
        {
            if (!PV.IsMine)
                return;

                rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
        }
    
}
