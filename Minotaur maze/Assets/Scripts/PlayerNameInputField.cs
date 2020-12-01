using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    private const string PlayerNamePrefKey = "PlayerName";
    public InputField inputField;

    private void Start()
    {
        var defaultName = string.Empty;

        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(PlayerNamePrefKey, value);
    }
}
