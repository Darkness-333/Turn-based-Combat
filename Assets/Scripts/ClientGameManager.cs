using UnityEngine;
using System.Collections;

public class ClientGameManager : MonoBehaviour {
    public IGameAdapter gameAdapter;
    public UIController uiController;
    private float enemyTurnDelay = 1;

    private void Start() {
        gameAdapter = new ServerGameAdapter(this);
        uiController.BindUnits(gameAdapter.GetPlayerUnit(), gameAdapter.GetEnemyUnit());
        UpdateUI();

    }

    public void OnAbilitySelected(int abilityId) {
        if (!gameAdapter.IsPlayerTurn()) return;

        gameAdapter.PerformAction(abilityId);
        UpdateUI();

        if (gameAdapter.CheckGameOver()) {
            uiController.ShowGameOver();
        }
        //else {
        //    StartCoroutine(DelayedEnemyTurn());
        //}
    }

    private IEnumerator DelayedEnemyTurn() {
        yield return new WaitForSeconds(enemyTurnDelay);

        gameAdapter.PerformEnemyAction();
        UpdateUI();

        if (gameAdapter.CheckGameOver()) {
            uiController.ShowGameOver();
        }
    }

    public void OnRestartButton() {
        gameAdapter.ResetGame();
        UpdateUI();
        uiController.gameEndText.SetActive(false);
    }

    public void UpdateUI() {
        //uiController.UpdateHealthBars(gameAdapter.GetPlayerUnit(), gameAdapter.GetEnemyUnit());
        uiController.UpdateEffects(gameAdapter.GetPlayerUnit(), gameAdapter.GetEnemyUnit());
    }

    public void ShowDamage(Unit target, int amount, HealthChangeType type) {
        if (target == gameAdapter.GetPlayerUnit()) {
            uiController.ShowPlayerDamage(amount, type);

        }
        else {
            uiController.ShowEnemyDamage(amount, type);

        }
    }
}
