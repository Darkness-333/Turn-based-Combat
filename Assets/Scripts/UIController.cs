using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public GameObject gameEndText;
    [SerializeField] private TextMeshProUGUI floatingTextPrefab;

    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private Transform playerCanvas;
    [SerializeField] private Button[] playerAbilitiesButtons;
    [SerializeField] private TextMeshProUGUI[] playerAbilitiesTexts;
    [SerializeField] private TextMeshProUGUI[] playerCooldownsTexts;


    [SerializeField] private HealthBar enemyHealthBar;
    [SerializeField] private Transform enemyCanvas;
    [SerializeField] private Button[] enemyAbilitiesButtons;
    [SerializeField] private TextMeshProUGUI[] enemyAbilitiesTexts;
    [SerializeField] private TextMeshProUGUI[] enemyCooldownsTexts;


    public void BindUnits(Unit player, Unit enemy) {
        player.Health
            .Subscribe(health=>playerHealthBar.SetHealth(health,player.MaxHealth))
            .AddTo(this);
        enemy.Health
            .Subscribe(health => enemyHealthBar.SetHealth(health, enemy.MaxHealth))
            .AddTo(this);
    }

    //public void UpdateHealthBars(Unit player, Unit enemy) {
    //    playerHealthBar.SetHealth(player.Health, player.MaxHealth);
    //    enemyHealthBar.SetHealth(enemy.Health, enemy.MaxHealth);
    //}

    public void UpdateEffects(Unit player, Unit enemy) {
        int[] playerEffectsDuration = player.GetEffectsDuration();
        int[] playerAbilitiesCooldowns = player.GetAbilitiesCooldowns();
        for (int i = 0; i < playerAbilitiesTexts.Length; i++) {
            playerAbilitiesTexts[i].text = $"Действует {playerEffectsDuration[i]}\nПерезарядка {playerAbilitiesCooldowns[i]}";
            if (playerAbilitiesCooldowns[i] > 0) {
                playerAbilitiesButtons[i].interactable = false;
            }
            else {
                playerAbilitiesButtons[i].interactable = true;
            }
        }
        for (int i = 0; i < playerCooldownsTexts.Length; i++) {
            playerCooldownsTexts[i].text = playerEffectsDuration[i + 1].ToString();
        }


        int[] enemyEffectsDuration = enemy.GetEffectsDuration();
        int[] enemyAbilitiesCooldowns = enemy.GetAbilitiesCooldowns();
        for (int i = 0; i < enemyAbilitiesTexts.Length; i++) {
            enemyAbilitiesTexts[i].text = $"Действует {enemyEffectsDuration[i]}\nПерезарядка {enemyAbilitiesCooldowns[i]}";

            if (enemyAbilitiesCooldowns[i] > 0) {
                enemyAbilitiesButtons[i].interactable = false;
            }
            else {
                enemyAbilitiesButtons[i].interactable = true;
            }

            if (enemy.lastAbilityId != -1) {
                if (i == enemy.lastAbilityId) {
                    enemyAbilitiesTexts[i].color = Color.green;
                }
                else {
                    enemyAbilitiesTexts[i].color = Color.black;

                }

            }
        }
        for (int i = 0; i < enemyCooldownsTexts.Length; i++) {
            enemyCooldownsTexts[i].text = enemyEffectsDuration[i + 1].ToString();
        }

    }

    public void ShowGameOver() {
        for (int i = 0; i < playerAbilitiesButtons.Length; i++) {
            playerAbilitiesButtons[i].interactable = false;
        }
        gameEndText.SetActive(true);

    }

    public void ShowPlayerDamage(int amount, HealthChangeType type) {
        ShowDamage(amount, type, playerCanvas);
    }

    public void ShowEnemyDamage(int amount, HealthChangeType type) {
        ShowDamage(amount, type, enemyCanvas);
    }

    private void ShowDamage(int amount, HealthChangeType type, Transform canvas) {
        TextMeshProUGUI text = Instantiate(floatingTextPrefab, canvas);
        text.text = "-"+amount;
        Color color = Color.white;
        text.transform.localPosition = new Vector3(-0.5f, 2.5f, 0);
        if (type == HealthChangeType.BarrierAttack) {
            color = new Color(205 / 256f, 168 / 256f, 0);
        }
        else if (type == HealthChangeType.Regeneration) {
            color = new Color(96 / 256f, 243 / 256f, 107 / 256f);
            text.transform.localPosition = new Vector3(0, 2.5f, 0);
            text.text="+"+amount;
        }
        else if (type == HealthChangeType.Burning) {
            color = Color.red;
            text.transform.localPosition = new Vector3(0.5f, 2.5f, 0);
        }
        text.color = color;
        Destroy(text.gameObject,2);
    }
}
