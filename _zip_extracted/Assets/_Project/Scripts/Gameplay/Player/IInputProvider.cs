using UnityEngine;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// IInputProvider: Interface para isolar sistema de input
    /// Permite trocar entre Input antigo, novo Input System, ou controles customizados
    /// </summary>
    public interface IInputProvider
    {
        Vector2 GetMovementInput();
        bool GetPauseInput();
    }

    /// <summary>
    /// DefaultInputProvider: Implementação usando Input clássico do Unity
    /// </summary>
    public class DefaultInputProvider : IInputProvider
    {
        public Vector2 GetMovementInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            return new Vector2(horizontal, vertical);
        }

        public bool GetPauseInput()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
}