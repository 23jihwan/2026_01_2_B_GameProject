using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;

    //UI 요소
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    public void Start()
    {
        currentHealth = maxHealth;
    }


    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }
}
