using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ttpim.gamemodule.common
{
    public static class ApplicationController
    {
        public static Image GetNerdiImage(DayOfWeek day)
        {
            Image result = null;
            switch (day)
            {
                case DayOfWeek.Friday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdread;
                    break;
                case DayOfWeek.Monday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdi_girl3;
                    break;
                case DayOfWeek.Saturday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdi_girl5;
                    break;
                case DayOfWeek.Sunday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdi_girl4;
                    break;
                case DayOfWeek.Thursday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdi_girl2;
                    break;
                case DayOfWeek.Tuesday:
                    result = global::ttpim.gamemodule.Properties.Resources.Nerdi;
                    break;
                case DayOfWeek.Wednesday:
                    result = global::ttpim.gamemodule.Properties.Resources.nerdi_girl2;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
