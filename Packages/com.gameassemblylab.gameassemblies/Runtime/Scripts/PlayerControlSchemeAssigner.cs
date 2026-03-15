using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Detects available control schemes on the PlayerInput's input actions asset
/// and assigns each player to a unique gamepad (or falls back to keyboard).
/// Also handles runtime switching between gamepad and keyboard based on input.
/// Attach to the same GameObject as PlayerInput and playerController.
/// </summary>
public class PlayerControlSchemeAssigner : MonoBehaviour
{
    [Tooltip("Unique index per tank (0-3). When gamepads are connected, each index maps to a gamepad. When no gamepad is available at this index, falls back to keyboard (even=WASD, odd=Arrows). -1 = auto from playerController.playerID.")]
    public int controlSchemeIndex = -1;

    [Tooltip("Enable to allow runtime switching between gamepad and keyboard based on which device has input.")]
    public bool allowRuntimeSwitching = true;

    public bool debug = false;

    private PlayerInput _playerInput;
    private string _gamepadScheme;
    private int _resolvedIndex;

    public string CurrentScheme => _playerInput != null ? _playerInput.currentControlScheme : "";

    public void Initialize(PlayerInput playerInput, int playerID)
    {
        _playerInput = playerInput;
        _resolvedIndex = (controlSchemeIndex >= 0) ? controlSchemeIndex : playerID;

        DetectSchemeNames();
        AssignInitialScheme();
    }

    private void Update()
    {
        if (allowRuntimeSwitching)
            CheckRuntimeSwitch();
    }

    private void DetectSchemeNames()
    {
        _gamepadScheme = "Joystick";

        if (_playerInput?.actions == null) return;

        bool hasJoystick = false, hasGamepad = false;
        foreach (var scheme in _playerInput.actions.controlSchemes)
        {
            if (scheme.name == "Joystick") hasJoystick = true;
            if (scheme.name == "Gamepad") hasGamepad = true;
        }

        _gamepadScheme = hasJoystick ? "Joystick" : hasGamepad ? "Gamepad" : "Joystick";
    }

    private string GetKeyboardScheme()
    {
        if (_playerInput?.actions == null)
            return (_resolvedIndex % 2 == 0) ? "Keyboard" : "KeyboardArrows";

        bool hasKeyboard = false, hasKeyboardArrows = false, hasKeyboardMouse = false;
        foreach (var scheme in _playerInput.actions.controlSchemes)
        {
            switch (scheme.name)
            {
                case "Keyboard":       hasKeyboard = true; break;
                case "KeyboardArrows": hasKeyboardArrows = true; break;
                case "Keyboard&Mouse": hasKeyboardMouse = true; break;
            }
        }

        if (hasKeyboard && hasKeyboardArrows)
            return (_resolvedIndex % 2 == 0) ? "Keyboard" : "KeyboardArrows";
        if (hasKeyboardMouse)
            return "Keyboard&Mouse";
        if (hasKeyboard)
            return "Keyboard";
        return "Keyboard&Mouse";
    }

    private bool IsGamepadScheme(string scheme)
    {
        return scheme == "Joystick" || scheme == "Gamepad";
    }

    private void AssignInitialScheme()
    {
        if (_playerInput == null) return;

        if (Gamepad.all.Count > _resolvedIndex)
        {
            var gamepad = Gamepad.all[_resolvedIndex];
            try
            {
                _playerInput.SwitchCurrentControlScheme(_gamepadScheme, gamepad);
                if (debug) Debug.Log("Player " + _resolvedIndex + " assigned to Gamepad (" + gamepad.displayName + ") scheme=" + _gamepadScheme);
            }
            catch (Exception ex)
            {
                if (debug) Debug.LogWarning("Could not assign gamepad " + _resolvedIndex + ": " + ex.Message);
            }
        }
        else
        {
            string scheme = GetKeyboardScheme();
            var keyboard = Keyboard.current;
            try
            {
                if (keyboard != null)
                    _playerInput.SwitchCurrentControlScheme(scheme, keyboard);
                else
                    _playerInput.SwitchCurrentControlScheme(scheme);
                if (debug) Debug.Log("Player " + _resolvedIndex + " (no gamepad) -> " + scheme);
            }
            catch (Exception ex)
            {
                if (debug) Debug.LogWarning("Could not switch to " + scheme + ": " + ex.Message);
            }
        }
    }

    private void CheckRuntimeSwitch()
    {
        if (_playerInput == null) return;

        string currentScheme = _playerInput.currentControlScheme;

        if (IsGamepadScheme(currentScheme))
        {
            var kb = Keyboard.current;
            if (kb == null) return;

            bool keyboardActive = (_resolvedIndex % 2 == 0)
                ? kb.wKey.isPressed || kb.aKey.isPressed || kb.sKey.isPressed || kb.dKey.isPressed
                : kb.upArrowKey.isPressed || kb.downArrowKey.isPressed || kb.leftArrowKey.isPressed || kb.rightArrowKey.isPressed;

            if (keyboardActive)
            {
                string scheme = GetKeyboardScheme();
                try
                {
                    _playerInput.SwitchCurrentControlScheme(scheme, kb);
                    if (debug) Debug.Log("Player " + _resolvedIndex + " switched to " + scheme);
                }
                catch (Exception) { }
            }
        }
        else
        {
            if (Gamepad.all.Count > _resolvedIndex)
            {
                var gp = Gamepad.all[_resolvedIndex];
                bool gamepadActive = gp.leftStick.ReadValue().sqrMagnitude > 0.01f
                    || gp.buttonSouth.isPressed || gp.buttonEast.isPressed
                    || gp.buttonWest.isPressed || gp.buttonNorth.isPressed
                    || gp.startButton.isPressed;

                if (gamepadActive)
                {
                    try
                    {
                        _playerInput.SwitchCurrentControlScheme(_gamepadScheme, gp);
                        if (debug) Debug.Log("Player " + _resolvedIndex + " switched to " + _gamepadScheme);
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
