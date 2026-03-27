using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace TouchpadRecognizer
{
    public partial class TouchAreaLabel : Label
    {
        private static readonly Color DefaultColor = Color.LightGray;
        private static readonly Color SelectedColor = Color.LightBlue;

        #region カスタムプロパティ
        private bool _isSelected = false;
        [Category("カスタムプロパティ")]
        [Description("選択された状態か否かを示す論理値")]
        [DefaultValue(false)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        private bool _isCircle = false;
        [Category("カスタムプロパティ")]
        [Description("円形か否かを示す論理値")]
        [DefaultValue(false)]
        public bool IsCircle
        {
            get { return _isCircle; }
            set { _isCircle = value; }
        }
        #endregion

        public TouchAreaLabel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            this.BackColor = (_isSelected) ? SelectedColor : DefaultColor;
            if (_isCircle)
            {
                var gp = new GraphicsPath();
                gp.AddEllipse(0, 0, this.Width, this.Height);
                this.Region = new Region(gp);

                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.Black, 2);
                pe.Graphics.DrawEllipse(pen, 2, 2, this.Width - 4, this.Height - 4); // 2pxの枠線×2本分のスペースを上下左右に確保する。
            }
            base.OnPaint(pe);
        }
    }
}
