namespace ATAS.Indicators.Technical
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Windows.Media;
    using OFT.Localization;
    using OFT.Attributes;

    [DisplayName("Thirds ColorBar")]
    [HelpLink("https://github.com/rgutmen")]
    public class ColorBarByThirds : Indicator
    {
        #region Fields

        private Color _colorTop = Colors.Aqua;
        private Color _colorMiddle = Colors.Orange;
        private Color _colorBottom = Colors.DarkMagenta;

        private PaintbarsDataSeries _renderSeries = new("RenderSeries", "PaintBars")
        {
            IsHidden = true
        };

        #endregion

        #region Properties
        [Display(Name = "Buy Candle", GroupName = nameof(Strings.Color), Description = nameof(Strings.ColorDescription), Order = 10)]
        public Color ColorTop
        {
            get => _colorTop;
            set
            {
                _colorTop = value;
                RecalculateValues();
            }
        }

        [Display(Name = "Doji Candle", GroupName = nameof(Strings.Color), Description = nameof(Strings.ColorDescription), Order = 20)]
        public Color ColorMiddle
        {
            get => _colorMiddle;
            set
            {
                _colorMiddle = value;
                RecalculateValues();
            }
        }
        
        [Display(Name = "Sell Candle", GroupName = nameof(Strings.Color), Description = nameof(Strings.ColorDescription), Order = 30)]
        public Color ColorBottom
        {
            get => _colorBottom;
            set
            {
                _colorBottom = value;
                RecalculateValues();
            }
        }

        #endregion

        #region ctor

        public ColorBarByThirds()
            : base(true)
        {
            DenyToChangePanel = true;
            DataSeries[0] = _renderSeries;
        }

        #endregion

        #region Protected methods

        protected override void OnRecalculate()
        {
            Clear();
        }

        protected override void OnCalculate(int bar, decimal value)
        {
            if (bar == 0)
                return;

            var candle = GetCandle(bar);

            decimal high = candle.High;
            decimal low = candle.Low;
            decimal range = high - low;
            
            decimal third = range / 3;
            
            decimal close = candle.Close;
            
            if (close >= (high - third))
                _renderSeries[bar] = ColorTop;
            else if (close <= (low + third))
                _renderSeries[bar] = ColorBottom;
            else
                _renderSeries[bar] = ColorMiddle;

        }
        #endregion
    }
}
