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
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;
using gsscScreenshotManager;
using gladivsSSC.Helpers;
using gladivsSSC.Forms;
using System.IO;
using gladivsSSC.Properties;
using System.Threading;

namespace gladivsSSC
{
    public partial class MainForm : Form
    {
        #region Definició de variables, constants, delegats, etc

        private RegionCapture _regionCapture;   //Variable interna per al control d'un formulari de regió.

        private Image _capture; //S'usa per a conservar una còpia de treball de la captura actual.

        private Image _zoomImage; //S'utilitza per a les operacions de redimensionat de la imatge en el visor.

        private ZoomWait _zoomWaitForm; //S'utilitza per a les operacions de redimensionat de la imatge en el visor

        private string _entryFieldWidthValue; //S'utilitza per a conservar el valor original del camp quant l'usuari fa click damunt

        private string _entryFieldHeightValue; //S'utilitza per a conservar el valor original del camp quant l'usuari fa click damunt

        private bool _controlKey = false; //S'utilitza en el KeyListener per a controlar la pulsació de la tecla control

        private bool _silentRun; //Bandera per a configurar el mode d'inicialització

        private bool _termineThread = false; //Bandera per a permetre o no el tancament total de la aplicació

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor principal, estableix la llengua en la que es mostrarà la aplicació.
        /// Si la llengua del sistema no correspon amb cap es mostrarà l'anglés
        /// Si hi ha una llengua definida a la configuarció, es mostrarà eixa.
        /// </summary>
        /// <param name="silentRun"></param>
        public MainForm(bool silentRun)
        {
            //Capturem la inicialització per a permitir triar la llengua en que es mostrarà la aplicació.
            if (String.IsNullOrEmpty(Properties.Settings.Default.DefaultLang))
            {
                string llenguaUsuari = Thread.CurrentThread.CurrentUICulture.Name;
                string[] codiIso = llenguaUsuari.Split(Convert.ToChar("-"));

                switch (codiIso[0])
                {
                    case "ca":
                        break;
                    case "es":
                        break;
                    case "en":
                        break;
                    case "fr":
                        break;
                    default:
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
                        break;
                }

                ContinueInitialization();
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.DefaultLang);
                ContinueInitialization();
            }


            _silentRun = silentRun;   
        }

        /// <summary>
        /// Segon mètode de construcció i inicialització de la clase
        /// </summary>
        private void ContinueInitialization()
        {
            InitializeComponent();

            //Delegat per a capturar els events de teclat
            KeyboardListener.EventHandler += new EventHandler(KeyboardListener_eventHandler);

            //Accés directe a l'aplicació, comencem directament amb la finestra minimitzada.
            if (_silentRun)
            {
                _silentRun = false;
            }
            else
            {
                MinimizeToSystemTray();
            }

            // Initializers
            IniImageFormatSelector();
            IniButtonsAndFieldsButtonsState();
        }  

        #endregion

        #region Inicialitzadors de caracteristiques

        /// <summary>
        /// Inicialitza el selector de formats segons la configuració
        /// </summary>
        private void IniImageFormatSelector()
        {
            //String en configuració. Format d'imatge predeterminat.
            string customFormat = Properties.Settings.Default.DefaultImageFormat;

            ImageFormats imf = new ImageFormats(customFormat);
            
            try
            {
                Image icon = new Bitmap(imf.FormatIcon);
                tsddmImageFormatSelect.Image = icon;
            }
            catch
            {
                //Misatge error de fitxers mancants, reinstalació necessària
            }
            
            tsddmImageFormatSelect.Text = imf.FormatName;
        }

        /// <summary>
        /// Inicialitza els botons amb l'estat desitjat
        /// </summary>
        private void IniButtonsAndFieldsButtonsState()
        {
            tsddmViewerZoomSelector.Enabled = false;
            tstxtWidth.Enabled = false;
            tstxtHeight.Enabled = false;
            tsbtSave.Enabled = false;
            tsbtSaveAs.Enabled = false;
        }

        #endregion

        #region Events de comportament i accions generals de la finestra

        /// <summary>
        /// Minimitza la finestra a l'àrea de notificació
        /// </summary>
        private void MinimizeToSystemTray()
        {
            //Ocultació de la finestra
            this.Opacity = 0;
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        /// <summary>
        /// Restaura la finestra que estaba minimitzada
        /// </summary>
        private void RestoreWindow()
        {
            //Restauració de la finestra
            this.Opacity = 1;
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
        }

        /// <summary>
        /// Gestiona la acció del menú de selecció de format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsddmImageFormatSelect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Image icon = null;
            ImageFormats imf = null;

            switch (e.ClickedItem.Text)
            {
                case "JPEG":
                    imf = new ImageFormats("JPEG");
                    icon = new Bitmap(imf.FormatIcon);

                    break;

                case "GIF":
                    imf = new ImageFormats("GIF");
                    icon = new Bitmap(imf.FormatIcon);
                    break;

                case "PNG":
                    imf = new ImageFormats("PNG");
                    icon = new Bitmap(imf.FormatIcon);
                    break;

                case "TIFF":
                    imf = new ImageFormats("TIFF");
                    icon = new Bitmap(imf.FormatIcon);
                    break;
            }

            tsddmImageFormatSelect.Image = icon;
            tsddmImageFormatSelect.Text = imf.FormatName;
        }

        /// <summary>
        /// Canvia l'estat i la icona del botó de retenció de les proporcions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtConstraitProportions_Click(object sender, EventArgs e)
        {
            if (tsbtConstraitProportions.Checked)
            {
                Image icon = new Bitmap("Resources\\unlock_96px.png");

                tsbtConstraitProportions.Image = icon;
                tsbtConstraitProportions.Checked = false;
            }
            else
            {
                Image icon = new Bitmap("Resources\\lock_96px.png");

                tsbtConstraitProportions.Image = icon;
                tsbtConstraitProportions.Checked = true;
            }
        }

        /// <summary>
        /// Obre la finestra de configuració
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings cg = new Settings();

            cg.ShowDialog();
        }

        /// <summary>
        /// Obre la finestra de informació sobre nosaltres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutUs abs = new AboutUs();
            abs.ShowDialog();
        }

        /// <summary>
        /// Event quant es tanca la finestra. Ens permet cancelar el tancament si no ho volem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verifiquem que la propietat tancamentFinal no haja canviat. Si el valor es true, el tancament es deu realitzar, ja que és l'usuari qui ha sol·licitat el tancament,
            // per la qual cosa no es cancel·larà el tancament i es finalitzarà l'aplicació
            if (!_termineThread)
            {
                //Cancelar el tancament de la finestra principal
                e.Cancel = true;

                MinimizeToSystemTray();
            }
        }

        /// <summary>
        /// Tancament total i finalització de l'aplicació
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _termineThread = true;

            this.Close();
        }

        /// <summary>
        /// Tancament total i finalització de l'aplicació
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            _termineThread = true;

            this.Close();
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        #endregion

        #region Mètodes i events de la captura de pantalla

        /// <summary>
        /// Event del botó Capturar regió de la barra de ferramentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtNewRegionCapture_Click(object sender, EventArgs e)
        {
            RegionScreenCapture();
        }

        /// <summary>
        /// Event del menú contextual de l'àrea de notificació per a la captura de regió
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            RegionScreenCapture();
        }

        /// <summary>
        /// Mètode per al event doubleclick del formulari RegionCapture
        /// Obté la captura i la mostra al visor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rfrm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = Properties.Settings.Default.AfterCaptureWindowState;

            boxViewer.Image = _regionCapture.Screenshot;
            boxViewer.Width = _regionCapture.Screenshot.Width;
            boxViewer.Height = _regionCapture.Screenshot.Height;

            AfterNewCaptureButtonsEnable();

            _capture = _regionCapture.Screenshot;

            _regionCapture.Dispose();

            //Assignar valors als camps Width i Height de la barra de ferramentes
            tstxtWidth.Text = _capture.Width.ToString();
            tstxtHeight.Text = _capture.Height.ToString();

            RestoreWindow();
        }

        /// <summary>
        /// Mètode per al click del botó per a capturar la pantalla completa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtNewFullScreenCapture_Click(object sender, EventArgs e)
        {
            FullScreenCapture();
        }

        /// <summary>
        /// Event del menú contextual de l'àrea de notificació
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FullScreenCapture();
        }

        /// <summary>
        /// Inicialitza el procés de captura de una regió de la pantalla
        /// </summary>
        private void RegionScreenCapture()
        {
            Size size = Screen.PrimaryScreen.Bounds.Size;
            Image image = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(0, 0, 0, 0, size);

            g.Dispose();

            _regionCapture = new RegionCapture();

            _regionCapture.Initialize();

            _regionCapture.ActiveTool = RegionCapture.RegionToolType.Rectangle;
            _regionCapture.DrawRectangle = new DrawRectangle();
            _regionCapture.BackgroundImage = image;
            _regionCapture.Show();

            //Capturar el event per a saber que s'ha fet doble click i tenim una captura
            _regionCapture.MouseDoubleClick += Rfrm_MouseDoubleClick;
        }

        /// <summary>
        /// Realitza una captura de la pantalla completa
        /// </summary>
        private void FullScreenCapture()
        {
            _capture = FullScrn.Capture();

            boxViewer.Image = _capture;
            boxViewer.Width = _capture.Width;
            boxViewer.Height = _capture.Height;

            AfterNewCaptureButtonsEnable();

            //Assignar valors als camps Width i Height de la barra de ferramentes
            tstxtWidth.Text = _capture.Width.ToString();
            tstxtHeight.Text = _capture.Height.ToString();

            RestoreWindow();
        }

        /// <summary>
        /// Activa tots els botons activats
        /// </summary>
        private void AfterNewCaptureButtonsEnable()
        {
            tsddmViewerZoomSelector.Enabled = true;
            tstxtWidth.Enabled = true;
            tstxtHeight.Enabled = true;
            tsbtSave.Enabled = true;
            tsbtSaveAs.Enabled = true;
        }

        #endregion

        #region Events i mètodes de control de zoom en el visor

        /// <summary>
        /// Gestiona la acció del menú de zoom. Crea un missatge d'espera i inicia un segon procés per a executar el redimensionat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsddmViewerZoomSelector_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text != "100%" || (boxViewer.Width != _capture.Width && boxViewer.Height != _capture.Height))
            {
                //Creació d'una instancia del formulari d'espera; càlcul de les coordenades per a centrar-lo i desplegament del matèix.
                _zoomWaitForm = new ZoomWait();

                Point pt = new Point(
                    Convert.ToInt32((this.Width / 2) + this.Left) - (_zoomWaitForm.Width / 2),
                    Convert.ToInt32((this.Height / 2) + this.Top) - (_zoomWaitForm.Height / 2)
                    );

                _zoomWaitForm.Location = pt;
                _zoomWaitForm.Show();

                //Inici del procés en segon pla per al redimensionat de la imatge.
                bgwZoomImageOnViewer.RunWorkerAsync(e);
            }
            else
            {
                throw new ArgumentException("Arguments used in control structure are wrong. Error cod. CHX02987");
            }
        }

        /// <summary>
        /// Aquest event s'inicia quant arranquem el mètode RunWorkerAsync() del BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwZoomImageOnViewer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //Assignació del tipus que li correspon al valor de l'atribut Argument, passat en la crida al mètode RunWorkerAsync()
            ToolStripItemClickedEventArgs args = e.Argument as ToolStripItemClickedEventArgs;

            //Recuperació del valor de zoom sel·leccionat per l'usuari i preparació per a usar-lo
            string itemText = args.ClickedItem.Text;
            tsddmViewerZoomSelector.Text = itemText;
            string[] valor = itemText.Split(Convert.ToChar("%"));

            //Crida a la funció que realitza el redimensionat real.
            ModifyZoomInViewer(Convert.ToInt32(valor[0]));
        }

        /// <summary>
        /// Event que es produeix quant el backGroundWorker a finalitzat la tasca assignada. Es carrega la nova imatge i es tanca la finestra d'espera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwZoomImageOnViewer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            boxViewer.Width = _zoomImage.Width;
            boxViewer.Height = _zoomImage.Height;
            boxViewer.Image = _zoomImage;

            _zoomWaitForm.Close();
            _zoomWaitForm.Dispose();
        }

        /// <summary>
        /// Realitza les tasques de redimensionat de la imatge
        /// </summary>
        /// <param name="zoom"></param>
        private void ModifyZoomInViewer(int zoom)
        {
            Size sz = new Size();

            //Càlcul de l'escalat de la imatge
            float flWidth;
            float flHeight;

            flWidth = (_capture.Width * zoom) / 100;
            flHeight = (_capture.Height * zoom) / 100;

            //Conversió i assignació dels valors a l'objecte size
            sz.Width = Convert.ToInt32(flWidth);
            sz.Height = Convert.ToInt32(flHeight);

            _zoomImage = new Bitmap(_capture, sz);
        }

        /// <summary>
        /// Restaura la finestra al fer doble clic sobre la icona a l'àrea de notificació
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreWindow();
        }

        #endregion

        #region Mètodes i events del càlcul de dimensions de l'imatge (TODO: missatges d'error dels mètodes)

        /// <summary>
        /// Inicia el càlcul de les dimensions quant es pressiona la tecla enter sobre el camp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstxtWidth_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tsbtConstraitProportions.Checked)
            {
                CaptureProportionsModifier();
            }
        }

        /// <summary>
        /// Realitza els càlculs necessàris per a actualitzar les dades en els camps width i height
        /// </summary>
        private void CaptureProportionsModifier()
        {
            try
            {
                //Probar a convertir els valors de la captura valida els valors
                decimal dCpHeight = Convert.ToDecimal(_capture.Height); //Alt de la captura
                decimal dCpWidth = Convert.ToDecimal(_capture.Width);   //Ample de la captura

                decimal dRatio;     //Ratio de conversió
                decimal dOrWidth;   //Ample obtingut de l'event onmouseup
                decimal dOrHeight;  //Alt obtingut de l'event onmouseup

                try
                {
                    //Probar a convertir els valors originals obtinguts amb l'event mouseup
                    dOrWidth = Convert.ToDecimal(_entryFieldWidthValue);
                    dOrHeight = Convert.ToDecimal(_entryFieldHeightValue);

                    try
                    {
                        //Provem a convertir els valors actuals a decimal. Si valors incorrectes, error
                        decimal dAcWidth = Convert.ToDecimal(tstxtWidth.Text);
                        decimal dAcHeight = Convert.ToDecimal(tstxtHeight.Text);

                        if ((dOrWidth > 0 && dOrHeight > 0) && (dAcWidth > 0 && dAcHeight > 0))
                        {
                            //El ratio es calcula a partir de les dimensions originals de la captura
                            dRatio = dCpWidth / dCpHeight;

                            if (dOrWidth != dAcWidth && dOrHeight == dAcHeight)
                            {
                                if (dAcWidth > dAcHeight)
                                {
                                    tstxtHeight.Text = Convert.ToString(Convert.ToInt32(dAcWidth / dRatio));
                                }
                                else
                                {
                                    tstxtHeight.Text = Convert.ToString(Convert.ToInt32(dAcWidth * dRatio));
                                }

                            }
                            else if (dOrWidth == dAcWidth && dOrHeight != dAcHeight)
                            {
                                if (dAcWidth > dAcHeight)
                                {
                                    tstxtWidth.Text = Convert.ToString(Convert.ToInt32(dAcHeight * dRatio));
                                }
                                else
                                {
                                    tstxtWidth.Text = Convert.ToString(Convert.ToInt32(dAcHeight / dRatio));
                                }
                            }
                        }
                        else
                        {
                            //Valors no poden ser igual a zero o buits
                        }
                    }
                    catch
                    {
                        //Excepció valors de width o height dels camps no vàlids
                    }
                   
                }
                catch
                {
                    //Excepció valors de width o height dels camps no vàlids
                }
            }
            catch
            {
                //Error fatal, captura no te valors vàlids
                MessageBox.Show("Excepció");
            }
        }

        /// <summary>
        /// Inicia el procediment de càlcul de les dimensions de la imatge en pressionar la tecla enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstxtHeight_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tsbtConstraitProportions.Checked)
            {
                CaptureProportionsModifier();
            }
        }

        /// <summary>
        /// Grava els valors actuals dels camps witdh i height en fer click sobre un d'ells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstxtWidth_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _entryFieldWidthValue = tstxtWidth.Text;
                _entryFieldHeightValue = tstxtHeight.Text;
            }
        }

        /// <summary>
        /// Grava els valors actuals dels camps witdh i height en fer click sobre un d'ells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstxtHeight_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _entryFieldWidthValue = tstxtWidth.Text;
                _entryFieldHeightValue = tstxtHeight.Text;
            }
        }
        #endregion

        #region Mètodes i events del desat d'imatges
        /// <summary>
        /// Gestor de l'event del botó desar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtSave_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void tsbtSaveAs_Click(object sender, EventArgs e)
        {
           SaveImageAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImageAs();
        }

        /// <summary>
        /// Mètode comú per a desar com
        /// </summary>
        private void SaveImageAs()
        {
            ImageFormats imf = new ImageFormats(tsddmImageFormatSelect.Text);

            saveFileDialog1.FilterIndex = imf.FormatIndex + 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveImage(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Desa la imatge. Si se defineix un path, l'imatge es desarà en eixe path (deu incloure el nom de fitxer i la extensió).
        /// Si posem silentSave a true, no es mostra cap missatge de succès, però si els de error.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="silentSave"></param>
        private void SaveImage(string path = null, bool silentSave = false)
        {
            string fromPath;        //Variable per al path
            bool fromSettings;      //Variable per als settings
            bool PathDefined;       //Varialbe per al control de path definit o de settings
            string savePath;        //Ruta de la carpeta de desat
            string fileName;        //Nom de l'arxiu desat
            string fullSavePath;    //Ruta completa de desat (amb nom de fitxer inclòs)
            ImageFormat imageFormat;

            // Verificació de path rebut i adaptació dels arguments per a cridar a la funció de verificació de path
            if (String.IsNullOrEmpty(path))
            {
                ImageFormats imgFormat = new ImageFormats(tsddmImageFormatSelect.Text);

                fromSettings = true;
                fromPath = null;
                PathDefined = false;
                imageFormat = imgFormat.ImageFormat;
            }
            else
            {
                string extension = Path.GetExtension(path);
                ImageFormats imgFormat = new ImageFormats();
                imgFormat.ImageFormatByExtension(extension);

                fromSettings = false;
                fromPath = path;
                PathDefined = true;
                imageFormat = imgFormat.ImageFormat;
            }


            //Primer verifiquem la existencia del directori de desat
            if (FileSystem.VerifyPathToCapturesFolder(fromSettings, fromPath))
            {
                try
                {
                    int width = Convert.ToInt32(tstxtWidth.Text);
                    int height = Convert.ToInt32(tstxtHeight.Text);
                    
                    //Convertim la captura original al tamany especificat en la barra de ferramentes
                    Bitmap imageEscalada = new Bitmap(_capture, width, height);

                    if (PathDefined)
                    {
                        savePath = Path.GetDirectoryName(path);
                        fileName = Path.GetFileName(path);
                        fullSavePath = path;
                    }
                    else
                    {
                        savePath = Properties.Settings.Default.CapturesPath;
                        fileName = FileNameGenerator(imageFormat.ToString());
                        fullSavePath = savePath + "\\" + fileName + "." + imageFormat;
                    }

                    //Provem de desar l'imatge
                    try
                    {
                        imageEscalada.Save(fullSavePath, imageFormat);

                        if (File.Exists(fullSavePath))
                        {
                            System.GC.Collect();

                            //Desat correcte
                            MessageBox.Show(this, "The capture was saved correctly", "Capture saved correctly",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //Error de desat
                            MessageBox.Show(this, "An error occured when saving capture. Retry selecting other folder, it's possible that you don't have permissions to write in this folder.", "Error Saving",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch
                    {
                        //Error de desat
                        MessageBox.Show(this, "An error occurrred when saving capture. It seems to be a problem with codification of the image. Retry with other image format. If the problem persists, the image is damaged and can't be recovered.", "Capture saved correctly",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    //Error de format
                    MessageBox.Show(this,
                        "The values of width and/or height are wrong. Please verify that are numeric and greater than 0",
                        "Error width/height", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //Error en la creació del directori. Mostrem un missatge al usuari.
                MessageBox.Show(this, "An error occured when creating a new directory. Retry selecting other folder, it's possible that you don't have permissions to write in this folder.", "Error Saving",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FileNameGenerator(string fileExtension)
        {
            string savePath = Properties.Settings.Default.CapturesPath;

            //Creació d'un id únic i divisió en 5 cadenes per a utilitzar les seues parts individualment per a reduir la llargària.
            string longId = Guid.NewGuid().ToString();
            string[] shortId = longId.Split(Char.Parse("-"));

            DateTime dt = DateTime.Now;

            string fileName = "capture-" + dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute;
            string fullSavePath = savePath + "\\" + fileName + "." + fileExtension;

            if (File.Exists(fullSavePath))
            {
                fileName = fileName + "-" + shortId[1];
            }

            return fileName;
        }

        #endregion

        #region Mètodes i events de captura de tecles/teclat

        /// <summary>
        /// Gestiona les tecles pressionades i realitza les funcions definides per a certes tecles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardListener_eventHandler(object sender, EventArgs e)
        {
            KeyboardListener.KeyboardEventArgs eventArgs = (KeyboardListener.KeyboardEventArgs)e;

            string key = string.Format(eventArgs.KeyData.ToString().ToLower());

            if (eventArgs.MMessage == KeyboardListener.KeyDown)
            {
                //PrintScreen
                if (!_controlKey && (eventArgs.KeyData == Keys.PrintScreen || eventArgs.KeyData == Keys.Snapshot))
                {
                    FullScreenCapture();
                }
                //CTRL + PrintScreen
                else if (_controlKey && (eventArgs.KeyData == Keys.PrintScreen || eventArgs.KeyData == Keys.Snapshot))
                {
                    RegionScreenCapture();
                }
                //Controlador de pressió de la tecla Control
                else if (eventArgs.KeyData == Keys.ControlKey)
                {
                    _controlKey = true;
                }
                else
                {
                    _controlKey = false;
                }
            }
        }


        #endregion
    }
}