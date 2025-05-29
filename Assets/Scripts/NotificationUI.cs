using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject panel;

    public void Show(string message, float duration)
    {
        messageText.SetText(message);
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
        Vector3 startPosPanel = panel.transform.position;
        panel.transform.position = new Vector3(startPosPanel.x, startPosPanel.y + 500f, startPosPanel.z); // Đặt thấp hơn vị trí ban đầu
        LeanTween.moveY(panel, startPosPanel.y, 1f).setEase(LeanTweenType.easeOutBack);
        CancelInvoke();
        Invoke("Hide", duration);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}