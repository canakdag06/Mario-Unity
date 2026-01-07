using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindButton : MonoBehaviour
{
    public InputActionReference actionReference;
    public int bindingIndex = 0;
    public TMP_Text bindingText;

    private void OnEnable()
    {
        LoadOverride();
        UpdateBindingDisplay();
    }

    public void StartRebinding()
    {
        var action = actionReference.action;
        action.Disable();

        bindingText.text = "Press a key";

        actionReference.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("Keyboard/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                UpdateBindingDisplay();
                PlayerPrefs.SetString(actionReference.action.name + "_binding_" + bindingIndex,
                    actionReference.action.bindings[bindingIndex].effectivePath);
                PlayerPrefs.Save();
                action.Enable();
            }).Start();
    }

    public void UpdateBindingDisplay()
    {
        bindingText.text = InputControlPath.ToHumanReadableString(
            actionReference.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void LoadOverride()
    {
        string bindingKey = actionReference.action.name + "_binding_" + bindingIndex;
        if (PlayerPrefs.HasKey(bindingKey))
        {
            string overridePath = PlayerPrefs.GetString(bindingKey);
            actionReference.action.ApplyBindingOverride(bindingIndex, overridePath);
        }
    }

    public void RefreshBinding()
    {
        string bindingKey = actionReference.action.name + "_binding_" + bindingIndex;

        if (PlayerPrefs.HasKey(bindingKey))
        {
            string overridePath = PlayerPrefs.GetString(bindingKey);
            actionReference.action.ApplyBindingOverride(bindingIndex, overridePath);
        }
        else
        {
            actionReference.action.RemoveBindingOverride(bindingIndex);
        }

        UpdateBindingDisplay();
    }

}
