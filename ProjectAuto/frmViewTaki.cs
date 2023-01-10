using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ProjectAuto
{
    public partial class frmViewTaki : Telerik.WinControls.UI.RadTabbedForm
    {
        public List<ChromeView> views = new List<ChromeView>();
        public frmViewTaki()
        {
            this.FormClosing += frmViewTaki_FormClosing;
            this.FormClosed += frmViewTaki_FormClosed;
            InitializeComponent();
            this.AllowAero = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += tab_Resize;
        }

        #region Xữ lý Render
        public RadTabbedFormControl Tab
        {
            get { return this.tab; }
            set { tab = value; }
        }
        #endregion

        private void tab_Resize(object sender, EventArgs e)
        {
            //maximize = !maximize;
            //foreach(var view  in views)
            //{
            //    if (maximize)
            //    {
            //        view.driver.Manage().Window.Maximize();
            //    }
            //    else
            //    {
            //        view.driver.Manage().Window.Size = new Size(414, 896);
            //        view.driver.Manage().Window.Position = new Point(0, 0);
            //    }
            //}
        }

        private void frmViewTaki_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var view in views)
            {
                view.driver.Quit();
            }

            views = new List<ChromeView>();
        }

        private void frmViewTaki_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var view in views)
            {
                view.driver.Close();
                view.driver.Quit();
            }

            views = new List<ChromeView>();
        }
    }
}
