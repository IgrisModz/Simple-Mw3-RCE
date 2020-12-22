using IgrisLib;
using IgrisLib.Mvvm;
using MahApps.Metro.Controls.Dialogs;
using Simple_Mw3_RCE.Core;
using Simple_Mw3_RCE.Properties;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Mw3_RCE.ViewModels
{
    public sealed partial class MainViewModel
    {
        public MainViewModel()
        {
            Resources = Simple_Mw3_RCE.Resources.Language.SetLanguageDictionary();
            Dialog = DialogCoordinator.Instance;
            switch (Settings.Default.API)
            {
                case "TMAPI":
                    PS3 = new PS3API(new TMAPI());
                    break;
                case "CCAPI":
                    PS3 = new PS3API(new CCAPI());
                    break;
                case "PS3MAPI":
                    PS3 = new PS3API(new PS3MAPI());
                    break;
                default:
                    PS3 = new PS3API(new TMAPI());
                    break;
            }
            Players = new ObservableCollection<Player>();
            SetTMAPICommand = new DelegateCommand(SetTMAPI, CanExecuteSetTMAPI);
            SetCCAPICommand = new DelegateCommand(SetCCAPI, CanExecuteSetCCAPI);
            SetPS3MAPICommand = new DelegateCommand(SetPS3MAPI, CanExecuteSetPS3MAPI);
            ConnectionCommand = new DelegateCommand(Connection, CanExecuteConnection);
            GetPlayersCommand = new DelegateCommand(GetPlayers);
            SetGodmodeCommand = new DelegateCommand<DamageFlag>(flag => SetGodmode(flag), flag => CanExecuteSelected());
            SetMovementCommand = new DelegateCommand<MovementType>(movement => SetMovement(movement), movement => CanExecuteSelected());
            SetInvisibilityCommand = new DelegateCommand<bool>(state => SetInvisibility(state), state => CanExecuteSelected());
            GiveRedboxesCommand = new DelegateCommand<bool>(state => GiveRedboxes(state), state => CanExecuteSelected());
            GiveSpawnKillCommand = new DelegateCommand<bool>(state => GiveSpawnKill(state), state => CanExecuteSelected());
            SuicideCommand = new DelegateCommand(Suicide, CanExecuteSelected);
            SetInfiniteAmmoCommand = new DelegateCommand<bool>(state => SetInfiniteAmmo(state), state => CanExecuteSelected());
        }

        #region Can Execute
        private bool CanExecuteSetTMAPI()
        {
            return PS3 != null && Settings.Default.API != "TMAPI";
        }

        private bool CanExecuteSetCCAPI()
        {
            return PS3 != null && Settings.Default.API != "CCAPI";
        }

        private bool CanExecuteSetPS3MAPI()
        {
            return PS3 != null && Settings.Default.API != "PS3MAPI";
        }

        private bool CanExecuteConnection()
        {
            return PS3 != null && PS3.GetCurrentAPI() != null;
        }

        private bool CanExecuteSelected()
        {
            return IsAttached && SelectedPlayer != null;
        }
        #endregion Can Execute

        #region API
        private void SetTMAPI()
        {
            PS3.ChangeAPI(new TMAPI());
            Settings.Default.API = "TMAPI";
            Settings.Default.Save();
        }

        private void SetCCAPI()
        {
            PS3.ChangeAPI(new CCAPI());
            Settings.Default.API = "CCAPI";
            Settings.Default.Save();
        }

        private void SetPS3MAPI()
        {
            PS3.ChangeAPI(new PS3MAPI());
            Settings.Default.API = "PS3MAPI";
            Settings.Default.Save();
        }
        #endregion API

        #region Connection
        private async void Connection()
        {
            if (PS3.ConnectTarget())
            {
                if (PS3.AttachProcess())
                {
                    if (PS3.GetCurrentGame() == "Modern Warfare® 3")
                    {
                        await Dialog.ShowMessageAsync(this, Resources["success"].ToString(), $"{Resources["successAttached"]} \"{PS3.CurrentGame}\".");
                        IsAttached = true;
                        return;
                    }
                    else
                    {
                        await Dialog.ShowMessageAsync(this, Resources["wrongProcess"].ToString(), $"{Resources["currentProcess"]} \"{PS3.CurrentGame}\".");
                    }
                }
                else
                {
                    await Dialog.ShowMessageAsync(this, Resources["attachFailed"].ToString(), Resources["unableAttach"].ToString());
                }
            }
            else
            {
                await Dialog.ShowMessageAsync(this, Resources["connectFailed"].ToString(), Resources["unableConnect"].ToString());
            }
            IsAttached = false;
        }
        #endregion Connection

        #region Functions
        private bool ImHost()
        {
            return PS3.Extension.ReadUInt32(Addresses.HostIndex_a) == PS3.Extension.ReadUInt32(PS3.Extension.ReadUInt32(0x7BD008) + 0x150);
        }

        private async Task<bool[]> CanExecuteFunction()
        {
            if (PS3.GetAttached())
            {
                IsAttached = true;
                if (PS3.Extension.ReadBool(Addresses.IsInGame_a))
                {
                    bool isHost = ImHost();
                    if (isHost)
                        return new bool[] { true, true };
                    else if (!isHost && PS3.Extension.ReadByte(0xDE810) != 0xF8)
                        return new bool[] { true, false };
                    else
                        await Dialog.ShowMessageAsync(this, Resources["functionsFailed"].ToString(), Resources["installRCE"].ToString());
                }
                else
                    await Dialog.ShowMessageAsync(this, Resources["functionsFailed"].ToString(), Resources["mustBeInGame"].ToString());
            }
            IsAttached = false;
            Players?.Clear();
            SelectedPlayer = null;
            return new bool[] { false, false };
        }

        private async void GetPlayers()
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            Player player;
            Player selectedPlayer = SelectedPlayer;
            Players?.Clear();
            uint maxClient = PS3.Extension.ReadUInt32(Addresses.MaxClients_a);
            for (uint i = 0; i < maxClient; i++)
            {
                player = new Player(PS3) { Id = i }.GetInfo();
                if (!string.IsNullOrEmpty(player.Name) && !string.IsNullOrEmpty(player.Name))
                    Players?.Add(player);
            }
            SelectedPlayer = Players?.SingleOrDefault(p => p.Id == selectedPlayer?.Id);
        }

        private async void SetGodmode(DamageFlag flag)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (ImHost())
                SelectedPlayer.SetGodmode(flag);
            else
                SelectedPlayer.RCE.SetGodmode(flag);
        }

        private async void SetMovement(MovementType movement)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.SetMovement(movement);
            else
                SelectedPlayer.RCE.SetMovement(movement);
        }

        private async void SetInvisibility(bool state)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.SetInvisibility(state);
            else
                SelectedPlayer.RCE.SetInvisibility(state);
        }

        private async void GiveRedboxes(bool state)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.GiveRedboxes(state);
            else
                SelectedPlayer.RCE.GiveRedboxes(state);
        }

        private async void GiveSpawnKill(bool state)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.GiveSpawnKill(state);
            else
                SelectedPlayer.RCE.GiveSpawnKill(state);
        }

        private async void Suicide()
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.Suicide();
            else
                SelectedPlayer.RCE.Suicide();
        }

        private async void SetInfiniteAmmo(bool state)
        {
            bool[] canExecuteFunction = await CanExecuteFunction();
            if (!canExecuteFunction[0])
                return;
            if (canExecuteFunction[1])
                SelectedPlayer.SetInfiniteAmmo(state);
            else
                SelectedPlayer.RCE.SetInfiniteAmmo(state);
        }
        #endregion
    }
}
