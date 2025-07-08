using WinPrintLimiterInstaller.install;

namespace WinPrintLimiterInstaller
{
    public partial class Form1 : Form
    {
        InstallFormData data = new InstallFormData();
        const string DefaultPath = "C:\\Program Files";
        public Form1()
        {
            InitializeComponent();
            this.InstallDirectoryBox.Text = DefaultPath;
        }

        private void BrowseButton_click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.InstallDirectoryBox.Text = dialog.SelectedPath;
            }


        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            data.InstallPath = this.InstallDirectoryBox.Text;

            if (data.UseXPS)
            {
                data.XPSprinterName = this.pName.Text;
            }


            InstallProccess process = new InstallProccess(data);
            process.Start();
        }

        private void radio_inflight_CheckedChanged(object sender, EventArgs e)
        {
            data.UseXPS = false;
        }

        private void radio_xps_CheckedChanged(object sender, EventArgs e)
        {
            data.UseXPS = true;
        }
    }
}