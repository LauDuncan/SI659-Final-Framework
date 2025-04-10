using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AutoWalkToWaypoint : MonoBehaviour
{
    // 存放所有的目标 waypoint（在 Inspector 中直接拖入）
    public List<Transform> wayPoints = new List<Transform>();

    // NPC 的 NavMeshAgent 组件
    private NavMeshAgent agent;
    
    // 到达 waypoint 的距离阈值（当剩余距离小于这个值，就认为到达了目标）
    public float arrivalThreshold = 0.25f;
    
    // 走路速度
    public float walkSpeed = 2.0f;
    
    // 可选：Animator 组件，用于设置动画参数
    public Animator animator;

    void Start()
    {
        // 获取 NavMeshAgent 组件
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = walkSpeed;
        }
        
        // 如果有可选的 waypoint 列表，则选择一个随机 waypoint 作为目标
        if (wayPoints.Count > 0)
        {
            int index = Random.Range(0, wayPoints.Count);
            SetNextWaypoint(wayPoints[index]);
        }
    }

    void Update()
    {
        if (agent == null || agent.pathPending)
            return;

        // 当到达目标时，选择一个新的 waypoint
        if (agent.remainingDistance <= arrivalThreshold)
        {
            // 如需切换动画到待机状态
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            
            if (wayPoints.Count > 0)
            {
                // 随机选择新的目标点
                int index = Random.Range(0, wayPoints.Count);
                SetNextWaypoint(wayPoints[index]);
            }
        }
        else
        {
            // 移动中时，可在 Animator 中设置走路参数
            if (animator != null)
            {
                // 以 agent 当前速度作为参数值
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
    }

    // 设置 NavMeshAgent 的目标位置
    void SetNextWaypoint(Transform wp)
    {
        if (agent != null)
        {
            agent.SetDestination(wp.position);
        }
    }
}
