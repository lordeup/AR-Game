using UnityEngine;

public class PlayerSelectionManager : MonoBehaviour
{
    public static PlayerType PlayerType = PlayerType.Mage;

    public void MagePlayerSelection()
    {
        PlayerSelection(PlayerType.Mage);
    }

    public void WarriorPlayerSelection()
    {
        PlayerSelection(PlayerType.Warrior);
    }

    public void SpectatorPlayerSelection()
    {
        PlayerSelection(PlayerType.Spectator);
    }

    private static void PlayerSelection(PlayerType playerType)
    {
        PlayerType = playerType;
        SceneController.LoadScene("Lobby");
    }
}
