using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    internal bool PressedRoll()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
