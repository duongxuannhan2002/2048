using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionAnimation : MonoBehaviour
{
    public Image explosionImage;  // `Image` để hiển thị animation
    public Sprite[] explosionFrames; // Danh sách sprite frames cho hiệu ứng nổ
    public float frameRate = 0.05f; // Tốc độ đổi frame

    private int currentFrame = 0;

    public void PlayExplosion(Vector2 position)
    {
        transform.position = position;  // Đặt vị trí nổ
        gameObject.SetActive(true);     // Hiển thị hiệu ứng
        StartCoroutine(AnimateExplosion());
    }

    IEnumerator AnimateExplosion()
    {
        if (explosionFrames == null || explosionFrames.Length == 0)
        {
            Debug.LogError("Explosion frames are missing!");
            yield break;
        }

        while (currentFrame < explosionFrames.Length)
        {
            if (explosionImage != null)
            {
                explosionImage.sprite = explosionFrames[currentFrame]; // Đổi sprite
            }
            else
            {
                Debug.LogError("explosionImage is null!");
            }

            currentFrame++;
            yield return new WaitForSeconds(frameRate);
        }
        Destroy(gameObject);  // Hủy đối tượng sau khi hiệu ứng hoàn tất
    }
}
