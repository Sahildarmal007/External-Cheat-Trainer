using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Celestial;
using Guna.UI2.WinForms;
using LuciferMem;

namespace Celestial_Mods_V3
{
    public partial class Main: Form
    {
        Cosmic memoryfast = new Cosmic();
        private IEnumerable<long> speedResult;
        private IEnumerable<long> wallResult;
        public static readonly Cosmic Memory = new Cosmic();
        public static bool Streaming;
        [DllImport("user32.dll")]
        public static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
        public Main()
        {
            InitializeComponent();
        }

        public void Alert(string msg, Notify.enmType type)
        {
            Notify frm = new Notify();
            frm.showAlert(msg, type);
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            aimbotpanel.BringToFront();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            sniperpanel.BringToFront();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            visualpanel.BringToFront();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            brutalpanel.BringToFront();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            settingpanel.BringToFront();
        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            infopanel.BringToFront();
        }


        public void ExecuteCommand(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = new Process()
            {
                StartInfo = processStartInfo
            })
            {
                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
            }
        }
        private void guna2ToggleSwitch16_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch16.Checked)
            {
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");

            }
            else
            {
                this.ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                this.ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");

            }
        }

        private void guna2ToggleSwitch14_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch14.Checked)
            {
                base.ShowInTaskbar = false;
                Main.Streaming = true;
                Main.SetWindowDisplayAffinity(base.Handle, 17U);

            }
            else
            {
                base.ShowInTaskbar = true;
                Main.Streaming = false;
                Main.SetWindowDisplayAffinity(base.Handle, 0U);

            }
        }

        private async void guna2ToggleSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch5.Checked)
            {
                string[] pocessname = { "HD-Player" };
                bool success = memoryfast.SetProcess(pocessname);

                if (!success)
                {
                    return;
                }

                speedResult = await memoryfast.AoBScan("01 00 00 00 02 2B 07 3D");
                this.Alert("SuccessFully Applied", Notify.enmType.Applied);
               
            }
        }

        private void guna2ToggleSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch6.Checked)
            {
                foreach (long id in speedResult)
                {
                    memoryfast.AobReplace(id, "01 00 00 00 02 2B 70 3D");
                }
            }
            else
            {
                foreach (long id in speedResult)
                {
                    memoryfast.AobReplace(id, "01 00 00 00 02 2B 07 3D");
                }
            }
        }
        public string PID;
        private List<long> lastWalladdress = new List<long>();
        private string wallsearch;
        private string wallreplace;
        private async void guna2ToggleSwitch7_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch7.Checked)
            {
                wallsearch = "09 0E 00 00 80 3F 00 00 80 3F";
                wallreplace = "09 0E 00 00 A0 4F 00 00 80 3F";

                bool k = false;
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                Lucifer.OpenProcess(proc);

                // Scan for the addresses and modify them
                IEnumerable<long> wl = await Lucifer.AoBScan2(wallsearch, writable: true);
                if (wl.Any())
                {
                    Parallel.ForEach(wl, address =>
                    {
                        lastWalladdress.Add(address); // Store modified addresses
                    });
                    k = true;
                }
                if (k)
                {
                    this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                }
            }
        }

        private void guna2ToggleSwitch8_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch8.Checked)
            {
                foreach (var address in lastWalladdress)
                {
                    Lucifer.WriteMemory(address.ToString("X"), "bytes", wallreplace);
                }
            }
            else
            {
                foreach (var address in lastWalladdress)
                {
                    Lucifer.WriteMemory(address.ToString("X"), "bytes", wallsearch); // Restore original value
                }
            }
        }
        private IEnumerable<long> cameraresult;
        private async void guna2ToggleSwitch9_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch9.Checked)
            {
                string[] pocessname = { "HD-Player" };
                bool success = memoryfast.SetProcess(pocessname);

                if (!success)
                {
                    return;
                }

                cameraresult = await memoryfast.AoBScan("00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF");
                this.Alert("SuccessFully Applied", Notify.enmType.Applied);
            }
        }

        private void guna2ToggleSwitch10_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch10.Checked)
            {
                foreach (long id in cameraresult)
                {
                    memoryfast.AobReplace(id, "22 8E C3 40 00 00");
                }
            }
            else
            {
                foreach (long id in cameraresult)
                {
                    memoryfast.AobReplace(id, "00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF");
                }
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            HeadSwitch.Visible = false;
            NeckSwitch.Visible = false;
            ShoulderSwitch.Visible = false;

            HeadSwitch.Location = new Point(568, 19);
            NeckSwitch.Location = new Point(568, 19);
            ShoulderSwitch.Location = new Point(568, 19);
           

            switch (guna2ComboBox3.SelectedIndex)
            {
                case 0:
                    HeadSwitch.Visible = true;
                    break;
                case 1:
                    NeckSwitch.Visible = true;
                    break;
                case 2:
                    ShoulderSwitch.Visible = true;
                    break;
               
            }
        }
        private static Mem Lucifer = new Mem();

        private Dictionary<long, int> orginalValues = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues1 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues2 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues3 = new Dictionary<long, int>();

        long Offset1 = 162;
        long offset2 = 158;
        private async void HeadSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (HeadSwitch.Checked)
            {
                orginalValues.Clear();
                orginalValues1.Clear();
                orginalValues2.Clear();
                orginalValues3.Clear();


                Int64 readOffset = Convert.ToInt64(Offset1);
                Int64 writeOffset = Convert.ToInt64(offset2);

                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                Lucifer.OpenProcess(proc);

                var result = await Lucifer.AoBScan(0x0000000000000000, 0x00007fffffffffff, "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43", true, true);

                if (result.Count() != 0)
                {
                    foreach (var CurrentAddress in result)
                    {
                        Int64 addressToSave = CurrentAddress + writeOffset;
                        var currentBytes = Lucifer.SunIsKind(addressToSave.ToString("X"), sizeof(int));
                        int currentValue = BitConverter.ToInt32(currentBytes, 0); orginalValues[addressToSave] = currentValue;
                        Int64 addressToSave9 = CurrentAddress + readOffset;

                        var currentBytes9 = Lucifer.SunIsKind(addressToSave9.ToString("X"), sizeof(int));
                        int currentValue9 = BitConverter.ToInt32(currentBytes9, 0); orginalValues1[addressToSave9] = currentValue9;
                        Int64 headbytes = CurrentAddress + readOffset;
                        Int64 chestbytes = CurrentAddress + writeOffset;

                        var bytes = Lucifer.SunIsKind(headbytes.ToString("X"), sizeof(int));
                        int Read = BitConverter.ToInt32(bytes, 0);
                        var bytes2 = Lucifer.SunIsKind(chestbytes.ToString("X"), sizeof(int));
                        int Read2 = BitConverter.ToInt32(bytes2, 0);

                        Lucifer.WriteMemory(chestbytes.ToString("X"), "int", Read.ToString());
                        Lucifer.WriteMemory(headbytes.ToString("X"), "int", Read2.ToString());

                        Int64 addressToSave1 = CurrentAddress + writeOffset;
                        var currentBytes1 = Lucifer.SunIsKind(addressToSave1.ToString("X"), sizeof(int));
                        int curentValue1 = BitConverter.ToInt32(currentBytes1, 0); orginalValues2[addressToSave1] = curentValue1;

                        Int64 addressToSave19 = CurrentAddress + readOffset;
                        var currentBytes19 = Lucifer.SunIsKind(addressToSave19.ToString("X"), sizeof(int));
                        int currentValues19 = BitConverter.ToInt32(currentBytes19, 0); orginalValues3[addressToSave19] = currentValues19;
                    }
                    orginalValues.Clear();
                    orginalValues1.Clear();
                    orginalValues2.Clear();
                    orginalValues3.Clear();
                    this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                }

            }
        }

        private Dictionary<long, int> orginalValues5 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues6 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues7 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues8 = new Dictionary<long, int>();

        long Offset3 = 92;
        long offset4 = 40;
        private async void NeckSwitch_CheckedChanged(object sender, EventArgs e)
        {
            orginalValues5.Clear();
            orginalValues6.Clear();
            orginalValues7.Clear();
            orginalValues8.Clear();

          
            Int64 readOffset = Convert.ToInt64(Offset3);
            Int64 writeOffset = Convert.ToInt64(offset4);

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            Lucifer.OpenProcess(proc);

            var result = await Lucifer.AoBScan(0x0000000000000000, 0x00007fffffffffff, "00 00 A5 43 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 80 BF", true, true);

            if (result.Count() != 0)
            {
                foreach (var CurrentAddress in result)
                {
                    Int64 addressToSave = CurrentAddress + writeOffset;
                    var currentBytes = Lucifer.SunIsKind(addressToSave.ToString("X"), sizeof(int));
                    int currentValue = BitConverter.ToInt32(currentBytes, 0); orginalValues5[addressToSave] = currentValue;
                    Int64 addressToSave9 = CurrentAddress + readOffset;

                    var currentBytes9 = Lucifer.SunIsKind(addressToSave9.ToString("X"), sizeof(int));
                    int currentValue9 = BitConverter.ToInt32(currentBytes9, 0); orginalValues6[addressToSave9] = currentValue9;
                    Int64 headbytes = CurrentAddress + readOffset;
                    Int64 chestbytes = CurrentAddress + writeOffset;

                    var bytes = Lucifer.SunIsKind(headbytes.ToString("X"), sizeof(int));
                    int Read = BitConverter.ToInt32(bytes, 0);
                    var bytes2 = Lucifer.SunIsKind(chestbytes.ToString("X"), sizeof(int));
                    int Read2 = BitConverter.ToInt32(bytes2, 0);

                    Lucifer.WriteMemory(chestbytes.ToString("X"), "int", Read.ToString());
                    Lucifer.WriteMemory(headbytes.ToString("X"), "int", Read2.ToString());

                    Int64 addressToSave1 = CurrentAddress + writeOffset;
                    var currentBytes1 = Lucifer.SunIsKind(addressToSave1.ToString("X"), sizeof(int));
                    int curentValue1 = BitConverter.ToInt32(currentBytes1, 0); orginalValues7[addressToSave1] = curentValue1;

                    Int64 addressToSave19 = CurrentAddress + readOffset;
                    var currentBytes19 = Lucifer.SunIsKind(addressToSave19.ToString("X"), sizeof(int));
                    int currentValues19 = BitConverter.ToInt32(currentBytes19, 0); orginalValues8[addressToSave19] = currentValues19;
                }
                orginalValues5.Clear();
                orginalValues6.Clear();
                orginalValues7.Clear();
                orginalValues8.Clear();
                this.Alert("SuccessFully Applied", Notify.enmType.Applied);
            }
        }

        private Dictionary<long, int> orginalValues9 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues10 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues11 = new Dictionary<long, int>();
        private Dictionary<long, int> orginalValues12 = new Dictionary<long, int>();

        long Offset5 = 92;
        long offset6 = 40;
        private async void ShoulderSwitch_CheckedChanged(object sender, EventArgs e)
        {
            orginalValues9.Clear();
            orginalValues10.Clear();
            orginalValues11.Clear();
            orginalValues12.Clear();

           

            Int64 readOffset = Convert.ToInt64(Offset5);
            Int64 writeOffset = Convert.ToInt64(offset6);

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            Lucifer.OpenProcess(proc);

            var result = await Lucifer.AoBScan(0x0000000000000000, 0x00007fffffffffff, "00 00 A5 43 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 80 BF", true, true);

            if (result.Count() != 0)
            {
                foreach (var CurrentAddress in result)
                {
                    Int64 addressToSave = CurrentAddress + writeOffset;
                    var currentBytes = Lucifer.SunIsKind(addressToSave.ToString("X"), sizeof(int));
                    int currentValue = BitConverter.ToInt32(currentBytes, 0); orginalValues9[addressToSave] = currentValue;
                    Int64 addressToSave9 = CurrentAddress + readOffset;

                    var currentBytes9 = Lucifer.SunIsKind(addressToSave9.ToString("X"), sizeof(int));
                    int currentValue9 = BitConverter.ToInt32(currentBytes9, 0); orginalValues10[addressToSave9] = currentValue9;
                    Int64 headbytes = CurrentAddress + readOffset;
                    Int64 chestbytes = CurrentAddress + writeOffset;

                    var bytes = Lucifer.SunIsKind(headbytes.ToString("X"), sizeof(int));
                    int Read = BitConverter.ToInt32(bytes, 0);
                    var bytes2 = Lucifer.SunIsKind(chestbytes.ToString("X"), sizeof(int));
                    int Read2 = BitConverter.ToInt32(bytes2, 0);

                    Lucifer.WriteMemory(chestbytes.ToString("X"), "int", Read.ToString());
                    Lucifer.WriteMemory(headbytes.ToString("X"), "int", Read2.ToString());

                    Int64 addressToSave1 = CurrentAddress + writeOffset;
                    var currentBytes1 = Lucifer.SunIsKind(addressToSave1.ToString("X"), sizeof(int));
                    int curentValue1 = BitConverter.ToInt32(currentBytes1, 0); orginalValues11[addressToSave1] = curentValue1;

                    Int64 addressToSave19 = CurrentAddress + readOffset;
                    var currentBytes19 = Lucifer.SunIsKind(addressToSave19.ToString("X"), sizeof(int));
                    int currentValues19 = BitConverter.ToInt32(currentBytes19, 0); orginalValues12[addressToSave19] = currentValues19;
                }
                orginalValues9.Clear();
                orginalValues10.Clear();
                orginalValues11.Clear();
                orginalValues12.Clear();
                this.Alert("SuccessFully Applied", Notify.enmType.Applied);

            }
        }


        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            Switch2x.Location = new Point(568, 19);
            Switch4x.Location = new Point(568, 19);
            
            switch (guna2ComboBox3.SelectedIndex)
            {
                case 0:
                    Switch2x.Visible = true;
                    break;
                case 1:
                    Switch4x.Visible = true;
                    break;
               
            }
        }

        private async void Switch2x_CheckedChanged(object sender, EventArgs e)
        {
            if (Switch2x.Checked)
            {
                List<string> searchList = new List<string>
{
                "93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 3F 01 00 00 00",
                "F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 3F 01",

};

                List<string> replaceList = new List<string>
{
                "93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 FF FF 00 00 F0 3F 00 00 80 3F 01 00 00 00",
                "F0 41 00 00 48 42 00 00 00 3F 33 33 FF FF 00 00 D0 3F 00 00 80 3F 01",

};

                bool k = false;

             
                Memory.SetProcess(new string[] { "HD-Player" });

                int i2 = 22000000;

                for (int j = 0; j < searchList.Count; j++)
                {
                    IEnumerable<long> cu = await Memory.AoBScan(searchList[j]);
                    string u = "0x" + cu.FirstOrDefault().ToString("X");

                    if (cu.Count() != 0)
                    {
                        for (int i = 0; i < cu.Count(); i++)
                        {
                            i2++;
                            Memory.AobReplace(cu.ElementAt(i), replaceList[j]);
                        }
                        k = true;
                    }
                }

                if (k == true)
                {
                    this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                }
                else
                {

                }

            }
        }
            

        private async void Switch4x_CheckedChanged(object sender, EventArgs e)
        {
            if (Switch4x.Checked)
            {
                List<string> searchList = new List<string>
{
                "93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 3F 01 00 00 00",
                "F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 3F 01",

};

                List<string> replaceList = new List<string>
{
                "93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 5C 01 00 00 00",
                "F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 5C 01",

};

                bool k = false;

                
                Memory.SetProcess(new string[] { "HD-Player" });

                int i2 = 22000000;

                for (int j = 0; j < searchList.Count; j++)
                {
                    IEnumerable<long> cu = await Memory.AoBScan(searchList[j]);
                    string u = "0x" + cu.FirstOrDefault().ToString("X");

                    if (cu.Count() != 0)
                    {
                        for (int i = 0; i < cu.Count(); i++)
                        {
                            i2++;
                            Memory.AobReplace(cu.ElementAt(i), replaceList[j]);
                        }
                        k = true;
                    }
                }

                if (k == true)
                {
                    this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                }
                else
                {

                }

            }
        }

        private async void guna2ToggleSwitch12_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch12.Checked)
            {
                string search = "7A 44 F0 48 2D E9 10 B0 8D E2 02 8B 2D ED 08 D0 4D E2 00 50 A0 E1 10 1A 08 EE 08 40 95 E5 00 00 54 E3 06";
                string replace = "7A 00";

                bool k = false;
             
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void guna2ToggleSwitch13_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch13.Checked)
            {
                string search = "C0 3F 00 00 00 3F 00 00 80 3F 00 00 00 40 CD CC CC 3D 01 00 00 00 CD CC CC 3D 01";
                string replace = "00 00 00 00 00 3F 00 00 80 3F 00 00 00 40 CD CC CC 3D 01 00 00 00 CD CC CC 3D 01";

                bool k = false;
              
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void guna2ToggleSwitch17_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch17.Checked)
            {
                string search = "6D 00 00 EB 00 0A B7 EE 10 0A 01 EE 00 0A 31 EE 10 5A 01 EE 00 0A 21 EE 10 0A 10 EE 30 88 BD E8 F0 48 2D E9 10 B0 8D E2 02 8B 2D ED 00 40 A0 E1 74 01 9F E5 00 00 8F E0 00 00 D0 E5 00 00 50 E3 06 00 00 1A 64 01 9F E5 00 00 9F E7 00 00 90 E5";
                string replace = "00 00 00 EB 00 0A B7 EE 10 0A";

                bool k = false;
              
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            RedChams.Location = new Point(568, 19);
            BlueChams.Location = new Point(568, 19);
            TransChams.Location = new Point(568, 19);

            switch (guna2ComboBox3.SelectedIndex)
            {
                case 0:
                    RedChams.Visible = true;
                    break;
                case 1:
                    BlueChams.Visible = true;
                    break;
                case 2:
                    TransChams.Visible = true;
                    break;
            }
        }

        private static void LuciferFallenAngel(string resourceName, string outputPath)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found.");
                }
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    byte[] buffer = new byte[resourceStream.Length];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        const uint PROCESS_CREATE_THREAD = 0x2;
        const uint PROCESS_QUERY_INFORMATION = 0x400;
        const uint PROCESS_VM_OPERATION = 0x8;
        const uint PROCESS_VM_WRITE = 0x20;
        const uint PROCESS_VM_READ = 0x10;
        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 4;

        private void RedChams_CheckedChanged(object sender, EventArgs e)
        {
            if (RedChams.Checked)
            {
                string processName = "HD-Player";
                string dllResourceName = "Celestial_Mods_V3.Properties.Red.dll";
                string tempDllPath = Path.Combine(Path.GetTempPath(), "Red.dll");
                LuciferFallenAngel(dllResourceName, tempDllPath);
                Process[] targetProcesses = Process.GetProcessesByName(processName);
                if (targetProcesses.Length == 0)
                {
                    Console.WriteLine($"Waiting for {processName}.exe...");
                }
                if (targetProcesses.Length == 0)
                {
                    MessageBox.Show("Fuck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process targetProcess = targetProcesses[0];
                    IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                    IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                    IntPtr bytesWritten;
                    WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                    CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                }
            }
        }

        private void BlueChams_CheckedChanged(object sender, EventArgs e)
        {
            if (BlueChams.Checked)
            {
                string processName = "HD-Player";
                string dllResourceName = "Celestial_Mods_V3.Properties.blue.dll";
                string tempDllPath = Path.Combine(Path.GetTempPath(), "blue.dll");
                LuciferFallenAngel(dllResourceName, tempDllPath);
                Process[] targetProcesses = Process.GetProcessesByName(processName);
                if (targetProcesses.Length == 0)
                {
                    Console.WriteLine($"Waiting for {processName}.exe...");
                }
                if (targetProcesses.Length == 0)
                {
                    MessageBox.Show("Fuck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process targetProcess = targetProcesses[0];
                    IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                    IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                    IntPtr bytesWritten;
                    WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                    CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                }
            }
        }

        private void TransChams_CheckedChanged(object sender, EventArgs e)
        {
            if (TransChams.Checked)
            {
                string processName = "HD-Player";
                string dllResourceName = "Celestial_Mods_V3.Properties.transparent.dll";
                string tempDllPath = Path.Combine(Path.GetTempPath(), "transparent.dll");
                LuciferFallenAngel(dllResourceName, tempDllPath);
                Process[] targetProcesses = Process.GetProcessesByName(processName);
                if (targetProcesses.Length == 0)
                {
                    Console.WriteLine($"Waiting for {processName}.exe...");
                }
                if (targetProcesses.Length == 0)
                {
                    MessageBox.Show("Fuck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process targetProcess = targetProcesses[0];
                    IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                    IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                    IntPtr bytesWritten;
                    WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                    CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                }
            }
        }

        private void guna2ToggleSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch4.Checked)
            {
                string processName = "HD-Player";
                string dllResourceName = "Celestial_Mods_V3.Properties.Sun.dll";
                string tempDllPath = Path.Combine(Path.GetTempPath(), "Sun.dll");
                LuciferFallenAngel(dllResourceName, tempDllPath);
                Process[] targetProcesses = Process.GetProcessesByName(processName);
                if (targetProcesses.Length == 0)
                {
                    Console.WriteLine($"Waiting for {processName}.exe...");
                }
                if (targetProcesses.Length == 0)
                {
                    MessageBox.Show("Fuck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process targetProcess = targetProcesses[0];
                    IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                    IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                    IntPtr bytesWritten;
                    WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                    CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                }
            }
        }

        
        private async void guna2ToggleSwitch19_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch13.Checked)
            {
                string search = "10 0A 18 EE 04 8B BD EC F0 88 BD E8 00 00 55 E3 01 00 00 1A 00 00 A0 E3 A8 9B AB EB 05";
                string replace = "82 0E 43 E3 04 8B BD EC F0 88 BD E8 00 00 55 E3 01 00 00 1A 00 00 A0 E3 A8 9B AB EB 05";

                bool k = false;
              
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void guna2ToggleSwitch20_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch13.Checked)
            {
                string search = "B4 43 DB 0F 49 40 10 2A 00 EE 00 10 80 E5 10 3A 01 EE 14 10 80 E5 00 2A 30 EE 00 10 00 E3 41 3A 30 EE 80 1F 4B E3 01 0A 30";
                string replace = "B4 43 00 00 A0 40 10 2A 00 EE 00 10 80 E5 10 3A 01 EE 14 10 80 E5 00 2A 30 EE 00 10 00 E3 41 3A 30 EE 80 1F 4B E3 01 0A 30";

                bool k = false;
             
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ToggleSwitch15_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = guna2ToggleSwitch15.Checked;
        }

        private async void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                string search = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40";
                string replace = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 FF FF";

                bool k = false;
             
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch2.Checked)
            {
                string search = "3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F ?? ?? ?? 3F ?? ?? ?? 3F 00 00 80 3F 00 00 00 00 ?? ?? ?? 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00";
                string replace = "3C 00 00 80 3C";

                bool k = false;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }
        private async void guna2ToggleSwitch22_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch22.Checked)
            {
                string search = "01 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 CB 00 00 00";
                string replace = "00";

                bool k = false;

                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch3.Checked)
            {
                string search = "E3 06 00 A0 E1 18 D0 4B E2 02 8B BD EC 70 8C BD E8 07 E3 F0 06 18 C8 D7 06 DF E2 F0 06 C8 94 D6";
                string replace = "E3 01 00 A0 E3";

                bool k = false;
              
                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }
        private async void guna2ToggleSwitch11_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch11.Checked)
            {
                string search = "7A 44 28 87";
                string replace = "00 00";

                bool k = false;

                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }
        private async void guna2ToggleSwitch18_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch11.Checked)
            {
                string search = "10 4C 2D E9 08 B0 8D E2 0C 01 9F E5";
                string replace = "01 00 A0 E3 1E FF 2F E1";

                bool k = false;

                if (Process.GetProcessesByName("HD-Player").Length > 0)
                {
                    Memory.SetProcess(new string[] { "HD-Player" });

                    IEnumerable<long> wl = await Memory.AoBScan(search);

                    if (wl.Any())
                    {
                        foreach (var address in wl)
                        {
                            Memory.AobReplace(address, replace);
                        }
                        k = true;
                    }
                    if (k)
                    {
                        this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                    }
                    else
                    {

                    }
                }
            }
        }
        private async void guna2ToggleSwitch21_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch21.Checked)
            {
                List<string> searchList = new List<string>
{
                "30 48 2D E9 08 B0 8D E2 92 DF 4D E2 01 DB 4D E2",
                "00 48 2D E9 0D B0 A0 E1 10 D0 4D E2 6C 20 9F E5",
                "00 48 2D E9 0D B0 A0 E1 18 D0 4D E2 04 00 0B E5",
                "02 20 9F E7 00 20 92 E5 04 20 0B E5 04 00 8D E5",
                "29 27 FD EB 0B D0 A0 E1 00 88 BD E8 00 48 2D E9",
                "E5 EE FC EB 02 EC 8D E2 99 00 8E E2 30 10 9D E5"

};

                List<string> replaceList = new List<string>
{
                "00 00 A0 E1 1E FF 2F E1",
                "00 00 A0 E1 1E FF 2F E1",
                "00 00 A0 E1 1E FF 2F E1",
                "00 00 A0 E3 1E FF 2F E1",
                "00 00 A0 E3 1E FF 2F E1",
                "00 00 A0 E3 1E FF 2F E1"
};

                bool k = false;

                Memory.SetProcess(new string[] { "HD-Player" });

                int i2 = 22000000;

                for (int j = 0; j < searchList.Count; j++)
                {
                    IEnumerable<long> cu = await Memory.AoBScan(searchList[j]);
                    string u = "0x" + cu.FirstOrDefault().ToString("X");

                    if (cu.Count() != 0)
                    {
                        for (int i = 0; i < cu.Count(); i++)
                        {
                            i2++;
                            Memory.AobReplace(cu.ElementAt(i), replaceList[j]);
                        }
                        k = true;
                    }
                }

                if (k == true)
                {
                    this.Alert("SuccessFully Applied", Notify.enmType.Applied);
                }
               
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login aa = new Login();
            aa.Show();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if the Delete key is pressed
            if (keyData == Keys.Delete)
            {

                Application.Exit();
                return true;
            }
           

            // Let the base class handle other keys
            return base.ProcessCmdKey(ref msg, keyData);
        }

        
    }
}

