using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image bgImage;
    public Image handleImage;
    private Vector2 inputVector;

    public float handleRange = 1f;

    private void Start()
    {
        if (bgImage == null) bgImage = GetComponent<Image>();

        if (handleImage == null && transform.childCount > 0)
        {
            handleImage = transform.GetChild(0).GetComponent<Image>();
        }

        if (handleImage == null)
        {
            Debug.LogError("VirtualJoystick Error: Handle Image belum dimasukkan di Inspector!");
        }
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handleImage.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bgImage.rectTransform.sizeDelta.x / 2), inputVector.y * (bgImage.rectTransform.sizeDelta.y / 2)) * handleRange;
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        handleImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        return inputVector.x;
    }

    public float Vertical()
    {
        if (inputVector.y != 0) return inputVector.y;
        return inputVector.y;
    }
}