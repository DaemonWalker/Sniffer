using SharpPcap;
using Sniffer.UI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sniffer.UI
{
    public partial class MainForm : Form
    {
        private ICaptureDevice selectedDevice;
        private string localIP;
        private List<PackageModel> list = new List<PackageModel>();
        private string selectFromIP;
        private string selectToIP;
        private string selectFromPort;
        private string selectToPort;
        private static object lockPortObj = new object();
        private static object lockIPObj = new object();
        private static object lockContent = new object();
        private static int pageSize;
        private static int nowPage;
        private static int maxPage;
        private delegate void DelegateNoArgs(PackageModel pm);
        private delegate void DelegateSetDataSource(DataGridView dgv, PackageModel pm, IList<string> props, object lockObj);
        private delegate void DelegateDgvIenumILStringObject(DataGridView dgv, IEnumerable<IList<string>> contents, object lockObj);
        private delegate void DelegateControlString(Control control, string text);
        public MainForm()
        {
            InitializeComponent();
        }

        private void drpDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedDevice != null && selectedDevice.Started)
            {
                selectedDevice.Close();
            }
            if (drpDevices.SelectedIndex == -1)
            {
                return;
            }
            var index = drpDevices.SelectedIndex;
            selectedDevice = CaptureDeviceList.Instance[index];
            selectedDevice.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);
            int readTimeoutMilliseconds = 1000;
            selectedDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            string filter = "ip and tcp";
            selectedDevice.Filter = filter;

            Task.Run(() =>
            {
                selectedDevice.Capture();
            });


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            drpDevices.DataSource = CaptureDeviceList.Instance;
            drpDevices.DisplayMember = "Description";
            drpDevices.ValueMember = "";
            drpDevices.SelectedIndex = -1;
            this.drpDevices.SelectedIndexChanged += this.drpDevices_SelectedIndexChanged;
            drpEncoding.SelectedIndex = 0;

            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.localIP = ipa.ToString();
                }
            }
        }

        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            var len = e.Packet.Data.Length;

            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            var tcpPacket = (PacketDotNet.TcpPacket)packet.Extract(typeof(PacketDotNet.TcpPacket));
            if (tcpPacket != null)
            {
                var ipPacket = (PacketDotNet.IpPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int dstPort = tcpPacket.DestinationPort;

                var model = new PackageModel()
                {
                    Time = time.ToLocalTime(),
                    FromIP = srcIp.ToString(),
                    FromPort = srcPort.ToString(),
                    ToIP = dstIp.ToString(),
                    ToPort = dstPort.ToString(),
                    Data = tcpPacket.PayloadData
                };
                if (model.FromIP == this.localIP)
                {
                    model.ProcName = UtilMethods.ShowPort(model.FromPort);
                }
                else
                {
                    model.ProcName = UtilMethods.ShowPort(model.ToPort);
                }
                list.Add(model);
                //label2.Text = list.Count.ToString();
                AddDvgData(this.dgvIP, model, new List<string>() { model.FromIP, model.ToIP }, lockIPObj);
                if (this.selectFromIP == model.FromIP && this.selectToIP == model.ToIP)
                {
                    AddDvgData(this.dgvPort, model, new List<string>() { model.FromPort, model.ToPort, model.ProcName }, lockPortObj);
                }
                if (this.selectFromPort == model.FromPort && this.selectToPort == model.ToPort)
                {
                    AddContentPackage(model);
                }
            }
        }

        private void AddDvgData(DataGridView dgv, PackageModel pm, IList<string> props, object lockObj)
        {
            if (dgv.InvokeRequired)
            {
                var foo = new DelegateSetDataSource(this.AddDvgData);
                this.Invoke(foo, new object[] { dgv, pm, props, lockObj });
            }
            else
            {
                lock (lockObj)
                {
                    var needAdd = true;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        var same = true;
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Value == null || row.Cells[i].Value.ToString() != props[i])
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                        {
                            needAdd = false;
                            break;
                        }
                    }
                    if (needAdd)
                    {
                        var row = dgv.Rows.Add();
                        for (int i = 0; i < props.Count; i++)
                        {
                            dgv.Rows[dgvIP.Rows.Count - 1].Cells[i].Value = props[i];
                        }
                    }
                }
            }
        }

        private void AddContentPackage(PackageModel pm)
        {
            if (this.InvokeRequired)
            {
                var foo = new DelegateNoArgs(this.AddContentPackage);
                this.Invoke(foo, pm);
            }
            else
            {
                lock (lockContent)
                {
                    txtContent.Text += $"{pm.Time.ToString("HH:mm:ss")} : {UtilMethods.GetContent(pm.Data, chkToBase64.Checked)}\n";
                }
            }
        }

        private void RefreshDgv(DataGridView dgv, IEnumerable<IList<string>> contents, object lockObj)
        {
            if (dgv.InvokeRequired)
            {
                var foo = new DelegateDgvIenumILStringObject(this.RefreshDgv);
                this.Invoke(foo, dgv, contents);
            }
            else
            {
                lock (lockObj)
                {
                    dgv.Rows.Clear();
                    foreach (var item in contents)
                    {
                        dgv.Rows.Add();
                        for (int i = 0; i < dgv.ColumnCount; i++)
                        {
                            dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value = item[i];
                        }
                    }
                }
            }
        }

        private void dgvIP_SelectionChanged(object sender, EventArgs e)
        {
            var index = dgvIP.CurrentRow.Index;
            if (index >= 0 && dgvIP.Rows[index].Cells[0].Value != null)
            {
                this.selectFromIP = dgvIP.Rows[index].Cells[0].Value.ToString();
                this.selectToIP = dgvIP.Rows[index].Cells[1].Value.ToString();
                Task.Run(() =>
                {
                    var data = list
                        .Where(p => p.FromIP == this.selectFromIP && p.ToIP == selectToIP)
                        .GroupBy(p => new { p.FromPort, p.ToPort, p.ProcName })
                        .Select(p => p.Key)
                        .Select(p => new List<string>() { p.FromPort, p.ToPort, p.ProcName });
                    RefreshDgv(dgvPort, data, lockPortObj);
                });
            }
        }

        private void SetControlText(Control control, string text)
        {
            if (string.IsNullOrWhiteSpace(selectFromPort) || string.IsNullOrWhiteSpace(selectToPort))
            {
                return;
            }
            if (txtContent.InvokeRequired)
            {
                var foo = new DelegateControlString(this.SetControlText);
                this.Invoke(foo, control, text);
            }
            else
            {
                lock (lockContent)
                {
                    control.Text = string.Empty;
                    control.Text = text;
                }
            }
        }

        private void RefreshContent()
        {
            var packs =
                list.Where(p =>
                p.FromIP == this.selectFromIP &&
                p.ToIP == selectToIP &&
                p.FromPort == this.selectFromPort &&
                p.ToPort == this.selectToPort)
                .OrderBy(p => p.Time);

            var sb = new StringBuilder();
            foreach (var item in packs)
            {
                sb.AppendFormat("{0} : {1}\n", item.Time.ToString("HH:mm:ss"), UtilMethods.GetContent(item.Data, chkToBase64.Checked));
            }
        }
    }
}