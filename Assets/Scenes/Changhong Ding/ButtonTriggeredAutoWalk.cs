using UnityEngine;
using UnityEngine.AI;
using Oculus.Avatar2.Experimental; // 如果需要引用 OVRInput，确保已安装 Oculus SDK

public class ButtonTriggeredAutoWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    // 要移动的目标点（在 Inspector 中拖拽目标物体的 Transform）
    public Transform targetPoint;
    // 到达目标的判定距离阈值
    public float arrivalThreshold = 0.25f;

    [Header("Input Settings")]
    // 开始移动的按钮（默认 A 按钮）
    public OVRInput.RawButton startButton = OVRInput.RawButton.A;
    // 使用哪个控制器（默认右手）
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    [Header("Animation Settings")]
    // Animator 组件
    [SerializeField] private Animator animator;
    // 动画触发器名称，须与你 Animator Controller 中设置一致
    [SerializeField] private string moveTrigger = "move";
    [SerializeField] private string idleTrigger = "idle";

    // NavMeshAgent 组件
    private NavMeshAgent agent;

    // 是否已经开始移动
    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("ButtonTriggeredAutoWalk: Missing NavMeshAgent component!");
            return;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("ButtonTriggeredAutoWalk: Missing Animator component!");
            }
        }

        // 初始时设置目标点
        if (targetPoint != null)
        {
            agent.SetDestination(targetPoint.position);
            // 初始时处于 Idle 状态
            if (animator != null)
                animator.SetTrigger(idleTrigger);
        }
        else
        {
            Debug.LogWarning("ButtonTriggeredAutoWalk: targetPoint is not set.");
        }
    }

    void Update()
    {
        // 如果尚未开始移动，并且检测到用户按下 startButton，则开始移动
        if (!isMoving && OVRInput.GetDown(startButton, controller))
        {
            isMoving = true;
            // 触发移动动画
            if (animator != null)
            {
                animator.ResetTrigger(idleTrigger);
                animator.SetTrigger(moveTrigger);
            }
            // 确保 NavMeshAgent 重新设置目标
            if (targetPoint != null)
                agent.SetDestination(targetPoint.position);
        }

        // 如果已经开始移动，检测是否到达目标
        if (isMoving)
        {
            if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold)
            {
                // 到达目标
                isMoving = false;
                // 停止移动（可以设置速度为0，也可以不改变，因为 agent 到达目标后会自己停止）
                agent.speed = 0;

                // 触发 idle 状态动画
                if (animator != null)
                {
                    animator.ResetTrigger(moveTrigger);
                    animator.SetTrigger(idleTrigger);
                }
            }
            else
            {
                // 尚未到达目标，保持移动状态动画
                if (animator != null)
                {
                    animator.ResetTrigger(idleTrigger);
                    animator.SetTrigger(moveTrigger);
                }
            }
        }
    }
}
