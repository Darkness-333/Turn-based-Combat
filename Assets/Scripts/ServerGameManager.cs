using UniRx;
using UnityEngine;

public class ServerGameManager {
    public Unit playerUnit;
    public Unit enemyUnit;
    //private int turn; // 0 - игрок, 1 - ИИ
    public ReactiveProperty<int> Turn { get; private set; }
    private ClientGameManager clientManager;

    public ServerGameManager(ClientGameManager clientManager) {
        this.clientManager = clientManager;
        playerUnit = new Unit(this, 100);
        enemyUnit = new Unit(this, 100);
        Turn = new ReactiveProperty<int>(0);
        //Turn.Subscribe(currentTurn => {
        //    if (currentTurn == 1) {
        //        PerformEnemyAction();
        //    }
        //}).AddTo(clientManager);
        Turn
            .Where(turn => turn == 1 && !IsGameOver()) // Реагируем только на ход врага
            .SelectMany(_ => Observable.Timer(System.TimeSpan.FromSeconds(1))) // Задержка 1 секунда
            .Subscribe(_ => {
                PerformEnemyAction(); // Выполняем действие врага
                clientManager.UpdateUI(); // Обновляем UI клиента

                if (IsGameOver()) {
                    clientManager.uiController.ShowGameOver();
                }
            });
    }

    public void PerformAction(int abilityId) {
        if (Turn.Value == 0) {
            ApplyAbility(playerUnit, enemyUnit, abilityId);
            Turn.Value = 1;
        }
    }

    public void PerformEnemyAction() {
        if (Turn.Value == 1) {
            int enemyAbility = Random.Range(0, 5);
            if (enemyUnit.abilitiesCooldowns[enemyAbility] > 0) {
                enemyAbility = 0;
            }
            //enemyAbility = 0;

            ApplyAbility(enemyUnit, playerUnit, enemyAbility);
            Turn.Value = 0;
        }
    }

    private void ApplyAbility(Unit caster, Unit target, int abilityId) {
        int abilityCooldown = 0;
        switch (abilityId) {
            case 0: // Атака
                target.TakeDamage(8);
                break;
            case 1: // Барьер
                caster.ApplyBarrier(5, 3);
                abilityCooldown = 4;
                break;
            case 2: // Регенерация
                caster.ApplyRegeneration(4, 3);
                abilityCooldown = 5;

                break;
            case 3: // Огненный шар
                target.TakeDamage(5);
                target.ApplyBurning(3, 5);
                abilityCooldown = 5;

                break;
            case 4: // Очищение
                caster.RemoveBurning();
                abilityCooldown = 5;

                break;
        }
        caster.lastAbilityId = abilityId;

        caster.UpdateAbilitiesCooldowns();

        caster.abilitiesCooldowns[abilityId] = abilityCooldown;


        //UpdateEffects(caster);

        if (!IsGameOver())
            UpdateEffects(target);
    }

    public void ShowDamage(Unit target, int amount, HealthChangeType type) {
        clientManager.ShowDamage(target, amount, type);

    }


    private void UpdateEffects(Unit unit) {
        unit.UpdateEffects();
    }

    public bool IsGameOver() {
        return playerUnit.Health.Value <= 0 || enemyUnit.Health.Value <= 0;
    }

    public void ResetGame() {
        playerUnit.Reset();
        enemyUnit.Reset();
        Turn.Value = 0;
    }

    public bool IsPlayerTurn() {
        return Turn.Value == 0;
    }
}
