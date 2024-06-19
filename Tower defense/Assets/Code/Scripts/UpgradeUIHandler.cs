using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool mouse_over = false;
    public TextMeshProUGUI priceText;

    private int currentPrice = 150;

    public void SetTowerPrice(int price)
    {
        currentPrice = price;
        UpdatePriceText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
        UpdatePriceText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        priceText.SetText(""); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(priceText.rectTransform, eventData.position))
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdatePriceText()
    {
        if (priceText != null)
        {
            priceText.SetText(currentPrice.ToString());
        }
    }
}
