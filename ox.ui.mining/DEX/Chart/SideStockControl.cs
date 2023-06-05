using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Ledger;
using OX.Mining.DEX;
using OX.UI.Mining.DEX.Chart;

namespace OX.UI.Swap
{
    public partial class SideStockControl : ContainerControl
    {
        INotecase Operater;
        bool IsHour = false;
        public SideStockControl(INotecase notecase, SideSwapPairKeyMerge swapPairMerge, AssetState assetState)
        {
            this.Operater = notecase;
            this.SwapPairMerge = swapPairMerge;
            this.AssetState = assetState;
            InitializeComponent();
            //加载事件
            this.HandleCreated += StockControl_HandleCreated;
            this.SizeChanged += StockControl_SizeChanged;
            this.Paint += StockControl_Paint;
            this.PreviewKeyDown += StockControl_PreviewKeyDown;
            this.KeyDown += StockControl_KeyDown;
            this.KeyUp += StockControl_KeyUp;
            this.MouseMove += StockControl_MouseMove;
            this.MouseUp += StockControl_MouseUp;
            this.MouseDown += StockControl_MouseDown;
            this.MouseWheel += StockControl_MouseWheel;
        }
        #region fields
        SideSwapPairKeyMerge SwapPairMerge;
        AssetState AssetState;
        List<DayLineData> DailyData = new List<DayLineData>();
        List<DayLineData> displayData = new List<DayLineData>();
        private DateTime lastMouseMoveTime = DateTime.Now;
        private object refresh_lock = new object();
        [Browsable(true)]
        public bool ShowLeftScale { get; set; } = true;
        [Browsable(true)]
        public bool ShowRightScale { get; set; } = true;

        [Browsable(true)]
        public int RightPixSpace { get; set; }
        [Browsable(true)]
        public int RightOrderSpace { get; set; }
        [Browsable(true)]
        public int AxisSpace { get; set; } = 8;
        [Browsable(true)]
        public int AreaSplit { get; set; } = 60;

        [Browsable(false)]
        public bool ShowCrossHair { get; set; } = false;


        public int FirstRecord { get; set; }
        public Color GridColor { get; set; } = Color.Gray;
        public Color YanColor { get; set; } = Color.Red;
        public Color YinColor { get; set; } = Color.Green;
        public Color CrossHairColor { get; set; } = Color.White;
        public Pen GridPen { get; set; } = new Pen(Color.Gray);
        public Pen YanPen { get; set; } = new Pen(Color.Red);
        public Pen YinPen { get; set; } = new Pen(Color.Green);
        public Pen CrossHair_Pen { get; set; } = new Pen(Color.White);
        public Brush YanBrush = new SolidBrush(Color.Red);
        public Brush YinBrush = new SolidBrush(Color.Green);
        public Brush GridBrush = new SolidBrush(Color.Gray);
        public Brush Xtip_Brush { get; set; } = new SolidBrush(Color.FromArgb(100, Color.Red));
        private Pen XTipFont_Pen { get; set; } = new Pen(Color.White);

        public Font StockFont { get; set; } = new Font("New Times Roman", 9);
        Dictionary<int, DayLineData> CrosshairXs = new Dictionary<int, DayLineData>();
        KeyValuePair<SwapPairExchangeKey, SideSwapVolumeMerge> LastPair;
        IEnumerable<KeyValuePair<SwapPairExchangeKey, SideSwapVolumeMerge>> LoadData;
        ~SideStockControl()
        {
            if (this.GridPen != default) this.GridPen.Dispose();
            if (this.YanPen != default) this.YanPen.Dispose();
            if (this.YinPen != default) this.YinPen.Dispose();
            if (this.CrossHair_Pen != default) this.CrossHair_Pen.Dispose();
            if (this.YanBrush != default) this.YanBrush.Dispose();
            if (this.YinBrush != default) this.YinBrush.Dispose();
            if (this.StockFont != default) this.StockFont.Dispose();
            if (this.GridBrush != default) this.GridBrush.Dispose();
            if (this.Xtip_Brush != default) this.Xtip_Brush.Dispose();
            if (this.XTipFont_Pen != default) this.XTipFont_Pen.Dispose();
        }
        #endregion
        #region events
        private void StockControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn(2);
            }
            else
            {
                ZoomOut(2);
            }
            this.RefreshGraph(false);
            this.Focus();
        }

        private void StockControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 1)
                {

                }
                else if (e.Clicks == 2)
                {
                    //双击显示或隐藏十字线
                    this.ShowCrossHair = !this.ShowCrossHair;
                    RefreshGraph(false);
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                DarkContextMenu menu = new DarkContextMenu();
                var sm = new ToolStripMenuItem(UIHelper.LocalString("我的交易记录", "My exchange records"));
                sm.Click += Sm_Click;
                menu.Items.Add(sm);
                var mc = IsHour ? "日K线" : "时K线";
                var me = IsHour ? "Daily K Line" : "Hour K Line";
                sm = new ToolStripMenuItem(UIHelper.LocalString($"转为{mc}", $"Change to {me}"));
                sm.Click += Sm_Click1;
                menu.Items.Add(sm);
                if (menu.Items.Count > 0)
                    menu.Show(this, e.Location);
            }
        }

        private void Sm_Click1(object sender, EventArgs e)
        {
            IsHour = !IsHour;
            ReloadInitData();
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            new MySideSwapRecords(this.Operater, this.AssetState, this.SwapPairMerge, this.LoadData).ShowDialog();
        }

        private void StockControl_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void StockControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastMouseMoveTime.AddTicks(100000) < DateTime.Now)
            {
                RefreshGraph(false);
            }
            lastMouseMoveTime = DateTime.Now;
        }

        private void StockControl_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void StockControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void StockControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyData == Keys.Left)//正常左移动为1步长
            {
                if (this.FirstRecord > 0)
                    this.FirstRecord--;
            }
            if (e.KeyData == Keys.Right)//正常右移动为1步长
            {
                var w = this.DisplayRectangle.Width - this.RightPixSpace - this.RightOrderSpace;
                var count = w / this.AxisSpace;
                if (count <= this.displayData.Count)
                {
                    if (this.FirstRecord < this.DailyData.Count - 1)
                        this.FirstRecord++;
                }
            }

            if (e.Control && e.KeyCode == Keys.Left)//组合Ctrl左移动为50步长
            {
                if (this.FirstRecord >= 50)
                    this.FirstRecord -= 50;
                else
                    this.FirstRecord = 0;
            }

            if (e.Control && e.KeyCode == Keys.Right)//组合Ctrl右移动为50步长
            {
                var w = this.DisplayRectangle.Width - this.RightPixSpace - this.RightOrderSpace;
                var count = w / this.AxisSpace;
                if (count <= this.displayData.Count)
                {
                    if (this.FirstRecord + 50 < this.DailyData.Count - 1)
                        this.FirstRecord += 50;
                    else
                        this.FirstRecord = this.DailyData.Count - 1;
                }
            }
            if (e.KeyData == Keys.Up)
            {
                this.ZoomIn(1);
            }
            if (e.KeyData == Keys.Down)
            {
                this.ZoomOut(1);
            }
            RefreshGraph(false);
            this.Focus();
        }

        private void StockControl_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph();
        }

        private void StockControl_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width != 0 && this.Size.Height != 0)
            {
                RefreshGraph(false);
            }
        }

        private void StockControl_HandleCreated(object sender, EventArgs e)
        {

        }
        #endregion
        #region methods

        /// 重置为空的图像
        /// </summary>
        public void ResetNullGraph()
        {
            this.AxisSpace = 8;
            this.RightPixSpace = 0;
            this.ShowLeftScale = false;
            this.ShowRightScale = false;
        }
        public void Init(IEnumerable<KeyValuePair<SwapPairExchangeKey, SideSwapVolumeMerge>> loaddata)
        {
            this.LoadData = loaddata;
            ReloadInitData();
        }
        void ReloadInitData()
        {
            List<DayLineData> datas = new List<DayLineData>();
            var last = this.LoadData.OrderByDescending(m => m.Key.Timestamp).FirstOrDefault();
            foreach (var g in this.LoadData.GroupBy(m => IsHour ? m.Key.Timestamp.ToHourLong() : m.Key.Timestamp.ToDayLong()).OrderBy(m => m.Key))
            {
                var ePrice = g.OrderByDescending(m => m.Key.Timestamp).FirstOrDefault().Value.Price;
                var bPrice = g.OrderBy(m => m.Key.Timestamp).FirstOrDefault().Value.Price;
                var hPrice = g.OrderByDescending(m => m.Value.Price).FirstOrDefault().Value.Price;
                var lPrice = g.OrderBy(m => m.Value.Price).FirstOrDefault().Value.Price;
                datas.Add(new DayLineData
                {
                    TimeUnit = g.Key,
                    BPrice = bPrice,
                    HPrice = hPrice,
                    LPrice = lPrice,
                    EPrice = ePrice,
                    TargetTotal = g.Sum(m => m.Value.Volume.TargetAssetVolume.GetInternalValue()),
                    PriceTotal = g.Sum(m => m.Value.Volume.PricingAssetVolume.GetInternalValue()),
                });
            }
            this.DailyData = datas.OrderBy(m => m.TimeUnit).ToList();
            this.LastPair = last;
            RefreshGraph(true);
        }



        public void RefreshGraph(bool InitFirst)
        {
            ResetFirstRecord(InitFirst);
            DrawGraph();
        }

        public void ResetFirstRecord(bool InitFirst)
        {
            var dc = DailyData.Count;
            if (dc == 0) return;
            if (dc < this.FirstRecord)
            {
                this.FirstRecord = dc - 1;
            };
            var w = this.DisplayRectangle.Width - this.RightPixSpace - this.RightOrderSpace;
            if (w <= 0) return;
            var count = w / this.AxisSpace;

            if (InitFirst)
            {
                if (count > dc)
                {
                    this.FirstRecord = 0;
                    displayData = DailyData.Take(new Range(new Index(this.FirstRecord), new Index(dc))).OrderBy(m => m.TimeUnit).ToList();
                }
                else
                {
                    this.FirstRecord = dc - count;
                    displayData = DailyData.Take(new Range(new Index(this.FirstRecord), new Index(dc))).OrderBy(m => m.TimeUnit).ToList();
                }
            }
            else
            {
                var c = count + this.FirstRecord;
                if (c > dc) c = dc;
                displayData = DailyData.Take(new Range(new Index(this.FirstRecord), new Index(c))).OrderBy(m => m.TimeUnit).ToList();
            }
        }

        public void DrawGraph()
        {
            PaintGraph(this.DisplayRectangle);
        }

        public void PaintGraph(Rectangle drawRectangle)
        {
            lock (refresh_lock)
            {
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics myBuffer = currentContext.Allocate(this.CreateGraphics(), drawRectangle);
                Graphics g = myBuffer.Graphics;
                DrawBackGround(g);
                DrawScale(g, out int klbottom, out decimal? mp, out decimal? lp);
                DrawRecords(g);
                if (klbottom != 0 && mp != 0)
                    DrawCrossHair(g, klbottom, mp, lp);
                myBuffer.Render();
                myBuffer.Dispose();
            }
        }
        public void DrawCrossHair(Graphics g, int klbottom, decimal? mp, decimal? lp)
        {
            if (this.ShowCrossHair && GetCrossHairPoint(out Point mousePoint, out DayLineData data))
            {
                if (mousePoint.Y > this.AreaSplit && mousePoint.Y < klbottom)
                    g.DrawLine(this.CrossHair_Pen, 0, mousePoint.Y, this.DisplayRectangle.Width - this.RightOrderSpace, mousePoint.Y);
                g.DrawLine(this.CrossHair_Pen, mousePoint.X, this.AreaSplit, mousePoint.X, this.DisplayRectangle.Height);
                var k = (klbottom - mousePoint.Y) / mp + lp;
                string s = $"{data.TimeUnit.ToTimeString(IsHour)}   {k.Value.ToString("f6")}      {new Fixed8(data.PriceTotal)}  /  {new Fixed8(data.TargetTotal)}    OPEN:{data.BPrice.ToString("f6")}  /  CLOSE:{data.EPrice.ToString("f6")}  /  HIGH:{data.HPrice.ToString("f6")}  /  LOW:{data.LPrice.ToString("f6")} ";
                Brush bBrush = new SolidBrush(this.CrossHairColor);
                g.DrawString(s, this.StockFont, bBrush, new PointF(10, this.AreaSplit + 10));
            }
        }
        public bool GetCrossHairPoint(out Point p, out DayLineData data)
        {
            p = default;
            data = default;
            int x = this.PointToClient(MousePosition).X;
            int y = this.PointToClient(MousePosition).Y;
            foreach (var cx in this.CrosshairXs)
            {
                if (x >= cx.Key && x <= cx.Key + this.AxisSpace)
                {
                    x = cx.Key + this.AxisSpace / 2;
                    p = new Point(x, y);
                    data = cx.Value;
                    return true;
                }
            }
            return false;
        }

        private void DrawBackGround(Graphics g)
        {
            //foreach (ChartPanel chartPanel in this.panels.Values)
            //{
            //    Rectangle drawRect = new Rectangle(0, chartPanel.RectPanel.Y, this.Width - 2, chartPanel.RectPanel.Height);
            //    g.FillRectangle(chartPanel.BgBrush, drawRect);
            //    g.DrawRectangle(chartPanel.PanelBorder_Pen, drawRect);
            //}
        }

        public void DrawRecords(Graphics g)
        {
            if (this.LoadData.IsNotNullAndEmpty())
            {
                StringFormat sf = new StringFormat();
                Brush yanBrush = new SolidBrush(this.YanColor);
                Brush yinBrush = new SolidBrush(this.YinColor);
                int y = this.AreaSplit;
                foreach (var p in this.LoadData.OrderByDescending(m => (long)m.Value.Volume.BlockIndex * 1000 + (long)m.Value.Volume.TxN))
                {
                    var bBrush = p.Value.Volume.IsBuy ? yanBrush : yinBrush;
                    string assetName = string.Empty;
                    bool ok = false;
                    Fixed8 amount = Fixed8.Zero;
                    if (p.Value.Volume.TargetAssetVolume > Fixed8.Zero)
                    {
                        amount = p.Value.Volume.TargetAssetVolume;
                        assetName = this.AssetState.GetName();
                        ok = true;
                    }
                    else if (p.Value.Volume.PricingAssetVolume > Fixed8.Zero)
                    {
                        amount = p.Value.Volume.PricingAssetVolume;
                        assetName = "OXC";
                        ok = true;
                    }
                    if (ok)
                    {
                        string s = $"{p.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm")}  {p.Value.Price.ToString("f6")} / {amount}  {assetName}";
                        SizeF bsFontSize = g.MeasureString(s, this.StockFont, 1000, sf);
                        g.DrawString(s, this.StockFont, bBrush, new PointF(this.DisplayRectangle.Width - bsFontSize.Width, y));
                        y = y + (int)bsFontSize.Height + 10;
                        if (y > this.DisplayRectangle.Height - this.AreaSplit)
                            break;
                    }
                }
                yanBrush.Dispose();
                yinBrush.Dispose();
            }
        }


        public void DrawScale(Graphics g, out int klbottom, out decimal? mp, out decimal? lp)
        {
            klbottom = 0;
            mp = 0;
            lp = 0;
            StringFormat sf = new StringFormat();
            var H = this.DisplayRectangle.Height - AreaSplit * 3;
            var H1 = (int)(0.5 * H);
            var H2 = (int)(0.25 * H);
            var H3 = (int)(0.25 * H);
            var hp = this.displayData.OrderByDescending(m => m.HPrice).FirstOrDefault()?.HPrice;
            lp = this.displayData.OrderBy(m => m.LPrice).FirstOrDefault()?.LPrice;
            var sp = hp - lp;
            if (sp == 0) return;
            mp = H1 / sp;
            var pt = this.displayData.OrderByDescending(m => m.PriceTotal).FirstOrDefault()?.PriceTotal;
            var tt = this.displayData.OrderByDescending(m => m.TargetTotal).FirstOrDefault()?.TargetTotal;
            var mpt = pt.HasValue ? H2 / (decimal)(pt.Value) : 0;
            var mtt = tt.HasValue ? H3 / (decimal)(tt.Value) : 0;

            klbottom = this.DisplayRectangle.Y + this.AreaSplit + H1;
            var gridHeight = H1 / 5;
            Brush bBrush = new SolidBrush(this.GridColor);
            for (int i = 0; i <= 5; i++)
            {
                var y = this.DisplayRectangle.Y + this.AreaSplit + gridHeight * i;
                g.DrawLine(this.GridPen, 0, y, this.DisplayRectangle.Width - this.RightOrderSpace, y);
                var gmp = gridHeight * (5 - i) / mp + lp;
                var sgmp = string.Empty;
                if (gmp.HasValue)
                    sgmp = gmp.Value.ToString("f6");
                SizeF bsFontSize = g.MeasureString(sgmp, this.StockFont, 1000, sf);
                g.DrawString(sgmp, this.StockFont, bBrush, new PointF(this.DisplayRectangle.Width - this.RightOrderSpace - bsFontSize.Width,
                y - 20));
            }
            var gridWidth = (this.DisplayRectangle.Width - this.RightOrderSpace) / 5;
            List<int> xl = new List<int>();
            for (int i = 0; i <= 5; i++)
            {
                var x = this.DisplayRectangle.X + gridWidth * i;
                g.DrawLine(this.GridPen, x, this.DisplayRectangle.Y + this.AreaSplit, x, klbottom - 5);
                if (i > 0 && i < 5) xl.Add(x);
            }
            int top = this.DisplayRectangle.Y + AreaSplit * 2 + H1;
            int top2 = this.DisplayRectangle.Y + AreaSplit * 3 + H1 + H2;
            int panelBottm2 = this.DisplayRectangle.Y + H1 + AreaSplit * 2 + H2;
            int panelBottm3 = this.DisplayRectangle.Y + H1 + AreaSplit * 3 + H2 + H3;
            g.DrawLine(this.GridPen, 0, top, this.DisplayRectangle.Width - this.RightOrderSpace, top);
            g.DrawLine(this.GridPen, 0, panelBottm2, this.DisplayRectangle.Width - this.RightOrderSpace, panelBottm2);
            g.DrawLine(this.GridPen, 0, top2, this.DisplayRectangle.Width - this.RightOrderSpace, top2);
            g.DrawLine(this.GridPen, 0, panelBottm3, this.DisplayRectangle.Width - this.RightOrderSpace, panelBottm3);

            for (int i = 0; i <= 5; i++)
            {
                var x = this.DisplayRectangle.X + gridWidth * i;
                g.DrawLine(this.GridPen, x, this.DisplayRectangle.Y + H1 + AreaSplit * 2, x, panelBottm2 - 5);
                g.DrawLine(this.GridPen, x, this.DisplayRectangle.Y + H1 + H2 + AreaSplit * 3, x, panelBottm3 - 5);
            }

            int k = 0;
            this.CrosshairXs.Clear();
            foreach (var d in this.displayData)
            {
                DrawKLine(g, d, k, this.AreaSplit + H1, mp, lp, xl);
                DrawPriceVolume(g, d, k, this.AreaSplit * 2 + H1 + H2, mpt, d.PriceTotal, xl);
                DrawTargetVolume(g, d, k, this.AreaSplit * 3 + H1 + H2 + H3, mtt, d.TargetTotal);
                k++;
            }
            var pricingAssetName = "OXC";
            var targetAssetName = this.AssetState.GetName();
            g.DrawString(pricingAssetName, this.StockFont, bBrush, new PointF(20, top - 20));
            g.DrawString(targetAssetName, this.StockFont, bBrush, new PointF(20, top2 - 20));
            if (!this.LastPair.Equals(new KeyValuePair<SwapPairPriceKey, SwapPairRechargeMerge>()))
            {
                if (this.LastPair.Value.IsNotNull() && this.LastPair.Key.IsNotNull())
                {
                    //string s = $"{this.LastPair.Value.Price}   /   {pricingAssetName}:{this.LastPair.Value.PriceBalance.ToString()}   /   {targetAssetName}:{this.LastPair.Value.TargetBalance.ToString()}   /   {this.LastPair.Key.SH.ToAddress()}";
                    string s = $"{this.LastPair.Value.Price.ToString("f6")}   /   {this.LastPair.Key.SH.ToAddress()}";
                    g.DrawString(s, this.StockFont, bBrush, new PointF(20, 20));
                }
            }
            bBrush.Dispose();
        }

        void DrawKLine(Graphics g, DayLineData data, int index, int H1, decimal? mp, decimal? lp, List<int> xl)
        {
            StringFormat sf = new StringFormat();
            var x = this.DisplayRectangle.X + this.AxisSpace * index + this.AxisSpace / 2;
            this.CrosshairXs[this.DisplayRectangle.X + this.AxisSpace * index] = data;
            var bottom = this.DisplayRectangle.Y + H1;
            var ly = bottom - (data.LPrice - lp.Value) * mp;
            var hy = bottom - (data.HPrice - lp.Value) * mp;
            var by = bottom - (data.BPrice - lp.Value) * mp;
            var ey = bottom - (data.EPrice - lp.Value) * mp;
            Pen pen = this.YanPen;
            Brush brush = this.YanBrush;
            if (data.BPrice > data.EPrice)
            {
                pen = this.YinPen;
                brush = this.YinBrush;
            }
            var y = by;
            decimal? sy = 0;
            sy = ey - by;
            if (ey < by)
            {
                sy = by - ey;
                y = ey;
            }

            g.DrawLine(pen, x, (int)(ly.Value), x, (int)(hy.Value));
            var x1 = x - this.AxisSpace / 2 + 2;
            if (sy.Value > 0)
            {
                var rect = new Rectangle(x1, (int)(y.Value), this.AxisSpace - 4, (int)(sy.Value));
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect);
            }
            else
            {
                g.DrawLine(pen, x1, (int)(ly.Value), x1 + this.AxisSpace - 4, (int)(ly.Value));
            }
            foreach (var x2 in xl)
            {
                if (x2 > x1 - 2 && x2 < x1 + this.AxisSpace)
                {
                    var str = data.TimeUnit.ToTimeString(IsHour);
                    SizeF bsFontSize = g.MeasureString(str, this.StockFont, 1000, sf);
                    Brush bBrush = new SolidBrush(this.GridColor);
                    g.DrawString(str, this.StockFont, bBrush, new PointF(x2 - 40,
                   bottom + 2));
                    bBrush.Dispose();
                }
            }
        }
        void DrawPriceVolume(Graphics g, DayLineData data, int index, int H2, decimal? mpt, long PriceTotal, List<int> xl)
        {
            StringFormat sf = new StringFormat();
            var x = this.DisplayRectangle.X + this.AxisSpace * index + this.AxisSpace / 2;
            var bottom = this.DisplayRectangle.Y + H2;
            var p = PriceTotal * mpt;
            Pen pen = this.YanPen;
            Brush brush = this.YanBrush;
            if (data.BPrice > data.EPrice)
            {
                pen = this.YinPen;
                brush = this.YinBrush;
            }
            var b2 = (int)(p.Value);
            var x1 = x - this.AxisSpace / 2 + 2;
            var rect = new Rectangle(x1, bottom - b2, this.AxisSpace - 4, b2);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            foreach (var x2 in xl)
            {
                if (x2 > x1 - 2 && x2 < x1 + this.AxisSpace)
                {
                    var str = data.TimeUnit.ToTimeString(IsHour);
                    SizeF bsFontSize = g.MeasureString(str, this.StockFont, 1000, sf);
                    Brush bBrush = new SolidBrush(this.GridColor);
                    g.DrawString(str, this.StockFont, bBrush, new PointF(x2 - 40,
                   bottom + 2));
                    bBrush.Dispose();
                }
            }
        }
        void DrawTargetVolume(Graphics g, DayLineData data, int index, int H3, decimal? mpt, long TargetTotal)
        {

            var x = this.DisplayRectangle.X + this.AxisSpace * index + this.AxisSpace / 2;
            var bottom = this.DisplayRectangle.Y + H3;
            var p = TargetTotal * mpt;
            Pen pen = this.YanPen;
            Brush brush = this.YanBrush;
            if (data.BPrice > data.EPrice)
            {
                pen = this.YinPen;
                brush = this.YinBrush;
            }
            var b2 = (int)(p.Value);
            var x1 = x - this.AxisSpace / 2 + 2;
            var rect = new Rectangle(x1, bottom - b2, this.AxisSpace - 4, b2);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);

        }
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="step"></param>
        private void ZoomIn(int step)
        {
            if (this.AxisSpace < 50)
            {
                this.AxisSpace += step;
                this.ResetFirstRecord(false);
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="step"></param>
        private void ZoomOut(int step)
        {
            if (this.AxisSpace > 5)
            {
                this.AxisSpace -= step;
                this.ResetFirstRecord(false);
            }
        }


        #endregion
    }
}
