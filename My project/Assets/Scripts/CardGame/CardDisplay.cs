using NUnit.Framework.Internal;
using System.Collections;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;               //카드 데이터
    public int cardIndex;


    //3D 카드 요소
    public MeshRenderer cardRenderer;
    public TextMeshPro nameText;
    public TextMeshPro costText;
    public TextMeshPro attackText;
    public TextMeshPro descriptionText;

    //카드 상태
    public bool isDragging = false;
    private Vector3 originalPosition;

    //레이어 마스크
    public LayerMask enemyLayer;            //적 레이어
    public LayerMask playerLayer;           //플레이어 레이어


    private CardManager cardManager;        //카드 매니저 참조 추가
    public void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");


        SetupCard(cardData);
    }

    //카드 데이터 설정

    public void SetupCard(CardData data)
    {
        cardData = data;

        //3D 텍스트 업데이트
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.manaCost.ToString();
        if (attackText != null) attackText.text = data.effectAmount.ToString();
        if (descriptionText != null) descriptionText.text = data.description;

        //카드 텍스트 설정
        if (cardRenderer != null && data.artwor != null)
        {
            Material cardMaterial = cardRenderer.material;
            cardMaterial.mainTexture = data.artwor.texture;
        }

        //카드 설명 텍스트에 추가 효과 설명 추가
        if(descriptionText != null)
        {
            descriptionText.text = data.description + data.GetAdditionalEffectDescription();
        }
    }

    private void OnMouseDown()
    {
        //드레그 시작 시 원래 위치 저장
        originalPosition = transform.position;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            //마우스 위치로 카드 이동
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
    }


    private void OnMouseUp()
    {
        isDragging = false;

        //버린 카드 더미 근처 드롭했는지 검사 (마나 체크전)
        if(CardManager.Instance != null)
        {
            float disTODiscard = Vector3.Distance(transform.position, CardManager.Instance.discradPosition.positon);

            if(disTODiscard < 2.0f)
            {
                CardManager.Instance.DiscardCard(cardIndex);
                return;
            }
        }

        //카드 사용 로직 (마나 체크)
        if (CardManager.Instance.playerStats != null && CardManager.Instance .playerStats.currentMana <cardData.manaCost)
        {
            Debug.Log($"마나가 부족합니다! (필요 :{cardData.manaCost},현재 : {CardManager.Instance.playerStats?.currentMana?? 0} ");
            transform.position = originalPosition;
            return;
        }
    }

    
    

    public void ProcessAdditionalEffectsAndDiscard()
    {
        //카드 데이터 및 인덱스 보존
        CardData cardDataCopy = cardData;
        int cardIndexCopy = cardIndex;

        //추가 효과 적용
        foreach (var effect in cardDataCopy.additionalEffects)
        {
            switch (effect.effectAmount)
            {
                case CardData.AdditionalEffectType.DrawCard:

                case CardData.AdditionalEffectType.DiscardCard:
                    for (int i = 0; i < effect.effectAmount; i++)
                    {
                        if (CardManager.Instance != null && CardManager.Instance.handCards.Count > 0)
                        {
                            int randomIndex = Random.Range(0, CardManager.Instance.handCards.Count);

                            Debug.Log($"랜덤 카드 버리기 : 선택된 인덱스 {randomIndex}, 현재 손패 크기 : {CardManager.Instance.handCards.Count}");

                            if (cardIndexCopy < CardManager.Instance.handCards.Count)
                            {
                                if (cardIndex != cardIndexCopy)
                                {
                                    CardManager.Instance.DiscardCard(randomIndex);

                                    //만약 버린 카드의 인덱스가 현재 카드의 인덱스보다 작다면 현재 카드의 인덱스를 1 감소 시켜야함
                                    if (randomIndex < cardIndexCopy)
                                    {
                                        cardIndexCopy--;
                                    }
                                }
                                else if (CardManager.Instance.handCards.Count > 1)
                                {
                                    //다른 카드 선택
                                    int newIndex = (randomIndex + 1) % CardManager.Instance.handCards.Count;
                                    CardManager.Instance.DiscardCard(newIndex);
                                    if (randomIndex < cardIndexCopy)
                                    {
                                        cardIndexCopy--;
                                    }
                                }
                            }
                            else
                            {
                                //cardIndexCopy가 더이상 유효하지 않은 경우 , 아무 카드나 버림
                                CardManager.Instance.DiscardCard(randomIndex);
                            }
                        }
                    }
                    break;

                case CardData.AdditionalEffectType.GainMana:
                    if (CardManager.Instance.playerStats != null)
                    {
                        CardManager.Instance.playerStats.Gainmana(effect.effectAmount);
                        Debug.Log($"마나를{effect.effectAmount}획득 했습니다");
                    }
                    break;

                case CardData.AdditionalEffectType.ReduceEnemyMana:

                    if (CardManager.Instance.EnemyStats != null)
                    {
                        CardManager.Instance.EnemyStats.UseMana(effect.effectAmount);
                        Debug.Log($"적이 마나를{effect.effectAmount}잃었습니다");
                    }
                    break;         
            }
        }

        //효과 적용 후 현재 카드로 버리기

        if (CardManager.Instance != null)
        {
            CardManager.Instance.DiscardCard(cardIndexCopy);
        }

    }

              
}





