using UnityEngine;

public class ButtonState : MonoBehaviour
{
    public enum State
    {
        Undamaged,
        Damaging,
        Damaged
    }

    public State currentState = State.Undamaged;

    public Renderer cubeRenderer; // Reference to the cube's Renderer
    public Material undamagedMaterial; // Material for undamaged state
    public Material damagingMaterial; // Material for damaging state
    public Material damagedMaterial; // Material for damaged state

    private void Start()
    {
        // Initialize the cube's appearance
        UpdateButtonAppearance();
    }

    public void SetState(State newState)
    {
        currentState = newState;
        UpdateButtonAppearance();
    }

    private void UpdateButtonAppearance()
    {
        // Change the cube's material based on the current state
        switch (currentState)
        {
            case State.Undamaged:
                cubeRenderer.material = undamagedMaterial;
                break;
            case State.Damaging:
                cubeRenderer.material = damagingMaterial;
                break;
            case State.Damaged:
                cubeRenderer.material = damagedMaterial;
                break;
        }
    }
}