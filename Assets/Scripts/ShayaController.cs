using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShayaController : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speed;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private Controls controls;
    private Vector2 direction;
    private Vector3 direction3D;
    private CharacterController controller;

    private Camera mainCam;

    private Vector3 mouvement;

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Dash.performed += OnDashPerformed;

    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();
        direction3D = new Vector3(direction.x, 0, direction.y);
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        direction = Vector2.zero;
        direction3D = Vector3.zero;
    }

    private void OnDashPerformed(InputAction.CallbackContext obj)
    {
        StartCoroutine(Dash());
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouvement = ApplyMove() + ApplyGravity();
        controller.Move(mouvement * Time.deltaTime);
    }

    private Vector3 ApplyMove()
    {
        if(direction3D == Vector3.zero)
        {
            return Vector3.zero;
        }

        var rotation = Quaternion.LookRotation(direction3D);

        rotation *= Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        var moveDirection = rotation * Vector3.forward;

        return moveDirection.normalized * speed;
    }

    private Vector3 ApplyGravity()
    {
        var falling = new Vector3(0, gravity, 0);
        return falling;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(controller * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
