using UnityEngine;
using UnityEngine.InputSystem;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float rotationSpeedMultiplier = 180f; // degrees/sec at full crank speed
    [SerializeField] private float deadZone = 0.2f;

    private Vector2 previousInput;
    private bool hasPreviousInput = false;

    public void Rotate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Ignore weak stick input
        if (input.magnitude < deadZone)
        {
            hasPreviousInput = false;
            return;
        }

        input.Normalize();

        if (!hasPreviousInput)
        {
            previousInput = input;
            hasPreviousInput = true;
            return;
        }

        // Signed angle between last stick direction and current one
        float deltaAngle = Vector2.SignedAngle(previousInput, input);

        // Clockwise / counterclockwise preserved by sign
        float rotationAmount = deltaAngle * rotationSpeedMultiplier * Time.deltaTime;

        transform.Rotate(0f, 0f, rotationAmount);

        previousInput = input;
    }
}
