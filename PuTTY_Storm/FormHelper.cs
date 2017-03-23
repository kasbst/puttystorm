using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public partial class FormHelper : Form
    {
        public FormHelper()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        }

        public FormHelper return_form2_object ()
        {
            return this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
