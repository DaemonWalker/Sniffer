using SharpPcap;
using Sniffer.UI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sniffer.UI
{
    public partial class Form1 : Form
    {
        private ICaptureDevice selectedDevice;
        private List<PackageModel> list = new List<PackageModel>();
        private BinaryFormatter binaryFormatter = new BinaryFormatter();
        private string selectFromIP;
        private string selectToIP;
        private string selectFromPort;
        private string selectToPort;
        private Encoding encoding = Encoding.UTF8;
        private string localIP;
        public Form1()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void Form1_Load(object sender, EventArgs e)
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

        private List<PackageModel> DeepCopyList()
        {
            var ms = new MemoryStream();
            binaryFormatter.Serialize(ms, this.list);
            ms.Position = 0;
            return binaryFormatter.Deserialize(ms) as List<PackageModel>;
        }
        private delegate void DelegateSetDataSource(DataGridView dgv, PackageModel pm, IList<string> props, object lockObj);
        private static object lockIPObj = new object();
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
        private delegate void DelegateSetDgvPortData();
        private static object lockPortObj = new object();
        private void RefreshDgvPort()
        {
            if (string.IsNullOrWhiteSpace(selectFromIP) || string.IsNullOrWhiteSpace(selectToIP))
            {
                return;
            }
            if (dgvPort.InvokeRequired)
            {
                var foo = new DelegateSetDgvPortData(this.RefreshDgvPort);
                this.Invoke(foo);
            }
            else
            {
                lock (lockPortObj)
                {
                    dgvPort.Rows.Clear();
                    var data = list.Where(p => p.FromIP == this.selectFromIP && p.ToIP == selectToIP).GroupBy(p => new { p.FromPort, p.ToPort, p.ProcName }).Select(p => p.Key);
                    foreach (var item in data)
                    {
                        dgvPort.Rows.Add();
                        dgvPort.Rows[dgvPort.Rows.Count - 1].Cells[0].Value = item.FromPort;
                        dgvPort.Rows[dgvPort.Rows.Count - 1].Cells[1].Value = item.ToPort;
                        dgvPort.Rows[dgvPort.Rows.Count - 1].Cells[2].Value = item.ProcName;
                    }
                }
            }
        }
        private static object lockContent = new object();
        private void RefreshContent()
        {
            if (string.IsNullOrWhiteSpace(selectFromPort) || string.IsNullOrWhiteSpace(selectToPort))
            {
                return;
            }
            if (txtContent.InvokeRequired)
            {
                var foo = new DelegateSetDgvPortData(this.RefreshContent);
                this.Invoke(foo);
            }
            else
            {
                lock (lockContent)
                {
                    txtContent.Text = string.Empty;
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
                    txtContent.Text = sb.ToString();
                }
            }
        }
        private delegate void DelegateNoArgs(PackageModel pm);
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
        private void dgvIP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                this.selectFromIP = dgvIP.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.selectToIP = dgvIP.Rows[e.RowIndex].Cells[1].Value.ToString();
                RefreshDgvPort();
                AutoChangeContent();
            }
        }

        private void dgvPort_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            this.selectFromPort = dgvPort.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.selectToPort = dgvPort.Rows[e.RowIndex].Cells[1].Value.ToString();
            RefreshContent();
        }

        private void drpEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.encoding = Encoding.GetEncoding(drpEncoding.Text);
            RefreshContent();
        }

        private void dgvIP_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIP.CurrentRow == null)
            {
                return;
            }
            var index = dgvIP.CurrentRow.Index;
            if (index >= 0 && dgvIP.Rows[index].Cells[0].Value != null)
            {
                this.selectFromIP = dgvIP.Rows[index].Cells[0].Value.ToString();
                this.selectToIP = dgvIP.Rows[index].Cells[1].Value.ToString();
                RefreshDgvPort();
            }
        }

        private void AutoChangeContent()
        {
            if (dgvPort.SelectedRows.Count == 0)
            {
                return;
            }
            this.selectFromPort = dgvPort.SelectedRows[0].Cells[0].Value.ToString();
            this.selectToPort = dgvPort.SelectedRows[0].Cells[1].Value.ToString();
            var index = dgvPort.CurrentRow.Index;
            if (index >= 0 && dgvPort.Rows[index].Cells[0].Value != null)
            {
                this.selectFromPort = dgvPort.Rows[index].Cells[0].Value.ToString();
                this.selectToPort = dgvPort.Rows[index].Cells[1].Value.ToString();
                RefreshContent();
            }
        }


        private void chkDepress_CheckedChanged(object sender, EventArgs e)
        {
            RefreshContent();
        }



    }
}
