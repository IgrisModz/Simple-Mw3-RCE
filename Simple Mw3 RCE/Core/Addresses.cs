namespace Simple_Mw3_RCE.Core
{
    public static class Addresses
    {
        #region Lobby Detection
        // You can also use offset: 0x72B08B
        public static uint IsInGame_a => 0x18D4C64;
        #endregion

        #region Server Info
        public static uint MaxClients_a => 0x89F28C;

        public static uint HostIndex_a => 0x89F290;
        #endregion

        #region In Game
        public static uint G_Grabber_a => 0x89D180;

        public static uint G_Grabber_s => 0x178;

        public static uint G_Entity_a => 0xFCA280;

        public static uint G_Entity_s => 0x280;

        public static uint G_Client_a => 0x110A280;

        public static uint G_Client_s => 0x3980;

        public static uint Client_t_a => 0x17BB210;

        public static uint Client_t_s => 0x68B80;
        public enum Grabber
        {
            Prestige = 0x10,
            Level = 0x0C,
            ClanTagType = 0x7C,
            ClanTag = 0x70,
            EliteClanTag = 0x9C,
            PlaystationName = 0x2C,
            PlaystationName2 = 0x50,
            ExternalIP = 0x11E,
            InternalIP = 0x10C,
            Port = 0x122,
            NatType = 0x144,
            ZIPCode = 0x40,
            XUID = 0x148,
            PartyID = 0x04,
            Micro = 0x75,
            EmblemIndex = 0x20,
            TitleIndex = 0x24,
            TitleType = 0x7D,
            EliteTitleText = 0x7E,
            EliteTitleIndex = 0x9B,
            EliteTitleLevel = 0xA4,
            EliteTitleBackgroundType = 0x9A,
            PrestigeMW = 0xC0,
            PrestigeWAW = 0xB8,
            PrestigeMW2 = 0xB0,
            PrestigeBO = 0xA8,
            LevelMW = 0xC4,
            LevelWAW = 0xBC,
            LevelMW2 = 0xB4,
            LevelBO = 0xAC
        }

        public enum G_EntityStruct
        {
            Invisibility = 0xFC,
            KillOnSpawn = 0x100,
            Godmode = 0x184,

        }

        public enum G_ClientStruct
        {
            Redboxes = 0x10,
            MovementType = 0x35FC,
        }
        #endregion
    }
}
