using UnityEngine;

public static class Helpers
{
    public static bool KeyDown(ref KeyCode[] keycodes, bool capturable = true)
    {
        if (capturable && GameManager.Instance.InputCapture) return false;

        for (int i = 0; i < keycodes.Length; i++)
        {
            if (Input.GetKeyDown(keycodes[i])) return true;
        }
        return false;
    }
}