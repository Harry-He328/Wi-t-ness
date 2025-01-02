using System.Collections;
using UnityEngine;
 
public class EnemyController : MonoBehaviour
{
    public Transform playerTransform1; // 引用玩家的位置
    public Transform playerTransform2;
    public float moveSpeed = 1.0f; // 移动速度
    public float gridSize = 1.0f; // 网格大小
    public bool enemyMoveable = true; // 敌人是否可以移动
    private bool isMoving = false; // 当前是否正在移动
    private Vector3 movePoint; // 目标移动点
    public static EnemyController Instance;
 
    void Start()
    {
        movePoint = transform.position;
        Instance = this;

    }

    void Update()
    {
        if (CameraController.Instance.activePlayer1)
        {
            EnemyMovement1();
        }
        else
        {
            EnemyMovement2();
        }
    }

    
    //想在这里实现让敌人每走一步就停一下
   private IEnumerator EnemyMovementCoroutine()
    {
        while (!GameManager.Instance.isPlayerTurn)
        {
            if (!enemyMoveable || GameManager.Instance.currentEnemyActionPoint <= 0)
                yield break;
            
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, step);
 
            // 检查是否到达目标点
            float distanceToTarget = Vector3.Distance(transform.position, movePoint);
            if (distanceToTarget <= 0.05f && !isMoving)
            {
                isMoving = false;
                
                // 追踪玩家
                Vector3 directionToPlayer = (playerTransform1.position - transform.position).normalized;
                Vector3 newMovePoint = transform.position;
 
                // 根据方向选择横或竖移动一格。
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.z))
                {
                    newMovePoint.x += directionToPlayer.x * gridSize;
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    newMovePoint.z += directionToPlayer.z * gridSize;
                    yield return new WaitForSeconds(1f);
                }

                movePoint = new Vector3(Mathf.Round(newMovePoint.x / gridSize) * gridSize, transform.position.y,
                    Mathf.Round(newMovePoint.z / gridSize) * gridSize);
                isMoving = true;
 
                // 消耗行动点数。
                GameManager.Instance.currentEnemyActionPoint--;
            }
            else
            {
                isMoving = distanceToTarget > 0.05f;
            }
            yield return null;
        }
    }
    
    public void EnemyMovement1()
    {
        if(!GameManager.Instance.isPlayerTurn && CameraController.Instance.activePlayer1)
        {
            if (!enemyMoveable || GameManager.Instance.currentEnemyActionPoint <= 0)
                return;

            // 角色移动到目标地点
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, step);

            // 检查是否到达目标点
            float distanceToTarget = Vector3.Distance(transform.position, movePoint);
            if (distanceToTarget <= 0.05f && !isMoving)
            {
                // 重置移动状态
                isMoving = false;

                // 计算新的目标地点（追踪玩家）
                Vector3 directionToPlayer = (playerTransform1.position - transform.position).normalized;
                Vector3 newMovePoint = transform.position;

                // 根据方向选择横或竖移动一格
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.z))
                {
                    // 水平移动
                    newMovePoint.x += directionToPlayer.x * gridSize;
                }
                else
                {
                    // 垂直移动
                    newMovePoint.z += directionToPlayer.z * gridSize;
                }

                // 更新移动点
                movePoint = new Vector3(Mathf.Round(newMovePoint.x / gridSize) * gridSize, transform.position.y,
                    Mathf.Round(newMovePoint.z / gridSize) * gridSize);

                // 消耗行动点数
                GameManager.Instance.currentEnemyActionPoint--;
                isMoving = true;
            }
            else
            {
                isMoving = distanceToTarget > 0.05f;
            }
        }
    }

    public void EnemyMovement2()
    {
        if (!GameManager.Instance.isPlayerTurn && !CameraController.Instance.activePlayer1)
        {
            // 计算目标方向与敌人当前方向的向量差
            Vector3 direction = playerTransform2.position - transform.position;
 
            // 确保敌人不会穿过目标，计算距离并限制在一步之内
            float distance = direction.magnitude;
            direction.Normalize(); // 标准化方向向量，使其长度为1
 
            // 计算移动量
            float moveAmount = moveSpeed * Time.deltaTime;
 
            // 如果距离小于一步的距离，则只移动剩余的距离
            if (distance < moveAmount)
            {
                moveAmount = distance;
            }
 
            // 根据方向向量和移动量更新敌人位置
            transform.position += direction * moveAmount;
        }
    }
}
