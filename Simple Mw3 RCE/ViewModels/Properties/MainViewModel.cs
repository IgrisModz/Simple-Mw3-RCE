using IgrisLib;
using IgrisLib.Mvvm;
using MahApps.Metro.Controls.Dialogs;
using Simple_Mw3_RCE.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace Simple_Mw3_RCE.ViewModels
{
    public sealed partial class MainViewModel : ViewModelBase
    {
        public ResourceDictionary Resources { get; }

        public IDialogCoordinator Dialog { get; }

        public PS3API PS3 { get; }

        public bool IsAttached { get => GetValue(() => IsAttached); set => SetValue(() => IsAttached, value); }

        public ObservableCollection<Player> Players { get => GetValue(() => Players); set => SetValue(() => Players, value); }

        public Player SelectedPlayer { get => GetValue(() => SelectedPlayer); set => SetValue(() => SelectedPlayer, value); }

        public DelegateCommand SetTMAPICommand { get; }

        public DelegateCommand SetCCAPICommand { get; }

        public DelegateCommand SetPS3MAPICommand { get; }

        public DelegateCommand ConnectionCommand { get; }

        public DelegateCommand GetPlayersCommand { get; }

        public DelegateCommand<DamageFlag> SetGodmodeCommand { get; }

        public DelegateCommand<MovementType> SetMovementCommand { get; }

        public DelegateCommand<bool> SetInvisibilityCommand { get; }

        public DelegateCommand<bool> GiveRedboxesCommand { get; }

        public DelegateCommand<bool> GiveSpawnKillCommand { get; }

        public DelegateCommand SuicideCommand { get; }

        public DelegateCommand<bool> SetInfiniteAmmoCommand { get; }
    }
}
