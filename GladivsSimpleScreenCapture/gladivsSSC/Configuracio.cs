/********************************************************************************************
*	Gladivs Simple Screen Capture															*
*-------------------------------------------------------------------------------------------*
*	(c) Copyright  2015 Guillermo Espert Carrasquer											*
*===========================================================================================*
*																							*
*			Licensed under the Apache License, Version 2.0 (the "License");					*
*			you may not use this file except in compliance with the License.				*
*			You may obtain a copy of the License at											*
*																							*
*			http://www.apache.org/licenses/LICENSE-2.0										*
*																							*
*			Unless required by applicable law or agreed to in writing, software				*
*			distributed under the License is distributed on an "AS IS" BASIS,				*
*			WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.		*
*			See the License for the specific language governing permissions and				*
*			limitations under the License.													*
*																							*
*********************************************************************************************/
using System;
using System.Windows.Forms;
using gladivsSSC.Properties;
using gladivsSSC.Helpers;

namespace gladivsSSC
{
    public partial class Configuracio : Form
    {
        private bool _idiomaCanviat = false;
        private string _llengua;

        public Configuracio()
        {
            InitializeComponent();

            txtRutaDesat.Text = Settings.Default.CapturesPath;
            folderBrowserDialog1.SelectedPath = Settings.Default.CapturesPath;
            btnCancelar.Select();

            //Fixa  l'idioma actual de la aplicació en el combobox com a llengua sel·leccionada
            switch(Settings.Default.DefaultLang)
            {
                case null:
                    comboBox1.SelectedIndex = 0;
                    break;
                case "ca-ES":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "en":
                    comboBox1.SelectedIndex = 2;
                    break;
                case "es":
                    comboBox1.SelectedIndex = 3;
                    break;
                case "fr":
                    comboBox1.SelectedIndex = 4;
                    break;
                default:
                    comboBox1.SelectedIndex = 0;
                    break;
            }

            //Selecciona o deselecciona el checkbox de autodesat de les captures segons la configuració existent.
            if(Properties.Settings.Default.AutosaveCaptures)
            {
                ckbAutoDesatCaptures.Checked = true;
            }
        }

        private void btnDesar_Click(object sender, EventArgs e)
        {
            //Cada mètode de desat d'opcions està separat. Cadascun d'ells deu retornar true o false nomes com a indicatiu que el formulari a finalitzat i es pot tancar
            //Fins que tots els mètodes no retornen true el formulari permaneixerà obert. Cadascún dels mètodes deu gestionar els seus errors i tornar true quant es puga tancar el formulari
            //siga quin siga el resultat.
            bool resRutaDesat = DesarRutaCaptures();
            bool resCanviIdioma = DesarIdioma();
            bool resAutoDesatCaptures = DesarAutoDesatCaptura();

            //Verificar que tots els mètodes de desat autoritzen el tancament del formulari
            if(resRutaDesat && resCanviIdioma && resAutoDesatCaptures)
            {
               //Quant l'idioma canvia, no es mostra deseguida, si no que devem reinicialitzar la aplicació. Aquesta rutina mostra un messagebox per a que l'usuari decidisca si vol o no
               //reinicialitzar la aplicació.
               if(_idiomaCanviat)
               {
                    if(MessageBox.Show(this, msgVerificacioReiniciarAplicacio.Text, msgCaptionVerificarReinici.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                    else
                    {
                        //Correcte, tanquem finestra
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
               }
               else
               {
                    //Correcte, tanquem finestra
                    this.DialogResult = DialogResult.OK;
                    this.Close();
               }
            }

        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                txtRutaDesat.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private bool DesarAutoDesatCaptura()
        {
            if((Properties.Settings.Default.AutosaveCaptures && ckbAutoDesatCaptures.Checked) || (!Properties.Settings.Default.AutosaveCaptures && !ckbAutoDesatCaptures.Checked))
            {
                return true;
            }
            else
            {
                Properties.Settings.Default.AutosaveCaptures = ckbAutoDesatCaptures.Checked;
                Properties.Settings.Default.Save();

                return true;
            }
        }


        /// <summary>
        /// Comproba els possibles canvis i desa la nova configuració si es diferent a la que existeix en la aplicació.
        /// </summary>
        /// <returns></returns>
        private bool DesarIdioma()
        {
            if(_llengua != Properties.Settings.Default.DefaultLang)
            {
                Properties.Settings.Default.DefaultLang = _llengua;
                _idiomaCanviat = true;

                Properties.Settings.Default.Save();
                return true;
            }
            else
            {
                _idiomaCanviat = false;
                return true;
            }
        }


        /// <summary>
        /// Verifica els canvis en la ruta de desat i realitza les operacions necessàries per a la actualització de la configuració i creació de la carpeta si cal.
        /// </summary>
        /// <returns></returns>
        private bool DesarRutaCaptures()
        {
            //Verificar si el camp ruta està buit
            if (txtRutaDesat.Text != String.Empty)
            {
                //Executem la funció de verificació i creació de directoris definits
                if (FileSystem.VerifyPathToCapturesFolder(false, txtRutaDesat.Text))
                {
                    return true;
                }
                else
                {
                    //Error, mostrem el missatge
                    MessageBox.Show(msgErrorCreantDirectoriDesatCaptures.Text, msgErrorCaptionGeneric.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
            {
                //Al estar buit mostrem una advertència a l'usuari i li donem la possibilitat de definir una nova ruta o prendre la predeterminada
                DialogResult dr = MessageBox.Show(msgErrorNoPermesRutaDesatCaptruresBlanc.Text, msgErrorCaptionGeneric.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                //Tria prendre la predeterminada, per això executem la funció de verificació i creació predeterminada
                if (dr == DialogResult.No)
                {
                    if (FileSystem.VerifyPathToCapturesFolder())
                    {
                        return true;
                    }
                    else
                    {
                        //Mostrem error i parem la execució

                        MessageBox.Show(msgErrorCreantDirectoriDesatCaptures.Text, msgErrorCaptionGeneric.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    _llengua = null;
                    break;
                case 1:
                    _llengua = "ca-ES";
                    break;
                case 2:
                    _llengua = "en";
                    break;
                case 3:
                    _llengua = "es";
                    break;
                case 4:
                    _llengua = "fr";
                    break;
                default:
                    _llengua = null;
                    break;
            }
        }
    }
}
