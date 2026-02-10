using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private TMP_Text messageText;
    public delegate void OnConfirmButtonClicked();
    private OnConfirmButtonClicked _onConfirmButtonClicked;

    public void Show(string message, OnConfirmButtonClicked onConfirmButtonClicked = null)
    {
        _onConfirmButtonClicked = onConfirmButtonClicked;
        messageText.text = message;
        Show();
    }

    public void OnClickCloseButton()
    {
        Hide(() =>
        {
            _onConfirmButtonClicked?.Invoke();
        });
    }
}