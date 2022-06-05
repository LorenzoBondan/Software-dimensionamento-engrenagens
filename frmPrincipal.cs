using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using exporttoword = Microsoft.Office.Interop.Word;

namespace PSM2
{
    public partial class frmPrincipal : Form
    {

        public string Texto;

        public double WtSaida;
        public double ns;
        public double i78;

        public int limiteinferior;
        public int limitesuperior;
        public int limiteinferiorSaida;
        public int limitesuperiorSaida;


        public double Te;
        public double Ts;
        public double dp;
        public double dm;

        public double vida;
        public double vidaSaida;

        public double Ks = 0;
        public double KsSaida = 0;
        public double Km1 = 0;
        public double Km2 = 0;

        public double kl = 0;
        public double klsaida = 0;

        public double CL;
        public double KT;

        public double sigmaB;
        public double sigmaBSaida;

        public double mt;
        

        public frmPrincipal()
        {
            InitializeComponent();

            lblData.Text = DateTime.Now.ToShortDateString();
            lblUser.Text = Environment.UserName;

        }

        private void btnDesenvolvimento1_Click_1(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento1.Height;
            panelCursor.Top = btnDesenvolvimento1.Top;

            pagesPaginas.PageIndex = 1;
            
        }

        private void btnDadosDeEntrada_Click_1(object sender, EventArgs e)
        {
            panelCursor.Height = btnDadosDeEntrada.Height;
            panelCursor.Top = btnDadosDeEntrada.Top;

            pagesPaginas.PageIndex = 0;
            
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString().ToString();
        }

        private void btnSair_Click_1(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void btnDesenvolvimento2_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 2;
            panelCursor.Height = btnDesenvolvimento2.Height;
            panelCursor.Top = btnDesenvolvimento2.Top;
        }

        #region CÁLCULOS ENTRADA
        private void Calculos_Entrada()
        {
            //int t = int.Parse((string)cb1.SelectedItem) + int.Parse((string)cb2.SelectedItem);
            //txtTeste.Text = t.ToString();

            // Potência
            double potencia = double.Parse(txtPotenciaRequerida.Text) * 736;
            txtPotencia.Text = potencia.ToString();

            //Texto = $"\n Potência: {txtPotencia.Text} W"; // ********************

            // Velocidade Angular
            double velocidadeangular = double.Parse(txtRotacaoEntrada.Text) * Math.PI / 30;
            txtVelocidadeAngular.Text = velocidadeangular.ToString("F3");

            //Texto = "\n Velocidade Angular:" + txtPotencia.Text;

        }
        #endregion

        #region CÁLCULOS PÁGINA 2
        private void Calculos_Pagina2()
        {
            #region ENGRENAMENTO COMPOSTO
            // ### ENGRENAMENTO COMPOSTO

            // Te = H / w
            Te = double.Parse(txtPotencia.Text) / double.Parse(txtVelocidadeAngular.Text);
            txtTe.Text = Te.ToString("F4");

            //Texto += "\n Te:" + Te; // *********************

            // i = mG = (N1/N2) * (N3/N4) * (N5/N6) * (N7/N8)
            double mG = (double.Parse(txtN1.Text) / double.Parse(txtN2.Text)) *
                        (double.Parse(txtN3.Text) / double.Parse(txtN4.Text)) *
                        (double.Parse(txtN5.Text) / double.Parse(txtN6.Text)) *
                        (double.Parse(txtN7.Text) / double.Parse(txtN8.Text));
            txtmG.Text = mG.ToString("F4");

            // Ts = Te * mG
            Ts = Te * mG;
            txtTs.Text = Ts.ToString("F4");

            // ns = Rotação de Entrada / mG
            ns = double.Parse(txtRotacaoEntrada.Text) / mG;
            txtns.Text = ns.ToString("F4");

            // i 7,8 = N8/N7
            i78 = double.Parse(txtN7.Text) / double.Parse(txtN8.Text);
            txti78.Text = i78.ToString("F4");

            // T4 = Ts / i 7,8
            double T4 = Ts / i78;
            txtT4.Text = T4.ToString("F4");

            #endregion

            #region PINHÃO CÔNICO DE ENTRADA
            // ### PINHÃO CÔNICO DE ENTRADA 

            // Wp = 2 * Te * 10^3 / m * N1
            double Wp = (2 * Te * 1000) / (double.Parse(txtModulo.Text) * double.Parse(txtN1.Text));
            txtWp.Text = Wp.ToString("F4");
            #endregion

            #region PINHÃO HELICOIDAL DE SAÍDA
            // ### PINHÃO HELICOIDAL DE SAÍDA 

            // Wt = 2 * T4 * 10^3 / m * N7
            WtSaida = (2 * T4 * 1000) / (double.Parse(txtModuloSaida.Text) * double.Parse(txtN8.Text));
            txtWt.Text = WtSaida.ToString("F4");
            #endregion

            #region VALIDAÇÃO DA LARGURA
            // ### VALIDAÇÃO DA LARGURA

            // dp = m * N2
            dp = double.Parse(txtModulo.Text) * double.Parse(txtN2.Text);
            txtdp.Text = dp.ToString("F4");

            // linha mostrando os limites de força
            limiteinferior = 8 * int.Parse(txtModulo.Text);
            limitesuperior = 16 * int.Parse(txtModulo.Text);
            lblLimitesForca.Text = $"{limiteinferior} N  < F < {limitesuperior} N ";

            // Ângulo
            double i12 = double.Parse(txtN1.Text) / double.Parse(txtN2.Text);
            double angulog = Math.Atan(i12) * (180 / Math.PI); // retorna em radianos, então converti pra graus
            double angulopgraus = 90 - angulog;
            txtAngulop.Text = angulopgraus.ToString("F4");

            // L = dp / 2 * sen (lp)
            double L = dp / (2 * Math.Sin(angulopgraus * Math.PI / 180)); // converti os graus
            txtL.Text = L.ToString("F4");

            // L/3
            double L3 = L / 3;
            txtL3.Text = L3.ToString("F4");

            // linha final
            if (limiteinferior < L3 && limitesuperior > L3)
            {
                lblResultadoComprimento.Text = $"Válido pois {L3.ToString("F4")} está entre {limiteinferior} e {limitesuperior}";
            }
            else
            {
                lblResultadoComprimento.Text = $"NÃO Válido pois {L3.ToString("F4")} NÃO está entre {limiteinferior} e {limitesuperior}";
            }


            #endregion

            #region RAIO INTERNO, MÉDIO, DIÂMETRO MÉDIO
            // ri = (dp/2) - Fmin * sen (lp)
            double ri = (dp / 2) - limiteinferior * (Math.Sin(angulopgraus * Math.PI / 180));
            txtri.Text = ri.ToString("F4");

            // rm = (ri + (dp/2)) / 2
            double rm = (ri + (dp / 2)) / 2;
            txtrm.Text = rm.ToString("F4");

            dm = rm * 2;
            txtdm.Text = dm.ToString("F4");
            #endregion
        }
        #endregion

        #region CÁLCULOS PÁGINA 3
        private void Calculos_Pagina3()
        {
            #region Wt ENTRADA

            // Wt = 2 * Tp / dm
            double wtEntrada = (2 * Te * 1000) / dm;
            txtWtDesenvolvimento2.Text = wtEntrada.ToString("F4");

            #endregion

            #region J e K's

            // J

            // Ka

            // Km
            
            if (limiteinferior < 50)
            {
                Km1 = 1.6;
                txtKm.Text = Km1.ToString();
            }
            else if (limiteinferior < 150)
            {
                Km1 = 1.7;
                txtKm.Text = Km1.ToString();
            }
            else
            {
                Km1 = 1.8;
                txtKm.Text = Km1.ToString();
            }

            // Ks
            
            if (int.Parse(txtModulo.Text) <= 5)
            {
                Ks = 1;
                txtKs.Text = Ks.ToString();
            }
            else
            {
                Ks = (1 + Math.Pow((int.Parse(txtModulo.Text) - 5), 1.2) * 0.025);
                txtKs.Text = Ks.ToString("F4");
            }

            // Kv

            txtdpKv.Text = dp.ToString("F4");

            double v = double.Parse(txtRotacaoEntrada.Text) * (Math.PI / 30) * (dp / 2000);
            txtvKv.Text = v.ToString("F4");

            // Kx

            #endregion

            #region σb

            // σb = Wt * (1 / m*F*J) * (Ka*Km*Ks / Kv * Kx)
            sigmaB = wtEntrada * (1 / (double.Parse(txtModulo.Text) * (8 * int.Parse(txtModulo.Text)) * double.Parse(txtJ.Text)) *
                (double.Parse(cbKa.Text) * Km1 * Ks / (double.Parse(txtKv.Text) * double.Parse(cbKx.Text))));
            txtSigmaB.Text = sigmaB.ToString("F4");

            #endregion
        }
        #endregion


        #region CÁLCULOS PÁGINA 4
        private void Calculos_Pagina4()
        {
            // Vida = NTotal = 1800 rot/min * 60 min/1h * 25000h  
            vida = double.Parse(txtRotacaoEntrada.Text) * 60 * 25000;
            txtVida.Text = vida.ToString("F4");

            // KT
            KT = 0;
            if (cbKT.Checked == true)
            {
                KT = (460 + double.Parse(txtt.Text)) / 710;
                txtKT.Text = KT.ToString("F4");
            }
            else
            {
                KT = 1;
                txtKT.Text = KT.ToString();
            }

            // KL
            
            if (rbKL6.Checked == true)
            {
                kl = 1.6831 * Math.Pow(vida, (-0.0323));
                txtKL.Text = kl.ToString("F4");
            }
            else if (rbKL3.Checked == true)
            {
                kl = 4.9404 * Math.Pow(vida, (-0.1045));
                txtKL.Text = kl.ToString("F4");
            }
            else if (rbKL1.Checked == true)
            {
                kl = 9.4518 * Math.Pow(vida, (-0.148));
                txtKL.Text = kl.ToString("F4");
            }
            else if (rbKL2.Checked == true)
            {
                kl = 6.1514 * Math.Pow(vida, (-0.1192));
                txtKL.Text = kl.ToString("F4");
            }
            else if (rbKL4.Checked == true)
            {
                kl = 2.3194 * Math.Pow(vida, (-0.0538));
                txtKL.Text = kl.ToString("F4");
            }
            else if (rbKL5.Checked == true)
            {
                kl = 1.3558 * Math.Pow(vida, (-0.0178));
                txtKL.Text = kl.ToString("F4");
            }

            // Sfb = ( KL / KT * KR) * Sfb'
            double sfb = (kl / KT * double.Parse(cbKR.Text) * double.Parse(txtSfbLinha.Text));
            txtSfb.Text = sfb.ToString("F4");

            // CS = Sfb / SigmaB 
            double cs = sfb / sigmaB;
            txtCS.Text = cs.ToString("F4");

        }
        #endregion

        #region CÁLCULOS PÁGINA 5
        private void Calculos_Pagina5()
        {
            // COEFICIENTES

            // Ca
            txtCa.Text = cbKa.Text;

            // Cm
            txtCm.Text = Km1.ToString();

            // Cs
            txtCoefCs.Text = Ks.ToString();

            // Cv
            txtCv.Text = txtKv.Text;

            // Cf
            txtCf.Text = 1.ToString();

            
            // TD
            double primeiraparte = (limiteinferior * double.Parse(txtI.Text) * double.Parse(txtKv.Text)) / (2000 * Ks * double.Parse(txtCmd.Text) * 1 * double.Parse(cbKa.Text) * double.Parse(txtCxc.Text));
            
            double segundaparte = Math.Pow((double.Parse(txtSfcLinha.Text) * dm * 0.774 * double.Parse(txtCH.Text)) / (0.634 * double.Parse(txtCp.Text) * 1 * 1), 2);

            double TD = primeiraparte * segundaparte;
               
            txtTD.Text = TD.ToString("F4");

            // Z
            double Z = 0;

            if (TD < Te)
            {
                Z = 1;
                txtZ.Text = Z.ToString();
            }
            else
            {
                Z = 0.667;
                txtZ.Text = Z.ToString();
            }

            // σc
            double sigmaC = double.Parse(txtCp.Text) * double.Parse(txtCb.Text) * Math.Sqrt( (2 * TD / (limiteinferior * double.Parse(txtI.Text) * dm * dm)) * Math.Pow( Te * 1000 / TD , Z) *
                (double.Parse(cbKa.Text) * Km1 * Ks * double.Parse(txtCf.Text) * double.Parse(txtCxc.Text) / double.Parse(txtCv.Text) ));
            txtSigmaC.Text = sigmaC.ToString("F4");

            // CL
            CL = 2.466 * Math.Pow(vida, (-0.056));
            txtCL.Text = CL.ToString("F4");

            // Sfc
            double Sfc = CL * double.Parse(txtCH.Text) * double.Parse(txtSfcLinha.Text) / 1 * 1;
            txtSfc.Text = Sfc.ToString("F4");

            // CSc
            double CSc = Sfc / sigmaC;
            txtCSc.Text = CSc.ToString("f4");

        }

        #endregion

        #region CÁLCULOS PÁGINA 6
        private void Calculos_Pagina6()
        {

            limiteinferiorSaida = int.Parse(txtModuloSaida.Text) * 8;
            limitesuperiorSaida = int.Parse(txtModuloSaida.Text) * 16;

            // mt
            mt = double.Parse(txtModuloSaida.Text) / Math.Cos(20 * Math.PI/180) ; // (20 graus) tem que converter os graus para radianos
            txtmt.Text = mt.ToString("F4");

            // Ks

            if (int.Parse(txtModuloSaida.Text) <= 5)
            {
                KsSaida = 1;
                txtKsSaida.Text = KsSaida.ToString();
            }
            else
            {
                KsSaida = (1 + Math.Pow((int.Parse(txtModuloSaida.Text) - 5), 1.2) * 0.025);
                txtKsSaida.Text = KsSaida.ToString("F4");
            }

            // Km

            if (limiteinferiorSaida < 50)
            {
                Km2 = 1.6;
                txtKmSaida.Text = Km2.ToString();
            }
            else if (limiteinferiorSaida < 150)
            {
                Km2 = 1.7;
                txtKmSaida.Text = Km2.ToString();
            }
            else
            {
                Km2 = 1.8;
                txtKmSaida.Text = Km2.ToString();
            }

            // Ts2
            txtTs2.Text = Ts.ToString("F4");

            // Ts1
            double Ts1 = Ts / ((double.Parse(txtN7.Text) / double.Parse(txtN8.Text))); 
            txtTs1.Text = Ts1.ToString("F4");

            // Wt
            double WtSaida = (2 * Ts1 * 1000) / (mt * double.Parse(txtN8.Text));
            txtWtSaida.Text = WtSaida.ToString("F4");

            // Vida Saída
            vidaSaida = (ns * i78) * 60 * 25000;
            txtVidaSaida.Text = vidaSaida.ToString("F4");

            // Kl saida
            klsaida = 1.6831 * Math.Pow(vidaSaida, -0.0323);
            txtKLSaida.Text = klsaida.ToString("F4");

            // sigmaB  <<<<----- 120 (?)
            double sigmaBSaida = ( WtSaida / ( mt * 120 * double.Parse(txtJSaida.Text)) ) * 
                (double.Parse(txtKaSaida.Text) * Km2 * KsSaida * 1 * 1 / 1) ;
            txtSigmaBSaida.Text = sigmaBSaida.ToString("F4");

            // Sfb
            double SfbSaida = klsaida * double.Parse(txtSfBLinhaSaida.Text);
            txtSfbSaida.Text = SfbSaida.ToString("F4");

            // CS
            double csSaida = SfbSaida / sigmaBSaida;
            txtCSSaida.Text = csSaida.ToString("F4");

        }

        #endregion

        #region CÁLCULOS PÁGINA 7
        private void Calculos_Pagina7()
        {
            // mt
            txtmtSaidaC.Text = mt.ToString("F4");

            // mG
            double mGSaidaC = double.Parse(txtN7.Text) / double.Parse(txtN8.Text);
            txtmGSaidaC.Text = mGSaidaC.ToString("F4");

            // pt
            double pt = Math.PI * mt;
            txtptSaidaC.Text = pt.ToString("F4");

            // pn
            double pn = pt * (Math.Cos(20 * Math.PI / 180));
            txtpnSaidaC.Text = pn.ToString("F4");

            // px
            double px = pn / (Math.Sin(20 * Math.PI / 180));
            txtpxSaidaC.Text = px.ToString("F4");

            // tan
            double tan = (180 / Math.PI) * Math.Atan((Math.Tan(25 * Math.PI / 180) / (Math.Cos(20 * Math.PI / 180)))) ;
            txttanSaidaC.Text = tan.ToString("F4");

            #region DENTES HELICOIDAIS
            // DENTES HELICOIDAIS

            // Np
            double Np = Math.Round((8 * double.Parse(txtN8.Text)) / mt) +1;
            txtNpSaidaC.Text = Np.ToString("F4");

            // Ng
            double Ng = Math.Round( Np * mGSaidaC) ;
            txtNgSaidaC.Text = Ng.ToString("F4");

            // dp
            double dpSaidaC = Np * mt;
            txtdpSaidaC.Text = dpSaidaC.ToString("F4");

            // dg
            double dgSaidaC = Ng * mt;
            txtdgSaidaC.Text = dgSaidaC.ToString("F4");

            // C
            double CSaidaC = dpSaidaC / 2 + dgSaidaC / 2;
            txtCSaidaC.Text = CSaidaC.ToString("F4");

            #endregion
            double rp = dpSaidaC / 2;
            double rg = dgSaidaC / 2;


            // Z
            // Z = ((dp / 2 + mt)² -(dp / 2 * cos(Φ))² )^0,5 + ((dg / 2 + mt)² -(dg / 2 * cos(Φ))² )^0,5
            double primeira = Math.Sqrt((Math.Pow(rp + mt, 2)) - (Math.Pow(rp * Math.Cos(tan * Math.PI / 180), 2)));
            double segunda = (Math.Sqrt(Math.Pow(rg + mt, 2) - Math.Pow(rg * Math.Cos(tan * Math.PI / 180), 2)));
            double terceira = CSaidaC * Math.Sin(tan * Math.PI / 180);

            double ZSaidaC = primeira + segunda - terceira;
            txtZSaidaC.Text = ZSaidaC.ToString("F4");

            // mp 
            double mp = ZSaidaC / (mt * Math.PI * Math.Cos(tan * Math.PI / 180));
            txtmpSaidaC.Text = mp.ToString("F4");

            // mF
            double mF = double.Parse(txtFSaidaC.Text) / px;
            txtmFSaidaC.Text = mF.ToString("F4");

            // na PARTE FRACIONAL DE mF
            string valor = mF.ToString();
            int espaco = valor.IndexOf(",");
            string na = "0" + valor.Substring(espaco, 5); 
            txtnaSaidaC.Text = na;

            // nr PARTE FRACIONAL DE mp
            string valor2 = mp.ToString();
            int espaco2 = valor2.IndexOf(",");
            string nr = "0" + valor2.Substring(espaco, 5);
            txtnrSaidaC.Text = nr;


            // Ψb 
            double phib = (180 / Math.PI) * Math.Acos( ( (Math.Cos(20 * Math.PI / 180)) * (Math.Cos(25 * Math.PI / 180)) ) / (Math.Cos(tan * Math.PI / 180)) );
            txtphiBSaidaC.Text = phib.ToString("F4");

            // Lmin
            // Lmin = mp * F - (1-na) * (1-nr) * px / cos(Ψb)
            double Lmin = mp * double.Parse(txtFSaidaC.Text) - (1 - double.Parse(na)) * (1 - double.Parse(nr)) * px / (Math.Cos(phib * Math.PI / 180));
            txtLminSaidaC.Text = Lmin.ToString("F4");

            // mN
            double mN = double.Parse(txtFSaidaC.Text) / Lmin;
            txtmNSaidaC.Text = mN.ToString("F4");

            // Pp
            // ρp = ( { 0,5[(rp + ap) + -(C - rg - ag)]}² -(rp * cos(Φ))² ) ^0,5
            double rop = Math.Sqrt(( Math.Pow(((rp + mt) + (CSaidaC - rg - mt)) / 2, 2)) - (Math.Pow(rp * Math.Cos(tan * Math.PI / 180), 2 )));
            txtropSaidaC.Text = rop.ToString("F4");

            // ρg 
            double rog = (CSaidaC * Math.Sin(tan * Math.PI / 180)) - rop;
            txtrogSaidaC.Text = rog.ToString("F4");

            // I
            // I = cos (Φ) / ( (1/ρp) + (1/ρg) * dp * mN )
            double I = (Math.Cos(tan * Math.PI / 180)) / (((1 / rop) + (1 / rog)) * dpSaidaC * mN);
            txtISaidaC.Text = I.ToString("F4");

            // CL Saida
            double CLSaida = 2.466 * Math.Pow(vidaSaida, -0.056);
            txtCLSaida.Text = CLSaida.ToString("F4");

            // SigmaC
            double sigmaCSaidaC = double.Parse(txtCp.Text) * Math.Sqrt( (WtSaida / (double.Parse(txtFSaidaC.Text) * I * dpSaidaC)) * (double.Parse(txtKaSaida.Text) * Km2 * 
                KsSaida * double.Parse(txtKfSaida.Text) / double.Parse(txtKvSaida.Text)  ));
            txtSigmaCSaidaC.Text = sigmaCSaidaC.ToString("F4");

            // Sfc
            double SfcSaidaC = (CLSaida * double.Parse(txtCH.Text) * double.Parse(txtSfcLinhaSaidaC.Text)) / (KT * double.Parse(cbKR.Text));
            txtSfcSaidaC.Text = SfcSaidaC.ToString("F4");

            // CS
            double CSSaidaC = SfcSaidaC / sigmaCSaidaC;
            txtCSSaidaC.Text = CSSaidaC.ToString("F4");
        }

        #endregion

        private void btnCalcular_Click(object sender, EventArgs e)
        {

            if (txtReducao.Text == "" || txtReducao.Text == "" || txtRotacaoEntrada.Text == "" || txtPotenciaRequerida.Text == "" || txtModulo.Text == "" || txtModuloSaida.Text == "" ||
                txtN1.Text == "" || txtN2.Text == "" || txtN3.Text == "" || txtN4.Text == "" || txtN5.Text == "" || txtN6.Text == "" || txtN7.Text == "" || txtN8.Text == "")
            {
                MessageBox.Show("Preencha todos os campos com bordas verdes!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // validação de F
            if (double.Parse(txtFSaidaC.Text) > double.Parse(txtModuloSaida.Text) * 16 && double.Parse(txtFSaidaC.Text) < double.Parse(txtModuloSaida.Text) * 8)
            {
                MessageBox.Show("A largura F não respeita o intervalo 8*m < F < 16*m.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Calculos_Entrada();
            Calculos_Pagina2();
            Calculos_Pagina3();
            Calculos_Pagina4();
            Calculos_Pagina5();
            Calculos_Pagina6();
            Calculos_Pagina7();
            EscreverDados();

            MessageBox.Show("Cálculos realizados!","", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Calculos_Entrada();
            Calculos_Pagina2();
            Calculos_Pagina3();
            Calculos_Pagina4();
            Calculos_Pagina5();
            Calculos_Pagina6();
            Calculos_Pagina7();
            EscreverDados();

            this.datagridKm.Rows.Add("50", "1,6");
            this.datagridKm.Rows.Add("150", "1,7");
            this.datagridKm.Rows.Add("250", "1,8");

            this.dataGridView1.Rows.Add("Aço", "A1-A5", "Endurecimento Completo", "<= 180 HB", "170 - 590 MPa");
            this.dataGridView2.Rows.Add("Aço", "A1-A5", "Endurecimento Completo", "<= 180 HB", "170 - 590 MPa");
            this.dataGridView3.Rows.Add("Aço", "A1-A5", "Nitroliza", "Nitretado", "90 HR15N", "280 MPa");
            this.dataGridView4.Rows.Add("Aço", "A1-A5", "Nitroliza", "Nitretado", "90 HR15N", "1340 MPa");

            pictureBox5.Parent = imgFundo1;
            bunifuSeparator42.Parent = panel2;

        }

        private void EscreverDados()
        {
            Texto = "PROJETO DE SISTEMAS MECÂNICOS 2";
            Texto += "\nAVALIAÇÃO 2";
            Texto += "\nGrupo 4: Alan Frizzo Koltz, Daltro Riconi, Everton Augusto Omizzolo, Fernando Dal Castel, Lorenzo Benatti Bondan";
            Texto += "\n\n=============================================================================";
            Texto += "\n\nRELATÓRIO DE EXTRAÇÃO DOS DADOS CALCULADOS PELO SOFTWARE";
            Texto += "\n=============================================================================";
            Texto += "\n\n### DADOS DE ENTRADA ###";
            Texto += $"\n\nRedução: {txtReducao.Text}";
            Texto += $"\nRotação de Entrada: {txtRotacaoEntrada.Text} RPM";
            Texto += $"\nPotência Requerida: {txtPotenciaRequerida.Text} CV";
            Texto += $"\nMódulo de Entrada: {txtModulo.Text}";
            Texto += $"\nMódulo de Saída: {txtModuloSaida.Text}";
            Texto += $"\n\nDentes: ";
            Texto += $"\nN1: {txtN1.Text} N2: {txtN2.Text}  N3: {txtN3.Text}  N4: {txtN4.Text}  N5: {txtN5.Text}  N5: {txtN5.Text}  N5: {txtN5.Text}  " +
                $"N6: {txtN6.Text}  N7: {txtN7.Text}  N8: {txtN8.Text}.";
            Texto += "\n\n### RESULTADOS ###";
            Texto += "\n### PINHÃO CÔNICO DE ENTRADA ###";
            Texto += "\n*Para σb:";
            Texto += $"\nResistência a fadiga do material selecionado = {txtSfbLinha.Text} MPa.";
            Texto += $"\nσb = {txtSigmaB.Text} MPa.";
            Texto += $"\nSfb = {txtSfb.Text} MPa.";
            Texto += $"\nCS = {txtCS.Text}.";
            Texto += "\n";
            Texto += "\n*Para σc:";
            Texto += $"\nResistência a fadiga do material selecionado = {txtSfcLinha.Text} MPa.";
            Texto += $"\nσb = {txtSigmaC.Text} MPa.";
            Texto += $"\nSfb = {txtSfc.Text} MPa.";
            Texto += $"\nCS = {txtCSc.Text}.";

            Texto += "\n\n### PINHÃO HELICOIDAL DE SAÍDA ###";
            Texto += "\n*Para σb:";
            Texto += $"\nResistência a fadiga do material selecionado = {txtSfBLinhaSaida.Text} MPa.";
            Texto += $"\nσb = {txtSigmaBSaida.Text} MPa.";
            Texto += $"\nSfb = {txtSfbSaida.Text} MPa.";
            Texto += $"\nCS = {txtCSSaida.Text}.";
            Texto += "\n";
            Texto += "\n*Para σc:";
            Texto += $"\nResistência a fadiga do material selecionado = {txtSfcLinhaSaidaC.Text} MPa.";
            Texto += $"\nσb = {txtSigmaCSaidaC.Text} MPa.";
            Texto += $"\nSfb = {txtSfcSaidaC.Text} MPa.";
            Texto += $"\nCS = {txtCSSaidaC.Text}.";
            Texto += "\n\n=============================================================================";

            txtConteudo.Text = Texto;
        }

        private void btnEscrever_Click(object sender, EventArgs e)
        {
            Texto += "\nImpresso por: " + Environment.UserName;
            Texto += "\nÀs " + lblHora.Text.ToString() + " - " + lblData.Text.ToString();

            txtConteudo.Text = Texto;

            try
            {
                exporttoword.Application wordapp = new exporttoword.Application();
                wordapp.Visible = true;
                exporttoword.Document worddoc;
                object wordobj = System.Reflection.Missing.Value;
                worddoc = wordapp.Documents.Add(ref wordobj);
                wordapp.Selection.TypeText(txtConteudo.Text);
                wordapp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao executar o comando.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        #region MOUSE ENTER E BOTÕES CLICK

        private void btnTabelaJ_Click_1(object sender, EventArgs e)
        {
            frmTabelaJ j = new frmTabelaJ();
            j.Show();
        }

        private void btnDesenvolvimento3_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 3;
            panelCursor.Height = btnDesenvolvimento3.Height;
            panelCursor.Top = btnDesenvolvimento3.Top;
        }

        private void btnDesenvolvimento4_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 4;
            panelCursor.Height = btnDesenvolvimento4.Height;
            panelCursor.Top = btnDesenvolvimento4.Top;
        }

        private void btnDesenvolvimento5_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 5;
            panelCursor.Height = btnDesenvolvimento5.Height;
            panelCursor.Top = btnDesenvolvimento5.Top;
        }

        private void btnDadosDeEntrada_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 0;
            panelCursor.Height = btnDadosDeEntrada.Height;
            panelCursor.Top = btnDadosDeEntrada.Top;
        }

        private void btnDesenvolvimento6_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 6;
            panelCursor.Height = btnDesenvolvimento6.Height;
            panelCursor.Top = btnDesenvolvimento6.Top;
        }

        private void btnDesenvolvimento8_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 8;
            panelCursor.Height = btnDesenvolvimento8.Height;
            panelCursor.Top = btnDesenvolvimento8.Top;
        }

        private void btnDesenvolvimento7_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 7;
            panelCursor.Height = btnDesenvolvimento7.Height;
            panelCursor.Top = btnDesenvolvimento7.Top;
        }

        private void btnDadosDeEntrada_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDadosDeEntrada.Height;
            panelCursor.Top = btnDadosDeEntrada.Top;
        }

        private void btnDesenvolvimento1_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento1.Height;
            panelCursor.Top = btnDesenvolvimento1.Top;
        }

        private void btnDesenvolvimento2_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento2.Height;
            panelCursor.Top = btnDesenvolvimento2.Top;
        }

        private void btnDesenvolvimento3_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento3.Height;
            panelCursor.Top = btnDesenvolvimento3.Top;
        }

        private void btnDesenvolvimento4_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento4.Height;
            panelCursor.Top = btnDesenvolvimento4.Top;
        }

        private void btnDesenvolvimento5_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento5.Height;
            panelCursor.Top = btnDesenvolvimento5.Top;
        }

        private void btnDesenvolvimento6_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento6.Height;
            panelCursor.Top = btnDesenvolvimento6.Top;
        }

        private void btnDesenvolvimento7_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenvolvimento7.Height;
            panelCursor.Top = btnDesenvolvimento7.Top;
        }

        private void btnDesenhos_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 9;
            panelCursor.Height = btnDesenhos.Height;
            panelCursor.Top = btnDesenhos.Top;
        }

        private void btnDesenhos_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnDesenhos.Height;
            panelCursor.Top = btnDesenhos.Top;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            pagesPaginas.PageIndex = 10;
            panelCursor.Height = btnExportar.Height;
            panelCursor.Top = btnExportar.Top;
        }

        private void btnExportar_MouseEnter(object sender, EventArgs e)
        {
            panelCursor.Height = btnExportar.Height;
            panelCursor.Top = btnExportar.Top;
        }

        #endregion

        #region TABELAS
        private void btnTabelaModulos_Click(object sender, EventArgs e)
        {
            frmModulos m = new frmModulos();
            m.Show();
        }

        private void btnTabelaSfbLinha_Click(object sender, EventArgs e)
        {
            frmSfblinha m = new frmSfblinha();
            m.Show();
        }

        private void btnTabelaKL_Click(object sender, EventArgs e)
        {
            frmTabelaKL m = new frmTabelaKL();
            m.Show();
        }

        private void btnTabelaI_Click(object sender, EventArgs e)
        {
            frmTabelaI m = new frmTabelaI();
            m.Show();
        }

        private void btnTabelaCp_Click(object sender, EventArgs e)
        {
            frmTabelaCp m = new frmTabelaCp();
            m.Show();
        }

        private void btnTabelaSfcLinha2_Click(object sender, EventArgs e)
        {
            frmSfcLinha m = new frmSfcLinha();
            m.Show();
        }

        private void btnJSaida_Click(object sender, EventArgs e)
        {
            frmTabelaJSaida m = new frmTabelaJSaida();
            m.Show();
        }

        private void btnTabelaSfbLinha2_Click(object sender, EventArgs e)
        {
            frmSfblinha m = new frmSfblinha();
            m.Show();
        }

        private void btnTabelaSfcLinhaSaidaC_Click(object sender, EventArgs e)
        {
            frmSfcLinha m = new frmSfcLinha();
            m.Show();
        }

        private void btnInstrucoes_Click(object sender, EventArgs e)
        {
            frmInstrucoes m = new frmInstrucoes();
            m.Show();
        }
        #endregion

        private void btnLimparDadosDeEntrada_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Deseja limpar os dados de entrada?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                txtReducao.Text = "";
                txtRotacaoEntrada.Text = "";
                txtPotenciaRequerida.Text = "";
                txtModulo.Text = "";
                txtModuloSaida.Text = "";

                txtN1.Text = "";
                txtN2.Text = "";
                txtN3.Text = "";
                txtN4.Text = "";
                txtN5.Text = "";
                txtN6.Text = "";
                txtN7.Text = "";
                txtN8.Text = "";

                MessageBox.Show("Os dados de entrada foram limpados.");
            }
            
        }

        private void btnVistaIsometrica_Click(object sender, EventArgs e)
        {
            pagesDesenhos.PageIndex = 0;
        }

        private void btnVistaSuperior_Click(object sender, EventArgs e)
        {
            pagesDesenhos.PageIndex = 1;
        }

        private void btnVistaLateral_Click(object sender, EventArgs e)
        {
            pagesDesenhos.PageIndex = 2;
        }
    }
}
