using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TweenSample : MonoBehaviour
{
    public RectTransform UITarget;          //UI 타겟
    public Image UIImage;
    public GameObject ObjectTarget;         //오브젝트 타겟

    public TMP_Text countText;
    public int currentValue = 0;
    public int addValue = 100;

    private int targetValue;

    public Color flashColor = Color.red;

    private Color originalColor;

    public CanvasGroup fadeTarget;

    public GameObject coinPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalColor = UIImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayPunchUIScale();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayPunchObjectScale();
        }

        if( Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayUIShake();
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayCountUp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayColorFlash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayFade();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Vector3 dropPosition = transform.position + Vector3.up;
            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }


    }

    public void PlayPunchUIScale()
    {
        if (ObjectTarget == null) return;
        UITarget.DOKill();
        UITarget.localScale = Vector3.one;
        UITarget.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);
    }

    public void PlayPunchObjectScale()
    {
        if (ObjectTarget == null) return;
        ObjectTarget.transform.DOKill();
        ObjectTarget.transform.localScale = Vector3.one;
        ObjectTarget.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);
    }


    public void PlayUIShake()
    {
        if(ObjectTarget == null) return;
        {
            ObjectTarget.transform.DOKill();
            ObjectTarget.transform.DOShakePosition(0.3f, 20f, 20, 90f);
        }
    }

    public void PlayCountUp()
    {
        if (countText == null) return;

        targetValue += addValue;
        DOTween.Kill("CountTween", true);

        DOTween.To(
            () => currentValue,                 //현재 값
            value =>                            //중간 값이 바뀔때 마다 실행 되는 부분
            {
                currentValue = value;
                countText.text = currentValue.ToString();
            },
            targetValue,
            0.5f


        )
        .SetEase(Ease.OutQuad)
        .SetId("CountTween");
    }

    public void PlayColorFlash()
    {
        if (UIImage = null) return;

        UIImage.DOKill();
        UIImage.color = originalColor;
        UIImage.DOColor(flashColor, 0.1f)
        .OnComplete(() =>
        {
            UIImage.DOColor(originalColor, 0.2f);
        });
        
    }

    public void PlayFade()
    {
        if(fadeTarget == null) return;
        fadeTarget.DOKill();
        fadeTarget.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(fadeTarget.DOFade(1, 0.2f));
        seq.AppendInterval(0.5f);
        seq.Append(fadeTarget.DOFade(0f, 0.3f));
    }

}
