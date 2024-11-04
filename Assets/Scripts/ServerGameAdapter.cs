public class ServerGameAdapter : IGameAdapter {
    private ServerGameManager server;

    public ServerGameAdapter(ClientGameManager clientManager) {
        server = new ServerGameManager(clientManager);
    }

    public void PerformAction(int abilityId) {
        server.PerformAction(abilityId);
    }

    public void PerformEnemyAction() {
        server.PerformEnemyAction();
    }

    public bool CheckGameOver() {
        return server.IsGameOver();
    }

    public void ResetGame() {
        server.ResetGame();
    }

    public Unit GetPlayerUnit() {
        return server.playerUnit;
    }

    public Unit GetEnemyUnit() {
        return server.enemyUnit;
    }

    public bool IsPlayerTurn() {
        return server.IsPlayerTurn();
    }

}
