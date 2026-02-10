using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PanelController : MonoBehaviour
{
    // refer to RectTransform of popup panel
    [SerializeField] private RectTransform panelTransform;

    public delegate void PanelControllerHideDelegate();

    private CanvasGroup _canvasGroup;

    void Awake() 
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Show Popup
    public void Show()
    {
        Debug.Log("Show panel");

        // Hide the panel for now
        _canvasGroup.alpha = 0;
        panelTransform.localScale = Vector3.zero;

        _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear);
        panelTransform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    // Hide Popup
    public void Hide(PanelControllerHideDelegate onComplete = null)
    {
        Debug.Log("Hide panel");

        _canvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear);
        panelTransform.DOScale(0, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onComplete.Invoke();
            Destroy(gameObject);
        });
    }
}