using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    private NotificationUI notification;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateNotificationUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateNotificationUI()
    {
        GameObject prefab = Resources.Load<GameObject>("NotificationUI");
        if (prefab == null)
        {
            Debug.LogError("NotificationUI prefab not found in Resources folder!");
            return;
        }

        GameObject notifObj = Instantiate(prefab);

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            notifObj.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Debug.LogError("Canvas not found in the scene!");
        }

        DontDestroyOnLoad(notifObj);
        notification = notifObj.GetComponent<NotificationUI>();
        notification.Hide();
    }

    public void Show(string message, float duration)
    {
        if (notification == null || notification.gameObject == null)
        {
            Debug.LogWarning("NotificationUI missing, recreating...");
            CreateNotificationUI();
        }

        if (notification != null)
        {
            notification.Show(message, duration);
        }
        else
        {
            Debug.LogError("Failed to create NotificationUI.");
        }
    }
}