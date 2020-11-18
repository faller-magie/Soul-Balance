using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShayaController : MonoBehaviour
{
    [SerializeField] private float gravity;

    private Controls controls;
    private Vector2 direction;
    private Vector3 direction3D;
    private CharacterController controller;

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();
        direction3D = new Vector3(direction.x, 0, direction.y);;
        Debug.Log(direction);
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        direction = Vector2.zero;
        direction3D = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouvement = ApplyMove() + ApplyGravity();
        controller.Move(mouvement * Time.deltaTime);
    }

    private Vector3 ApplyMove()
    {
        if(direction3D == Vector3.zero)
        {
            return Vector3.zero;
        }

        var rotation = Quaternion.LookRotation(direction3D);

        var moveDirection = rotation * Vector3.forward;

        return moveDirection.normalized * 25;
    }

    private Vector3 ApplyGravity()
    {
        var falling = new Vector3(0, gravity, 0);
        return falling;
    }
}
