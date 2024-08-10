using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Meta.XR.BuildingBlocks;
using Meta.XR.MultiplayerBlocks.Shared;
using Meta.XR.MultiplayerBlocks.NGO;

namespace Meta.XR.MultiplayerBlocks.Shared
{
    public class PlayerIdentifier : NetworkBehaviour
    {
        public NetworkVariable<FixedString128Bytes> PlayerName = new NetworkVariable<FixedString128Bytes>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                SetPlayerNameServerRpc(PlayerNameTagSpawnerNGO.Instance.GetLocalPlayerName());
                Debug.Log($"Player name set to {PlayerName.Value}");
            }

            PlayerName.OnValueChanged += OnPlayerNameChanged;
        }

        [ServerRpc]
        private void SetPlayerNameServerRpc(string name)
        {
            PlayerName.Value = new FixedString128Bytes(name);
        }

        private void OnPlayerNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
        {
            Debug.Log($"Player name changed from {previousValue} to {newValue}");
            // You can add additional logic here if needed when the name changes
        }

        public override void OnNetworkDespawn()
        {
            PlayerName.OnValueChanged -= OnPlayerNameChanged;
        }
    }
}