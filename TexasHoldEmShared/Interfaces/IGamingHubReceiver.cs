using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHubReceiver
    {
        void OnJoinRoom(PlayerEntity player, int playerCount);
        void OnLeaveRoom(PlayerEntity player, int playerCount);
        void OnGetAllPlayers(PlayerEntity[] playerEntities);
        void OnGameStart(PlayerEntity[] playerEntities, PlayerEntity currentPlayer);
        void OnCancelGameStart();
        void OnQuitGame();
        void OnDoAction(Enums.CommandTypeEnum commandType, PlayerEntity currentPlayer, string actionMessage);
    }
}