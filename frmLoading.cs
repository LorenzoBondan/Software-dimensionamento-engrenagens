using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSM2
{
    public partial class frmLoading : Form
    {
        int contador = 0;
        public frmLoading()
        {
            InitializeComponent();

            //deixando os controles com fundo transparente
            lblOla.Parent = pbFundo;
            pictureBox1.Parent = pbFundo;
            label1.Parent = pbFundo;
            label2.Parent = pbFundo;
            btnEntrar.Parent = pbFundo;
            lblPorcentagem.Parent = pbFundo;
            //progressBar.Parent = pbFundo;
           
        }
        
        int counter = 0;
        int len = 0;
        string text;

        private void frmLoading_Load(object sender, EventArgs e)
        {

            btnEntrar.Visible = false;
            lblOla.Text = "";
            label2.Visible = false;
            text = "Bem Vindo " + Environment.UserName + "!";
            len = text.Length;
            timer1.Start();
            timer3.Start();

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblOla.Text = text.Substring(0,counter);
            ++counter;

            if(counter > len)
            {
                timer1.Stop();
            }

        }

        private void btnEntrar_Click_1(object sender, EventArgs e)
        {
            Hide();
            frmPrincipal f = new frmPrincipal();
            f.ShowDialog();
            Close();
            Dispose();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            progressBar.Value = contador;
            lblPorcentagem.Text = contador.ToString() + "%";
            contador++;

            if (contador == 100)
            {
                lblPorcentagem.Text = "100%";
                timer3.Stop();
                label2.Visible = true;
                label1.Visible = false;
                btnEntrar.Visible = true;
            }
        }


    }
}
