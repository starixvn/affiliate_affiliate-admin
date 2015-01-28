using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ConfigGenerator.Core;
using ConfigGenerator.Services;

namespace ConfigGenerator
{
    /// <summary>
    /// Main Form.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly GeneratorService _generatorService = new GeneratorService();
        private ConfigArgument _configArgument;

        public MainForm()
        {
            InitializeComponent();

            LoadLicensees();
        }

        private void btnSelectAllLicensees_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewLicensee.Items)
            {
                item.Checked = true;
            }
        }

        private void btnDeselectAllLicensees_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewLicensee.Items)
            {
                item.Checked = false;
            }
        }

        private void LoadLicensees()
        {
            var licenses = LicenseeUtility.SupportedLicensees;

            listViewLicensee.Items.Clear();

            foreach (var licensee in licenses)
            {
                listViewLicensee.Items.Add(licensee.Name);
            }
        }

        private void btnGenerateConfig_Click(object sender, EventArgs e)
        {
            var condition = new CheckCondition();

            _configArgument = new ConfigArgument
            {
                SystemType = GetSystemType(condition),
                ConfigurationType = GetConfigurationType(condition),
                EnvironmentType = GetEnvironmentType(condition),
                SelectedLicensees = GetSelectedLicensees(condition)
            };

            if (!IsValidCondition(condition))
            {
                return;
            }

            backgroundWorker1.RunWorkerAsync();
        }

        private bool IsValidCondition(CheckCondition condition)
        {
            var valid = true;

            if (condition.CheckedLicenseeNum == 0)
            {
                MessageBox.Show("Please choose at leat one licensee", "Licensee Warning");
                return false;
            }

            if (condition.CheckedSystemNum == 0)
            {
                MessageBox.Show("Please choose at leat one system", "System Warning");
                return false;
            }

            if (condition.CheckedEnvironmentNum == 0)
            {
                MessageBox.Show("Please choose at leat one environment", "Environment Warning");
                return false;
            }

            if (condition.CheckedConfigurationNum == 0)
            {
                MessageBox.Show("Please choose at leat one configuration type", "Configuration Warning");
                return false;
            }

            return valid;
        }

        private IEnumerable<string> GetSelectedLicensees(CheckCondition condition)
        {
            int checkedSystemNum = 0;
            var licensees = new List<string>();

            foreach (ListViewItem item in listViewLicensee.Items)
            {
                if (item.Checked)
                {
                    checkedSystemNum++;
                    licensees.Add(item.Text);
                }
            }

            condition.CheckedLicenseeNum = checkedSystemNum;

            return licensees;
        }

        private SystemType GetSystemType(CheckCondition condition)
        {
            var systemType = SystemType.None;

            int checkedSystemNum = 0;

            if (chkBoxAdmin.Checked)
            {
                checkedSystemNum++;
                systemType = systemType | SystemType.Admin;
            }

            if (chkBoxUser.Checked)
            {
                checkedSystemNum++;
                systemType = systemType | SystemType.User;
            }

            if (chkBoxBanner.Checked)
            {
                checkedSystemNum++;
                systemType = systemType | SystemType.Banner;
            }

            if (chkBoxLink.Checked)
            {
                checkedSystemNum++;
                systemType = systemType | SystemType.Link;
            }

            if (chkBoxWebService.Checked)
            {
                checkedSystemNum++;
                systemType = systemType | SystemType.WebService;
            }

            condition.CheckedSystemNum = checkedSystemNum;

            return systemType;
        }

        private ConfigurationType GetConfigurationType(CheckCondition condition)
        {
            var config = ConfigurationType.None;

            int checkedConfigNum = 0;

            if (chkBoxWebConfig.Checked)
            {
                checkedConfigNum++;
                config = config | ConfigurationType.WebConfig;
            }

            if (chkBoxWebAppSettingConfig.Checked)
            {
                checkedConfigNum++;
                config = config | ConfigurationType.AppSetting;
            }

            if (chkBoxWebConnectionStringConfig.Checked)
            {
                checkedConfigNum++;
                config = config | ConfigurationType.ConnectionStrings;
            }

            if (chkBoxSpName.Checked)
            {
                checkedConfigNum++;
                config = config | ConfigurationType.SpName;
            }

            condition.CheckedConfigurationNum = checkedConfigNum;

            return config;
        }

        private EnvironmentType GetEnvironmentType(CheckCondition condition)
        {
            var environment = EnvironmentType.None;

            int checkedEnvironmentNum = 0;

            if (chkBoxLocal.Checked)
            {
                checkedEnvironmentNum++;
                environment = environment | EnvironmentType.Local;
            }

            if (chkBoxMaster.Checked)
            {
                checkedEnvironmentNum++;
                environment = environment | EnvironmentType.Master;
            }

            if (chkBoxUat.Checked)
            {
                checkedEnvironmentNum++;
                environment = environment | EnvironmentType.Uat;
            }

            if (chkBoxProduction.Checked)
            {
                checkedEnvironmentNum++;
                environment = environment | EnvironmentType.Production;
            }

            condition.CheckedEnvironmentNum = checkedEnvironmentNum;

            return environment;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoker(delegate
                {
                    btnGenerateConfig.Enabled = false;
                    lblStatus.Text = "Generating Config.";
                });

            _generatorService.Generate(_configArgument);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MoveUtil.MoveConfig(_configArgument.SystemType);

            Invoker(delegate
            {
                btnGenerateConfig.Enabled = true;
                lblStatus.Text = "Generated Config.";
            });
        }

        private void Invoker(MethodInvoker methodToInvoke)
        {
            if (!Disposing && !IsDisposed)
            {
                try
                {
                    Invoke(methodToInvoke);
                }
                catch
                {
                }
            }
        }
    }
}
