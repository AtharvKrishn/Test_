using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private bool canRotate;

    void Update()
    {
        if (canRotate)
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.up, -rotationX);
            transform.Rotate(Vector3.right, rotationY);
        }
    }

    public void ToggleRotation(bool isRotating)
    {
        canRotate = isRotating;
    }
}
