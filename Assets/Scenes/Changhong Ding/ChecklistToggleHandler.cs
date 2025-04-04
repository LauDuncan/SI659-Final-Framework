using UnityEngine;
using UnityEngine.UI;

public class ChecklistToggleHandler : MonoBehaviour
{
    // 引用 Checklist 使用的 UI Toggle
    public Toggle checklistToggle;

    private void OnEnable()
    {
        if (checklistToggle != null)
        {
            // 订阅 onValueChanged 事件
            checklistToggle.onValueChanged.AddListener(OnChecklistToggleValueChanged);
        }
        else
        {
            Debug.LogError("请在 Inspector 中赋值 checklistToggle。");
        }
    }

    private void OnDisable()
    {
        if (checklistToggle != null)
        {
            checklistToggle.onValueChanged.RemoveListener(OnChecklistToggleValueChanged);
        }
    }

    /// <summary>
    /// 当 Toggle 的值改变时调用此函数。参数 newValue 表示 Toggle 当前是否选中。
    /// </summary>
    /// <param name="newValue">true 表示选中，false 表示未选中</param>
    public void OnChecklistToggleValueChanged(bool newValue)
    {
        // 这里可以加入你希望在值变化时执行的逻辑
        if (newValue)
        {
            Debug.Log("Checklist 项目被选中（打勾）");
            // 例如：更新 UI、记录状态、触发其他事件……
        }
        else
        {
            Debug.Log("Checklist 项目未选中");
            // 例如：取消选中后的处理逻辑……
        }
    }
}
