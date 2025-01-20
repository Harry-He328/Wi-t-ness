using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player2Controller : MonoBehaviour
{
    private GameObject movePoint;
    public bool isMoving;
    private bool isActivePlayer;
    private Rigidbody rb;
    public bool onGround;
    public CinemachineVirtualCamera vc1;
    public CinemachineVirtualCamera vc2;
    public float jumpForce;
    private CapsuleCollider groundCheckCollider;
    private float alpha1 = 255 / 255f;
    private float alpha2 = 200 / 255f;

    private Vector3 groundCheckPoint;
    public LayerMask groundLayer;
    public float groundCheckDistance;
    private RaycastHit hit;

    private SpriteRenderer _spriteRenderer;
    
    [Header("Turn-Based Movement")]
    public bool turn_based_movement;
    private float elapsedTime = 0.0f;
    private Vector3 targetPosition;
    private float time = 0.5f;
    [Space]
    [Header("Real-Time Movement")]
    public bool real_time_movement;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movePoint = transform.Find("Player2MovePoint").gameObject;
        movePoint.transform.parent = null;
        groundCheckCollider = GetComponent<CapsuleCollider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheckPoint = transform.Find("GroundCheckPoint").position;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        //让相机对准的角色才能移动
        if (CameraController.Instance.activePlayer2)
        {
            Color color = _spriteRenderer.color;
            color.a = alpha1;
            _spriteRenderer.color = color;
            
            if (!onGround)
            {
                Gravity();
            }
            Player2Movement();
            Jump();
            //让移动状态检测最后执行，否则不会移动
            MoveableDetection();
        }
        else
        {
            rb.velocity = Vector3.zero;
            
            Color color = _spriteRenderer.color;
            color.a = alpha2;
            _spriteRenderer.color = color;
        }

        time += 1f * Time.deltaTime;
    }
    
    public void Player2Movement()
    {

        if (turn_based_movement)
        {
            if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) && !isMoving)
            {
                // 计算目标位置
                float moveDistance = 2f;
                time = 1f;
                targetPosition = transform.position + new Vector3(0, 0, -Input.GetAxisRaw("Horizontal") * moveDistance);
                
                if (Input.GetAxisRaw("Horizontal") > 0.1f) transform.localScale = new Vector3(1, 1, 1);
                else if (Input.GetAxisRaw("Horizontal") < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
        
                // 减少行动点数
                //GameManager.Instance.SetActionPoint();
            
                isMoving = true;
                elapsedTime = 0.0f;
            }
        
            if (isMoving)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / time;
                t = Mathf.Clamp01(t);
            
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        
                // 检查是否到达目标位置
                if (t >= 1.0f)
                {
                    isMoving = false;
                    elapsedTime = 0.0f;
                }
            }
        }

        if (real_time_movement)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
 
            // 计算移动量
            float moveAmount = horizontalInput * moveSpeed * Time.deltaTime;
 
            // 更新角色位置
            transform.Translate(moveAmount, 0, 0);
            
            //转向
            if (horizontalInput > 0.1f) transform.localScale = new Vector3(1, 1, 1);
            else if (horizontalInput < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    //判断是否在移动，如果正在移动过程中则不能进行下一次移动，实现每次移动消耗一个行动点数的效果
    public void MoveableDetection()
    {
        isMoving = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                    Input.GetKey(KeyCode.D));
    }

    void Gravity()
    {
        rb.AddForce(-1 * GameManager.Instance.g,0,0,ForceMode.Force);
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            turn_based_movement = false;
            real_time_movement = true;
            onGround = false;
            rb.AddForce(new Vector3(1 * jumpForce, 0,0),ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            turn_based_movement = true;
            real_time_movement = false;
        }
    }
    
    void GroundCheck()
    {
        Debug.DrawRay(groundCheckPoint,Vector3.left,Color.green);
        if (Physics.Raycast(groundCheckPoint, Vector3.left, out hit, groundCheckDistance, groundLayer))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
}



