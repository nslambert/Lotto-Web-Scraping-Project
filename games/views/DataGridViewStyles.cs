using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentFactory.Krypton.Toolkit;
using System.Data;
using FormGridViewStyle = System.Windows.Forms.DataGridViewCellStyle;

namespace ttpim.gamemodule.games.views
{
    public class DataGridViewStyles
    {
        private DataTable _dataTable;
        private DataGridViewStyles _defaultColumnHeaderStyle;
        private DataGridViewStyles _defaultRowHeaderStyle;
        private DataGridViewStyles _defaultColumnStyle;
        private System.Windows.Forms.DataGridViewCellStyle _defaultRowStyle;
        private DataGridViewStyles _defaultCellStyle;
        private DataGridViewStyles _columnStyle;
        private DataGridViewStyles _rowStyle;
        private List<DataGridViewStyles> _columnHeaderStyles;
        private List<DataGridViewStyles> _rowHeaderStyles;
        private List<Dictionary<string, DataGridViewStyles>> _cellStyles;
        private List<System.Windows.Forms.DataGridViewCellStyle> _rowStyles;
        private Dictionary<string, FormGridViewStyle> _viewStyles;

        private int _rowCount;
        private int _columnCount;

        public DataGridViewStyles()
        {
            _columnHeaderStyles = new List<DataGridViewStyles>();
            _columnHeaderStyles.Capacity = _columnCount;
            _rowHeaderStyles = new List<DataGridViewStyles>();
            _rowHeaderStyles.Capacity = _rowCount;
            _defaultRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            _cellStyles = new List<Dictionary<string, DataGridViewStyles>>();
            _rowStyles = new List<System.Windows.Forms.DataGridViewCellStyle>();
            _viewStyles = new Dictionary<string, FormGridViewStyle>();
        }

        public DataTable GridDataTable
        {
            get
            {
                return _dataTable;
            }
            set
            {
                _dataTable = value;
            }
        }

        public int GridRowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                _rowCount = value;
            }
        }

        public int GridColumnCount
        {
            get
            {
                return _columnCount;
            }
            set
            {
                _columnCount = value;
            }
        }

        public DataGridViewStyles DefaultColumnHeaderStyle
        {
            get
            {
                return _defaultColumnHeaderStyle; 
            }
            set
            {
                _defaultColumnHeaderStyle = value;
            }
        }

        public DataGridViewStyles DefaultRowHeaderStyle
        {
            get
            {
                return _defaultRowHeaderStyle;
            }
            set
            {
                _defaultRowHeaderStyle = value;
            }
        }

        public System.Windows.Forms.DataGridViewCellStyle DefaultRowStyle
        {
            get
            {
                return _defaultRowStyle;
            }
            set
            {
                _defaultRowStyle = value;
            }
        }

        public DataGridViewStyles DefaultColumnStyle
        {
            get
            {
                return _defaultColumnStyle;
            }
            set
            {
                _defaultColumnStyle = value;
            }
        }

        public DataGridViewStyles DefaultCellStyle
        {
            get
            {
                return _defaultCellStyle;
            }
            set
            {
                _defaultCellStyle = value;
            }
        }

        public List<DataGridViewStyles> ColumnHeaderStyles
        {
            get
            {
                return _columnHeaderStyles;
            }
            set
            {
                _columnHeaderStyles = value;
            }
        }

        public List<DataGridViewStyles> RowHeaderStyles
        {
            get
            {
                return _rowHeaderStyles;
            }
            set
            {
                _rowHeaderStyles = value;
            }
        }

        public List<Dictionary<string, DataGridViewStyles>> CellStyles
        {
            get
            {
                return _cellStyles;
            }
            set
            {
                _cellStyles = value;
            }
        }

        public List<FormGridViewStyle> RowStyles
        {
            get
            {
                return _rowStyles;
            }
            set
            {
                _rowStyles = value;
            }
        }

        public Dictionary<string, FormGridViewStyle> ViewStyles
        {
            get
            {
                return _viewStyles;
            }
            set
            {
                _viewStyles = value;
            }
        }

        public DataTable GridViewDataTable
        {
            get
            {
                return _dataTable;
            }
            set
            {
                _dataTable = value;
            }
        }

        public void AddViewRowStyle(string styleName, FormGridViewStyle viewStyle)
        {
            ViewStyles.Add(styleName, viewStyle);
        }

        public void ProcessDataTable()
        {
            foreach (DataRow row in GridViewDataTable.Rows)
            {
                FormGridViewStyle rowStyle;
                Dictionary<string, DataGridViewStyles> cells = new Dictionary<string, DataGridViewStyles>();
                if (row["SubscriptionMarker"] != DBNull.Value)
                {
                    switch (Convert.ToInt32(row["SubscriptionMarker"]))
                    {
                        case 1:
                            rowStyle = ViewStyles["SubscriptionStyle"];
                            RowStyles.Add(rowStyle);
                            break;
                        case 2:
                            rowStyle = ViewStyles["ResultStyle"];
                            RowStyles.Add(rowStyle);
                            break;
                        default:
                            rowStyle = ViewStyles["DefaultStyle"];
                            RowStyles.Add(rowStyle);
                            break;
                    }
                }

                if ((row["Board"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 1"]))))
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(row["Board"])))
                    {
                        //cells["Board"] = ViewStyles["Board"];
                    }
                }

                if ((row["Ball 1"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 1"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 1"]) == Convert.ToString(row["BonusMarker"]))
                        {
                            //cells["Ball 1"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 1"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Ball 2"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 2"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 2"]) == Convert.ToString(row["BonusMarker"]))
                        {
                            //cells["Ball 2"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 2"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Ball 3"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 3"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 3"]) == Convert.ToString(row["BonusMarker"]))
                        {
                            //cells["Ball 3"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 3"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Ball 4"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 4"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 4"]) == Convert.ToString(row["BonusMarker"]))
                        {
                           // cells["Ball 4"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 4"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Ball 5"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 5"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 5"]) == Convert.ToString(row["BonusMarker"]))
                        {
                            //cells["Ball 5"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 5"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Ball 6"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Ball 6"]))))
                {
                    if ((row["BonusMarker"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["BonusMarker"]))))
                    {
                        if (Convert.ToString(row["Ball 6"]) == Convert.ToString(row["BonusMarker"]))
                        {
                            //cells["Ball 6"] = ViewStyles["BallMatch+Bonus"];
                        }
                    }
                    else
                    {
                        //cells["Ball 6"] = ViewStyles["BallMatch"];
                    }
                }

                if ((row["Bonus Match"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Bonus Match"]))))
                {
                }

                if ((row["Win"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Win"]))))
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(row["Win"])))
                    {
                    }
                }

                if ((row["Division"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Division"]))))
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(row["Division"])))
                    {
                    }
                }

                if ((row["Winnings"] != DBNull.Value) && (!String.IsNullOrEmpty(Convert.ToString(row["Winnings"]))))
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(row["Winnings"])))
                    {
                    }
                }
                CellStyles.Add(cells);
            }
        }

    }
}
