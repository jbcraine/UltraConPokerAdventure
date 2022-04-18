using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public const float baseSpeed = 6.0f;
    public float gravity = -9.8f;
    public bool gravityOn = false;
    private CharacterController _characterController;

    bool active = true;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        DialogueManager.Instance.ConversationStateChanged += SetActive;
    }

    void Update()
    {
        if (!active)
            return;

        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        if (gravityOn)
            movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _characterController.Move(movement);
    }

    private void SetActive(bool state)
    {
        active = !state;
    }
}
