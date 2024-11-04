public interface IGameAdapter {
    void PerformAction(int abilityId);
    void PerformEnemyAction();
    bool CheckGameOver();
    void ResetGame();
    Unit GetPlayerUnit();
    Unit GetEnemyUnit();
    bool IsPlayerTurn();
}
