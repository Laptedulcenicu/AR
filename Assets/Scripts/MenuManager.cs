﻿
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header(" — -Menus — -")]
    public GameObject mainMenu;
    public GameObject lobbyMenu;
    [Header(" — -Main Menu — -")]
    public Button createRoomBtn;
    public Button joinRoomBtn;
    [Header(" — -Lobby Menu — -")]
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerList;
    public Button startGameBtn;

    private void Start()
    {
        createRoomBtn.interactable = false;
        joinRoomBtn.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
    }

    void SetMenu(GameObject menu)
    {
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void OnCreateRoomBtn(TextMeshProUGUI roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
        roomName.text = roomNameInput.text;
    }

    public void OnJoinRoomBtn(TextMeshProUGUI roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
        roomName.text = roomNameInput.text;
    }

    public void OnPlayerNameUpdate(TextMeshProUGUI playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        SetMenu(lobbyMenu);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI()
    {
        playerList.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                playerList.text += player.NickName + " (Host) \n";
            }
            else
            {
                playerList.text += player.NickName + "\n";
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.interactable = true;
        }
        else
        {
            startGameBtn.interactable = false;
        }
    }

    public void OnLeaveLobbyBtn()
    {
        PhotonNetwork.LeaveRoom();
        SetMenu(mainMenu);
    }

    public void OnStartGameBtn()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}