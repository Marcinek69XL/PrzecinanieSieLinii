using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms.Model
{
    public class Config
    {
        private int? _line1Width;

        public int Line1Width
        {
            get
            {
                if (_line1Width == null)
                {
                    return 1;
                }
                return _line1Width.Value;
            }
            set => _line1Width = value;
        }

        private int? _line2Width;
        public int Line2Width
        {
            get
            {
                if (_line2Width == null)
                {
                    return 1;
                }
                return _line2Width.Value;
            }
            set => _line2Width = value;
        }

        private int? _netWidth;
        public int NetWidth
        {
            get
            {
                if (_netWidth == null)
                {
                    return 1;
                }
                return _netWidth.Value;
            }
            set => _netWidth = value;
        }

        private int? _axisWidth;
        public int AxisWidth
        {
            get
            {
                if (_axisWidth == null)
                {
                    return 1;
                }
                return _axisWidth.Value;
            }
            set => _axisWidth = value;
        }

        private Color? _line1Color;
        public Color Line1Color
        {
            get
            {
                if (_line1Color == null)
                    return Color.Black;

                return _line1Color.Value;
            }
            set => _line1Color = value;
        }

        private Color? _line2Color;
        public Color Line2Color
        {
            get
            {
                if (_line2Color == null)
                    return Color.Black;

                return _line2Color.Value;
            }
            set => _line2Color = value;
        }

        private Color? _netColor;
        public Color NetColor
        {
            get
            {
                if (_netColor == null)
                    return Color.Black;

                return _netColor.Value;
            }
            set => _netColor = value;
        }

        private Color? _axisColor;
        public Color AxisColor
        {
            get
            {
                if (_axisColor == null)
                    return Color.Black;

                return _axisColor.Value;
            }
            set => _axisColor = value;
        }

        private bool? _line1IsVisibility;
        public bool Line1IsVisibility
        {
            get
            {
                if (_line1IsVisibility == null)
                    return true;

                return _line1IsVisibility.Value;
            }
            set => _line1IsVisibility = value;
        }

        private bool? _line2IsVisibility;
        public bool Line2IsVisibility
        {
            get
            {
                if (_line2IsVisibility == null)
                    return true;

                return _line2IsVisibility.Value;
            }
            set => _line2IsVisibility = value;
        }

        private bool? _netVisibility;
        public bool NetVisibility
        {
            get
            {
                if (_netVisibility == null)
                    return true;

                return _netVisibility.Value;
            }
            set => _netVisibility = value;
        }

        private bool? _axisIsVisibility;
        public bool AxisIsVisibility
        {
            get
            {
                if (_axisIsVisibility == null)
                    return true;

                return _axisIsVisibility.Value;
            }
            set => _axisIsVisibility = value;
        }

        private int? _pointSize;
        public int PointSize
        {
            get => _pointSize ?? 1;
            set => _pointSize = value;
        }

        private bool? _pointsVisibily;

        public bool PointsVisibily
        {
            get => _pointsVisibily ?? true;
            set => _pointsVisibily = value;
        }

        private bool? _saveConfig;
        public bool SaveConfig
        {
            get => _saveConfig ?? true;
            set => _saveConfig = value;
        }

        private bool? _saveValues;
        public bool SaveValues
        {
            get => _saveValues ?? true;
            set => _saveValues = value;
        }

        private bool? _gridlinesVisibily;
        public bool GridlinesVisibility
        {
            get => _gridlinesVisibily ?? true;
            set => _gridlinesVisibily = value;
        }
    }
}
