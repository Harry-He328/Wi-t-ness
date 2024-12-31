
using UnityEngine;
using Cinemachine;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 2f; // 每秒移动的网格数（实际上这是每秒移动多少个网格大小的距离）
    public float gridSize = 1f; // 网格单元的大小
    private Animator anim;
    public bool isMoving;
    public float rotationSpeed;
    private Rigidbody rb;
    private GameObject movePoint;
    private bool isActivePlayer;
    public CinemachineVirtualCamera vc1;
    public CinemachineVirtualCamera vc2;

    private void Start()
    {
        movePoint = transform.Find("Player1MovePoint").gameObject;
        rb = GetComponent<Rigidbody>();
        movePoint.transform.parent = null;
    }

    private void Update()
    {
        //让相机对准的角色才能移动
        if (vc1.Priority > vc2.Priority)
        {
            Player1Movement();
            MoveableDetection();
        }
    }

    public void Player1Movement()
{
    // 角色移动到目标地点
    float step = moveSpeed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, step);
 
    // 检查是否到达目标点
    float distanceToTarget = Vector3.Distance(transform.position, movePoint.transform.position);
    if (distanceToTarget <= 0.05f && !isMoving && GameManager.Instance.GetActionPoint())
    {
        // 重置移动状态
        isMoving = false;
 
        // 根据输入设置新的目标地点
        Vector3 newDirection = Vector3.zero;
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            newDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f) * gridSize;
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            newDirection = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * gridSize);
        }
 
        if (newDirection != Vector3.zero)
        {
            movePoint.transform.position += newDirection;
            GameManager.Instance.SetActionPoint(); // 行动点数-1
            isMoving = true;
        }
    }
    else
    {
        isMoving = distanceToTarget > 0.05f;
    }
 
    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit floorHit;
    if (Physics.Raycast(camRay, out floorHit, LayerMask.GetMask("Ground")))
    {
        // 计算角色到鼠标投影点的向量
        Vector3 playerToMouse = floorHit.point - new Vector3(transform.position.x, 0f, transform.position.z);
        playerToMouse.y = 0f;

        Vector3 direction = playerToMouse.normalized;
 
        // 计算角度
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        angle = Mathf.Round(angle / 90.0f) * 90.0f;
        if (angle == -0f) angle = 0f;
        
        
        if (angle == 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (angle == 90)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (angle == 180 || angle == -180)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if(angle == 270 || angle == -90)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}

    //判断是否在移动，如果正在移动过程中则不能进行下一次移动，实现每次移动消耗一个行动点数的效果
    public void MoveableDetection()
    {
        isMoving = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                    Input.GetKey(KeyCode.D));
    }
}
