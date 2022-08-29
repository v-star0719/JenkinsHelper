using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using SpeechLib;


namespace JenkinsHelp
{
    public partial class MainForm : Form
    {
        private Thread thread;
        private bool isLoaded;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.Init(UpdateText);
            thread = new Thread(WorkingThread.MainThread);
            thread.Start();
            LoadConfig();
            WorkingThread.OutputDevName = ConfigManager.Instance.outputDevice;

            UpdateComboBoxBranch();
            UpdateOutputDevice();
            comboBoxBranch.SelectedIndex = WorkingThread.Branch;
        }

        private void LoadConfig()
        {
            ConfigManager.Instance.Init();

            WorkingThread.OutputDevName = ConfigManager.Instance.outputDevice;
            WorkingThread.JenkinsViewUrls = ConfigManager.Instance.jenkinsViewUrls;
            WorkingThread.TaskAudioTextDict = ConfigManager.Instance.taskAudioTextDict;
            WorkingThread.TaskStatusAudioTextDict = ConfigManager.Instance.taskStatusAudioTextDict;
            WorkingThread.refreshInterval = ConfigManager.Instance.refreshInterval;

            isLoaded = true;
        }

        private void UpdateComboBoxBranch()
        {
            comboBoxBranch.Items.Clear();
            foreach(var url in ConfigManager.Instance.jenkinsViewUrls)
            {
                var endIndex = url.LastIndexOf("/");
                if(endIndex < 0)
                {
                    endIndex = url.Length;
                }

                var starIndex = url.LastIndexOf("/", endIndex - 1);
                starIndex++;
                comboBoxBranch.Items.Add(url.Substring(starIndex, endIndex - starIndex));
            }
        }

        private void UpdateOutputDevice()
        {
            audioOutputSelector.Items.Clear();
            SpVoice voice = new SpVoice();
            foreach(var speechObjectToken in voice.GetAudioOutputs())
            {
                var o = speechObjectToken as SpeechLib.SpObjectToken;
                var desc = o.GetDescription();
                audioOutputSelector.Items.Add(desc);
            }

            var sel = 0;
            for(int i = 0; i<audioOutputSelector.Items.Count; i++)
            {
                object item = (object)audioOutputSelector.Items[i];
                if(item.ToString().Contains(ConfigManager.Instance.outputDevice))
                {
                    sel = i;
                    break;
                }
            }

            audioOutputSelector.SelectedIndex = sel;
        }

        public void UpdateText(string text)
        {
            BeginInvoke(new Action(
                () =>
                {
                    textBox.Text = text;
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.ScrollToCaret();
                }));
        }

        private void ShowMainForm()
        {

            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            UpdateOutputDevice();
        }

        private void HideMainForm()
        {
            this.Hide();
        }

        private void ExitMainForm()

        {
            if(MessageBox.Show("真的要离开我吗", "真的吗",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                   MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                this.notifyIcon.Visible = false;

                this.Close();
                this.Dispose();

                Application.Exit();
                WorkingThread.exist = true;
            }
        }

        ~MainForm()
        {
            WorkingThread.exist = true;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(this.WindowState ==
               FormWindowState.Normal)
            {
                this.WindowState =
                    FormWindowState.Minimized;


                HideMainForm();
            }
            else if(this.WindowState ==
                    FormWindowState.Minimized)
            {

                ShowMainForm();
            }
        }


        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)

            {
                HideMainForm();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            HideMainForm();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideMainForm();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitMainForm();
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            WorkingThread.volume = trackBar.Value;
        }

        private void audioOutputSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = audioOutputSelector.SelectedItem.ToString();
            if (WorkingThread.OutputDevName != sel)
            {
                WorkingThread.OutputDevName = sel;
                ConfigManager.Instance.SetOutputDev(sel);
                ConfigManager.Instance.Save();
            }
        }

        private void comboBoxBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkingThread.Branch  = comboBoxBranch.SelectedIndex;
        }

        private void OnTick(object sender, EventArgs e)
        {
            var diff = WorkingThread.NextRefreshTime.Ticks - DateTime.Now.Ticks;
            diff = diff < 0 ? 0 : diff / 10000000;
            timerLabel.Text = diff.ToString();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            var msg = textBoxMsg.Text;
            if (!string.IsNullOrEmpty(msg))
            {
                WorkingThread.BroadcastMsg(msg);
            }
        }
    }
}
