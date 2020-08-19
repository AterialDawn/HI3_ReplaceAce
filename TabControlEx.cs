using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HI3_ReplaceAce
{
    public class TabControlEx : TabControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the tab headers should be drawn
        /// </summary>
        [Description("Gets or sets a value indicating whether the tab headers should be drawn"), DefaultValue(true)]
        public bool ShowTabHeaders { get; set; }
        public bool CheckDesignMode { get; set; }
        public TabControlEx()
        {
        }
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (!ShowTabHeaders && m.Msg == 0x1328 && (CheckDesignMode ? !DesignMode : true))
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }
    }
}
