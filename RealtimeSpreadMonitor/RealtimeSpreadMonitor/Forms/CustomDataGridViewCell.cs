using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.Forms
{
    class CustomDataGridViewCell : DataGridViewTextBoxCell
    {
        private DataGridViewAdvancedBorderStyle _style;

        public CustomDataGridViewCell(bool isSumLine)
        //: base()
        {
            _style = new DataGridViewAdvancedBorderStyle();

            if (isSumLine)
            {
                _style.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                _style.Top = DataGridViewAdvancedCellBorderStyle.Single;
                _style.Left = DataGridViewAdvancedCellBorderStyle.None;
                _style.Right = DataGridViewAdvancedCellBorderStyle.Single;
            }
            else
            {
                _style.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                _style.Top = DataGridViewAdvancedCellBorderStyle.None;
                _style.Left = DataGridViewAdvancedCellBorderStyle.None;
                _style.Right = DataGridViewAdvancedCellBorderStyle.Single;
            }


        }

        //         public DataGridViewAdvancedBorderStyle AdvancedBorderStyle
        //         {
        //             get { return _style; }
        //             set
        //             {
        //                 _style.Bottom = value.Bottom;
        //                 _style.Top = value.Top;
        //                 _style.Left = value.Left;
        //                 _style.Right = value.Right;
        //             }
        //         }

        protected override void PaintBorder(Graphics graphics, Rectangle clipBounds, Rectangle bounds, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle)
        {

            //cellStyle. = Color.GreenYellow;



            base.PaintBorder(graphics, clipBounds, bounds, cellStyle, _style);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            //clipBounds..BackColor = Color.GreenYellow;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, _style, paintParts);
        }
    }
}
