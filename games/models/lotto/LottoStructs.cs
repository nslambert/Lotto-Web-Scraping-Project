using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTabControl;
using System.Drawing;

namespace ttpim.gamemodule.lotto.models
{
    public struct LottoResults
    {

    }

    public struct LottoPlusResults
    {

    }

    public struct PowerballResults
    {

    }

    public struct DownloadData
    {
        private object sender;
        private WebBrowserDocumentCompletedEventArgs args;

        public object Sender
        {
            get
            {
                return sender;
            }
            set
            {
                sender = value;
            }
        }

        public WebBrowserDocumentCompletedEventArgs Args
        {
            get
            {
                return args;
            }
            set
            {
                args = value;
            }
        }
    }

    public struct PanelConfiguration
    {
        private DockStyle _dockstyle;
        private Color _color;
        private Point _point;
        private int _numcolumns;
        private int _numrows;
        private string _name;
        private ColumnStyle[] _columnstyles;
        private RowStyle[] _rowstyles;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public DockStyle DockStyle
        {
            get
            {
                return _dockstyle;
            }
            set
            {
                _dockstyle = value;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public Point Location
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
            }
        }


        public int NumColumns
        {
            get 
            {
                return _numcolumns;
            }
            set 
            {
                _numcolumns = value;
            }
        }

        public int NumRows
        {
            get
            {
                return _numrows;
            }
            set
            {
                _numrows = value;
            }
        }

        public ColumnStyle[] ColumnStyles
        {
            get
            {
                if (_columnstyles == null)
                    _columnstyles = new ColumnStyle[_numcolumns];
                return _columnstyles;
            }
            set
            {
                _columnstyles = value;
            }
        }

        public RowStyle[] RowStyles
        {
            get
            {
                if (_rowstyles == null)
                    _rowstyles = new RowStyle[_numrows];
                return _rowstyles;
            }
            set
            {
                _rowstyles = value;
            }
        }
    }
}
