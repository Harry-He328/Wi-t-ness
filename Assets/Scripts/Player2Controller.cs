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
    public float g = 9.8f;
    public float jumpForce;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        //让相机对准的角色才能移动
        if (vc1.Priority < vc2.Priority)
        {
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
        }

        time += 1f * Time.deltaTime;
    }
    
    public void Player2Movement()
    {
        //这个方法和一般的跳跃的方法兼容不了，有点烂
        // transform.position =
        //     Vector3.MoveTowards(transform.position, movePoint.transform.position,
        //         moveSpeed * Time.deltaTime);
   
        //这段可以让物体瞬移一格
        // if (Vector3.Distance(transform.position, movePoint.transform.position) <= .05f && !isMoving)
        // {
        //     if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        //     {
        //         //设置目标地点
        //         movePoint.transform.position += new Vector3( 0f, 0f,-Input.GetAxisRaw("Horizontal"));
        //     }
        // }
        if (turn_based_movement)
        {
            if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) && !isMoving && GameManager.Instance.GetActionPoint())
            {
                // 计算目标位置
                float moveDistance = 2f;
                time = 1f;
                
                targetPosition = transform.position + new Vector3(0, 0, -Input.GetAxisRaw("Horizontal") * moveDistance);
     
                // 减少行动点数
                GameManager.Instance.SetActionPoint();
            
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
            transform.Translate(0, 0, -moveAmount);
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
        rb.AddForce(-1 * g,0,0,ForceMode.Force);
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            onGround = false;
            rb.AddForce(new Vector3(1 * jumpForce, 0,0),ForceMode.Impulse);
        }
    }
    
    
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Ground"))
        {
            onGround = true;
        }
    }
}


