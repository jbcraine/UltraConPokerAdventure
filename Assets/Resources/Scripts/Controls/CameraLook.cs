using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHorizontal = 9.0f;
    public float sensitivityVertical = 9.0f;
    public float minimumVertical = -45.0f;
    public float maximumVertical = 45.0f;


    private float _rotationX = 0;

    private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.ConversationStateChanged += SetActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHorizontal, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVertical;
            _rotationX = Mathf.Clamp(_rotationX, minimumVertical, maximumVertical);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityHorizontal;
            _rotationX = Mathf.Clamp(_rotationX, minimumVertical, maximumVertical);

            float delta = Input.GetAxis("Mouse X") * sensitivityHorizontal;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }

    private void SetActive(bool state)
    {
        active = !state;
    }
}
