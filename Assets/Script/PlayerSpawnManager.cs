using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 1. 加载场景时：
///    • 如果场景里已有带 Tag=PlayerRig 的新 Rig ➜ 只切摄像机 & 输入到新 Rig；
///    • 否则 ➜ 把旧 Rig 移到 Tag=PlayerSpawn 的空物体位置；
/// 2. 始终保留唯一的 OVRManager（在 OVRRuntime 上，勿删除）；
/// 3. 旧 Rig 不再销毁 OVRManager，从而避免 MissingReferenceException。
/// </summary>
public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Tag 设置")]
    [SerializeField] private string rigTag   = "PlayerRig";
    [SerializeField] private string spawnTag = "PlayerSpawn";

    // 记录旧 Rig 的相机组件，方便禁用
    private Camera[] _cameras;

    private void Awake()
    {
        // 把本 Rig 保留下来以便可能继续使用
        DontDestroyOnLoad(gameObject);
        _cameras = GetComponentsInChildren<Camera>(true);
    }

    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ===== 1) 查找新场景是否有自己的 Rig =====
        var rigs = GameObject.FindGameObjectsWithTag(rigTag);
        foreach (var r in rigs)
        {
            if (r.scene == scene)                 // 这棵 Rig 属于新场景
            {
                SwitchToNewRig(r);
                return;                           // 结束：不再移动旧 Rig
            }
        }

        // ===== 2) 没有新 Rig ⇒ 把旧 Rig 移到 Spawn =====
        MoveOldRigToSpawn(scene);
    }

    /// <summary>启用新 Rig 的相机/输入，并禁用旧 Rig。</summary>
    private void SwitchToNewRig(GameObject newRig)
    {
        // 启用新 Rig
        newRig.SetActive(true);
        foreach (var cam in newRig.GetComponentsInChildren<Camera>(true))
            cam.enabled = true;

        // 禁用旧 Rig 的渲染与输入，但不销毁，防止 OVR 脚本失引用
        foreach (var cam in _cameras) cam.enabled = false;
        foreach (var collider in GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        // 可选择整体隐藏旧 Rig
        gameObject.SetActive(false);

        Debug.Log("[PlayerSpawnManager] Switched to new Rig in scene: " + newRig.name);
    }

    /// <summary>场景没有新 Rig 时，把旧 Rig 移动到 Spawn 点。</summary>
    private void MoveOldRigToSpawn(Scene scene)
    {
        var spawn = GameObject.FindGameObjectWithTag(spawnTag);
        if (spawn != null)
        {
            transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
            Debug.Log("[PlayerSpawnManager] Moved old Rig to Spawn in scene: " + scene.name);
        }
        else
        {
            Debug.LogWarning($"[PlayerSpawnManager] Spawn tag '{spawnTag}' not found in scene {scene.name}. Rig stays at previous position.");
        }
    }
}
