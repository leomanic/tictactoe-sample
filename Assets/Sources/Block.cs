using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private SpriteRenderer markerSpriteRenderer;

    public enum MarkerType { None, O, X };
    private int _blockIndex;

    // Init Block
    public void InitMarker(int blockIndex)
    {
        _blockIndex = blockIndex;
        SetMarker(MarkerType.None);
    }

    // Set Marker
    public void SetMarker(MarkerType markerType)
    {
        switch (markerType)
        {
            case MarkerType.O:
                markerSpriteRenderer.sprite = oSprite;
                break;
            case MarkerType.X:
                markerSpriteRenderer.sprite = xSprite;
                break;
            case MarkerType.None:
                markerSpriteRenderer.sprite = null;
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Debug.Log("Block Clicked : " + _blockIndex);
    }
}