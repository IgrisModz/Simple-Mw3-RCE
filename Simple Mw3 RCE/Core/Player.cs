using IgrisLib;
using System.Threading;

namespace Simple_Mw3_RCE.Core
{
    public class Player
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public PS3API PS3 { get; }

        public RemoteCallExecution RCE { get; }

        public Player(PS3API ps3)
        {
            PS3 = ps3;
            RCE = new RemoteCallExecution(PS3, this);
        }

        public Player GetInfo()
        {
            return new Player(PS3)
            {
                Id = Id,
                Name = PS3.Extension.ReadString(G_Grabber(Addresses.Grabber.PlaystationName2))
            };
        }

        public uint G_Grabber(uint mod)
        {
            return Addresses.G_Grabber_a + mod + (Addresses.G_Grabber_s * Id);
        }

        public uint G_Grabber(Addresses.Grabber mod)
        {
            return Addresses.G_Grabber_a + (uint)mod + (Addresses.G_Grabber_s * Id);
        }

        public uint G_Client(uint mod)
        {
            return Addresses.G_Client_a + mod + (Addresses.G_Client_s * Id);
        }

        public uint G_Client(Addresses.G_ClientStruct mod)
        {
            return Addresses.G_Client_a + (uint)mod + (Addresses.G_Client_s * Id);
        }

        public uint G_Entity(uint mod)
        {
            return Addresses.G_Entity_a + mod + (Addresses.G_Entity_s * Id);
        }

        public uint G_Entity(Addresses.G_EntityStruct mod)
        {
            return Addresses.G_Entity_a + (uint)mod + (Addresses.G_Entity_s * Id);
        }

        public uint Client_t(uint mod)
        {
            return Addresses.Client_t_a + mod + (Addresses.Client_t_s * Id);
        }

        public class RemoteCallExecution
        {
            public PS3API PS3 { get; }

            public Player Player { get; }

            public RemoteCallExecution(PS3API ps3, Player player)
            {
                PS3 = ps3;
                Player = player;
            }

            private void WriteInt32(uint address, int value, int timeMilisecond = 500)
            {
                PS3.Extension.WriteUInt32(0x10055010, address);
                PS3.Extension.WriteInt32(0x10055014, value);
                PS3.Extension.WriteUInt32(0x10055000, 0x02);
                while (PS3.Extension.ReadUInt32(0x10055000) != 0)
                    Thread.Sleep(10);
                Thread.Sleep(timeMilisecond);
            }

            private void WriteUInt32(uint address, uint value, int timeMilisecond = 500)
            {
                PS3.Extension.WriteUInt32(0x10055010, address);
                PS3.Extension.WriteUInt32(0x10055014, value);
                PS3.Extension.WriteUInt32(0x10055000, 0x02);
                while (PS3.Extension.ReadUInt32(0x10055000) != 0)
                    Thread.Sleep(10);
                Thread.Sleep(timeMilisecond);
            }

            public void SetGodmode(DamageFlag flag)
            {
                WriteUInt32(Player.G_Entity(Addresses.G_EntityStruct.Godmode), (uint)flag);
            }

            public void SetMovement(MovementType movementType = MovementType.Noclip)
            {
                WriteUInt32(Player.G_Client(Addresses.G_ClientStruct.MovementType), (uint)movementType);
            }

            public void SetInvisibility(bool state = true)
            {
                WriteUInt32(Player.G_Entity(Addresses.G_EntityStruct.Invisibility), state ? 0xFFFFFFFF : 0x00);
            }

            public void GiveRedboxes(bool state = true)
            {
                WriteUInt32(Player.G_Client(Addresses.G_ClientStruct.Redboxes), state ? 0x1010u : 0x1000);
            }

            public void GiveSpawnKill(bool state = true)
            {
                WriteUInt32(Player.G_Entity(Addresses.G_EntityStruct.KillOnSpawn), state ? 0x1010000u : 0x1000000u);
            }

            public void Suicide()
            {
                WriteUInt32(Player.G_Entity(Addresses.G_EntityStruct.KillOnSpawn), 0x1010000u);
                WriteUInt32(Player.G_Entity(Addresses.G_EntityStruct.KillOnSpawn), 0x1000000u);
            }

            public void SetInfiniteAmmo(bool state = true)
            {
                //Primary
                WriteInt32(Player.G_Client(0x428), state ? int.MaxValue : 1, 700);
                WriteInt32(Player.G_Client(0x3A8), state ? int.MaxValue : 1, 700);
                //Secondary
                WriteInt32(Player.G_Client(0x410), state ? int.MaxValue : 1, 700);
                WriteInt32(Player.G_Client(0x398), state ? int.MaxValue : 1, 700);
                //Grenade
                WriteInt32(Player.G_Client(0x41C), state ? int.MaxValue : 1, 700);
                //Smoke
                WriteInt32(Player.G_Client(0x434), state ? int.MaxValue : 1, 700);
            }
        }

        public void SetGodmode(DamageFlag flag)
        {
            PS3.Extension.WriteUInt32(G_Entity(Addresses.G_EntityStruct.Godmode), (uint)flag);
        }

        public void SetMovement(MovementType movementType = MovementType.UFO)
        {
            PS3.Extension.WriteUInt32(G_Client(Addresses.G_ClientStruct.MovementType), (byte)movementType);
        }

        public void SetInvisibility(bool state = true)
        {
            PS3.Extension.WriteUInt32(G_Entity(Addresses.G_EntityStruct.Invisibility), state ? 0xFFFFFFFF : 0x00);
        }

        public void GiveRedboxes(bool state = true)
        {
            PS3.Extension.WriteUInt32(G_Client(Addresses.G_ClientStruct.Redboxes), state ? 0x1010U : 0x1000);
        }

        public void GiveSpawnKill(bool state = true)
        {
            PS3.Extension.WriteUInt32(G_Entity(Addresses.G_EntityStruct.KillOnSpawn), state ? 0x1010000U : 0x1000000U);
        }

        public void Suicide()
        {
            PS3.Extension.WriteUInt32(G_Entity(Addresses.G_EntityStruct.KillOnSpawn), 0x1010000U);
            Thread.Sleep(500);
            PS3.Extension.WriteUInt32(G_Entity(Addresses.G_EntityStruct.KillOnSpawn), 0x1000000U);
        }

        public void SetInfiniteAmmo(bool state = true)
        {
            // 0x110A6A0
            PS3.Extension.WriteInt32(G_Client(0x420), state ? int.MaxValue : 1);
            // 0x110A6A8
            PS3.Extension.WriteInt32(G_Client(0x428), state ? int.MaxValue : 1);
            // 0x110A628
            PS3.Extension.WriteInt32(G_Client(0x3A8), state ? int.MaxValue : 1);
            // 0x110A6AC
            PS3.Extension.WriteInt32(G_Client(0x42C), state ? int.MaxValue : 1);
            // 0x110A630
            PS3.Extension.WriteInt32(G_Client(0x3B0), state ? int.MaxValue : 1);
            // 0x110A618
            PS3.Extension.WriteInt32(G_Client(0x398), state ? int.MaxValue : 1);
            // 0x110A690
            PS3.Extension.WriteInt32(G_Client(0x410), state ? int.MaxValue : 1);
            // 0x110A694
            PS3.Extension.WriteInt32(G_Client(0x414), state ? int.MaxValue : 1);
            // 0x110A620
            PS3.Extension.WriteInt32(G_Client(0x3A0), state ? int.MaxValue : 1);
            // 0x110A638
            PS3.Extension.WriteInt32(G_Client(0x3B8), state ? int.MaxValue : 1);
            // 0x110A69C
            PS3.Extension.WriteInt32(G_Client(0x41C), state ? int.MaxValue : 1);
            // 0x110A6B4
            PS3.Extension.WriteInt32(G_Client(0x434), state ? int.MaxValue : 1);
            // 0x110A6C0
            PS3.Extension.WriteInt32(G_Client(0x440), state ? int.MaxValue : 1);
        }
    }
}
