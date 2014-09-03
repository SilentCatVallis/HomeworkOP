using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manipulator
{
    //Этот небольшой костыль нужен, чтобы перерисовка происходила без миганий
    public class AntiFlickerForm : Form
    {
        public AntiFlickerForm()
        {
            DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AntiFlickerForm
            // 
            this.ClientSize = new System.Drawing.Size(592, 514);
            this.Name = "AntiFlickerForm";
            this.ResumeLayout(false);

        }
    }
}
