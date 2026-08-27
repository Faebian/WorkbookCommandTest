using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornOne.Abstractions.Workbook;

namespace WorkbookCommandTest
{
    public sealed class CommandTestPlugin :
        IWorkbookPlugin,
        IDisposable
    {
        private IWorkbookHost _host;
        private Panel _panel;
        private TextBox _commandBox;
        private Button _sendButton;

        public string Name => "Command Test";
        public string Version => "0.1";

        public void Initialize(string jsonConfig, IWorkbookHost host)
        {
            _host = host;

            _host.AppendLog(
                "CommandTest",
                "Info",
                "Command test workbook initialized");
        }

        public void BuildUI()
        {
            _panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            _host.Theme.ApplyBaseStyles(_panel);

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5),
                WrapContents = false
            };

            _host.Theme.ApplyBaseStyles(layout);

            _commandBox = new TextBox
            {
                Width = 600,
                Height = 30,
                Font = new Font("Consolas", 12),
                Text = "WriteMessage(Hello from workbook!)"
            };

            _sendButton = new Button
            {
                Width = 100,
                Height = 30,
                Text = "Send!"
            };

            _sendButton.Click += SendButton_Click;

            layout.Controls.Add(_commandBox);
            layout.Controls.Add(_sendButton);

            _panel.Controls.Add(layout);

            _host.AddWidget(_panel, "CommandTest");
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string command = _commandBox.Text;

            if (string.IsNullOrWhiteSpace(command))
                return;

            try
            {
                _host.AppendLog(
                    "CommandTest",
                    "Info",
                    $"> {command}");

                _host.ExecutePythonCommand(command);
            }
            catch (Exception ex)
            {
                _host.AppendLog(
                    "CommandTest",
                    "Warning",
                    ex.ToString());
            }
        }

        public void OnShown()
        {
            _commandBox?.Focus();
        }

        public void OnHidden()
        {
        }

        public void Dispose()
        {
        }
    }
}