// Copyright (c) Meta Platforms, Inc. and affiliates.

using System.Collections;
using Meta.XR.Samples;
using MRMotifs.SharedAssets;
using UnityEngine;
using UnityEngine.UI;

namespace MRMotifs.PassthroughTransitioning
{
    [MetaCodeSample("MRMotifs-PassthroughTransitioning")]
    public class PassthroughDissolver : MonoBehaviour
    {
        [Tooltip("The range of the passthrough dissolver sphere.")]
        [SerializeField]
        private float distance = 20f;

        [Tooltip("The inverted alpha value at which the contextual boundary should be enabled/disabled.")]
        [SerializeField]
        private float boundaryThreshold = 0.25f;

        [Tooltip("The value to set when passthrough is fully enabled")]
        [SerializeField]
        private float fullyEnabledValue = 1f;

        [Tooltip("The value to set when passthrough is fully disabled")]
        [SerializeField]
        private float fullyDisabledValue = 0f;

        [Tooltip("The duration in seconds for the passthrough transition")]
        [SerializeField]
        private float transitionDuration = 1.0f;

        private Camera m_mainCamera;
        private Material m_material;
        private MeshRenderer m_meshRenderer;
        private MenuPanel m_menuPanel;
        private Slider m_alphaSlider;
        private Coroutine m_activeTransition;

        private static readonly int s_dissolutionLevel = Shader.PropertyToID("_Level");

        private void Awake()
        {
            m_mainCamera = Camera.main;
            if (m_mainCamera != null)
            {
                m_mainCamera.clearFlags = CameraClearFlags.Skybox;
            }

            // This is a property that determines whether premultiplied alpha blending is used for the eye field of view
            // layer, which can be adjusted to enhance the blending with underlays and potentially improve visual quality.
            OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;

            m_meshRenderer = GetComponent<MeshRenderer>();
            m_material = m_meshRenderer.material;
            m_material.SetFloat(s_dissolutionLevel, 0);
            m_meshRenderer.enabled = true;

            SetSphereSize(distance);

            m_menuPanel = FindAnyObjectByType<MenuPanel>();

            if (m_menuPanel != null)
            {
                m_alphaSlider = m_menuPanel.PassthroughFaderSlider;
                m_alphaSlider.onValueChanged.AddListener(HandleSliderChange);
            }

#if UNITY_ANDROID
            CheckIfPassthroughIsRecommended();
#endif
        }

        private void OnDestroy()
        {
            if (m_menuPanel != null)
            {
                m_alphaSlider.onValueChanged.RemoveListener(HandleSliderChange);
            }
        }

        private void SetSphereSize(float size)
        {
            transform.localScale = new Vector3(size, size, size);
        }

        private void CheckIfPassthroughIsRecommended()
        {
            m_material.SetFloat(s_dissolutionLevel, OVRManager.IsPassthroughRecommended() ? 1 : 0);
            OVRManager.instance.shouldBoundaryVisibilityBeSuppressed = OVRManager.IsPassthroughRecommended();

            if (m_menuPanel != null)
            {
                m_alphaSlider.value = OVRManager.IsPassthroughRecommended() ? 1 : 0;
            }
        }

        private void HandleSliderChange(float value)
        {
            m_material.SetFloat(s_dissolutionLevel, value);

            if (value > boundaryThreshold || value < boundaryThreshold)
            {
                OVRManager.instance.shouldBoundaryVisibilityBeSuppressed = value > boundaryThreshold;
            }
        }

        public void TogglePassthrough()
        {
            // Get the current actual value from the material
            float currentLevel = m_material.GetFloat(s_dissolutionLevel);
            
            // Determine the appropriate target based on the current level
            // If we're closer to enabled, go to disabled, and vice versa
            float targetValue = (currentLevel > 0.5f) ? fullyDisabledValue : fullyEnabledValue;
            
            // Stop any ongoing transition
            if (m_activeTransition != null)
            {
                StopCoroutine(m_activeTransition);
                m_activeTransition = null;
            }
            
            // Start a new transition from the current material value
            m_activeTransition = StartCoroutine(AnimateTransition(currentLevel, targetValue, transitionDuration));
        }
        
        private IEnumerator AnimateTransition(float startValue, float targetValue, float duration)
        {
            float elapsedTime = 0;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                
                // Use smooth step for a more natural transition
                t = Mathf.SmoothStep(0, 1, t);
                
                float currentValue = Mathf.Lerp(startValue, targetValue, t);
                
                // Update material
                m_material.SetFloat(s_dissolutionLevel, currentValue);
                
                // Update slider if it exists
                if (m_alphaSlider != null)
                {
                    m_alphaSlider.value = currentValue;
                }
                
                // Update boundary visibility
                if (currentValue > boundaryThreshold || currentValue < boundaryThreshold)
                {
                    OVRManager.instance.shouldBoundaryVisibilityBeSuppressed = currentValue > boundaryThreshold;
                }
                
                yield return null;
            }
            
            // Ensure we end at exactly the target value
            m_material.SetFloat(s_dissolutionLevel, targetValue);
            
            if (m_alphaSlider != null)
            {
                m_alphaSlider.value = targetValue;
            }
            
            OVRManager.instance.shouldBoundaryVisibilityBeSuppressed = targetValue > boundaryThreshold;
            
            m_activeTransition = null;
        }
    }
}
