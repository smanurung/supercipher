using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace WordAddIn
{
    public partial class ThisAddIn
    {
        private Microsoft.Office.Tools.Word.Controls.Button ebutton = null;
        private Microsoft.Office.Tools.Word.Controls.Button dbutton = null;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Console.WriteLine("startup addin");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        internal void EncryptAddIn()
        {
            Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);

            //vstoDocument.ToString();
            //Console.WriteLine("Enkrip addin");
            
            //string name = "MyButton";

            //if (Globals.Ribbons.MyRibbon.addButtonCheckBox.Checked)
            //{
            //    Word.Selection selection = this.Application.Selection;
            //    if (selection != null && selection.Range != null)
            //    {
            //        button = vstoDocument.Controls.AddButton(
            //            selection.Range, 100, 30, name);
            //    }
            //}
            //else
            //{
            //    vstoDocument.Controls.Remove(name);
            //}
        }

        internal void DecryptAddIn()
        {

        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
