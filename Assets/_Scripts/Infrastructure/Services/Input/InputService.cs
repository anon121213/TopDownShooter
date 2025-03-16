using System;
 using UnityEngine;
 using UnityEngine.InputSystem;
 
 namespace _Scripts.Infrastructure.Services.Input
 {
   public class InputService : IInputService, IDisposable
   {
     public event Action OnStartMove;
     public event Action OnStopMove;
     public event Action OnChangeWeapon;

     public Vector2 MoveDirection => _input.Player.Move.ReadValue<Vector2>().normalized;

     private readonly GameInput _input;

     public InputService()
     {
       _input = new GameInput();
       _input.Enable();
       Enable();
     }

     public void Enable()
     {
       _input.Player.Move.performed += StartMoveHandler;
       _input.Player.Move.canceled += StopMoveHandler;
       _input.Player.SwitchWeapon.performed += InputHandler;
     }

     private void InputHandler(InputAction.CallbackContext obj) => 
       OnChangeWeapon?.Invoke();

     private void StartMoveHandler(InputAction.CallbackContext obj) => 
       OnStartMove?.Invoke();

     private void StopMoveHandler(InputAction.CallbackContext obj) => 
       OnStopMove?.Invoke();

     public void Disable()
     {
       _input.Player.Move.performed -= StartMoveHandler;
       _input.Player.Move.canceled -= StopMoveHandler;
       _input.Player.SwitchWeapon.performed -= InputHandler;
     }

     public void Dispose() => 
       Disable();
   }

   public interface IInputService
   {
     event Action OnStartMove;
     event Action OnStopMove;
     event Action OnChangeWeapon;
     Vector2 MoveDirection { get; }
   }
 }