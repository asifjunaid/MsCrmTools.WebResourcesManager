using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscrmTools.WebresourcesManager.AutoResponder
{
    public class ConsoleWriter : System.IO.TextWriter
    {
        System.Windows.Forms.ListView lst;
        public bool isWritingEnabled = false;
        public bool isWebResourceActivityEnabled = true;
        public ConsoleWriter(System.Windows.Forms.ListView lst)
        {
            this.lst = lst;
        }
        public override Encoding Encoding => System.Text.Encoding.UTF8;
        public override void WriteLine(string value)
        {
            if (lst.IsHandleCreated )
            {
                
                lst.BeginInvoke((Action)(() =>
                {
                    if (isWritingEnabled)
                        lst.Items.Add(value.ToString());
                    else if( isWebResourceActivityEnabled && value.IndexOf("AUTORESPONDER ") > -1)
                        lst.Items.Add(value.ToString().Replace("AUTORESPONDER ", ""));

                }
                ));
            }
        }

    }
    
}
