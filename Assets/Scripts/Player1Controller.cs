
using UnityEngine;
using Cinemachine;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 2f; // 每秒移动的网格数（实际上这是每秒移动多少个网格大小的距离）
    public float gridSize = 1f; // 网格单元的大小
    private Animator anim;
    public bool isMoving;

    private GameObject movePoint;
    private bool isActivePlayer;
    public CinemachineVirtualCamera vc1;
    public CinemachineVirtualCamera vc2;

    private void Start()
    {
        movePoint = transform.Find("Player1MovePoint").gameObject;
        //
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
        //角色移动到目标地点
        transform.position =
            Vector3.MoveTowards(transform.position, movePoint.transform.position,
                moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.transform.position) <= .05f
            && !isMoving && GameManager.Instance.GetActionPoint())
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                //设置目标地点
                movePoint.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * gridSize, 0f, 0f);
                //行动点数-1
                GameManager.Instance.SetActionPoint();

            }
        
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                //设置目标地点
                movePoint.transform.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * gridSize);
                //行动点数-1
                GameManager.Instance.SetActionPoint();
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
