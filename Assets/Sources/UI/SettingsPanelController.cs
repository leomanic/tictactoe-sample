using UnityEngine;

public class SettingsPanelController : PanelController
{
    public delegate void OnConfirmButtonClicked();
    private OnConfirmButtonClicked _onConfirmButtonClicked;
    public void OnClickCloseButton() 
    {
        // 1. Save to Setting
        // 2. Close Window
        Hide(() =>
        {
            _onConfirmButtonClicked?.Invoke();
        });
    }

    void Update() 
    {
        
    }
}
