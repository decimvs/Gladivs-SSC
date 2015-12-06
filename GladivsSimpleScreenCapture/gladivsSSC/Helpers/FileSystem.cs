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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace gladivsSSC.Helpers
{
    public static class FileSystem
    {
        public static bool VerifyPathToCapturesFolder(bool inSettings = true, string path = null)
        {
            string savePath;

            if (inSettings)
            {
                //Primer verifiquem si la ruta de desat està definida a la configuració, si no és així, en definim una de forma predeterminada
                if (String.IsNullOrEmpty(Properties.Settings.Default.CapturesPath))
                {
                    Properties.Settings.Default.CapturesPath =
                        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +
                        "\\Gladivs-SimpleScreenCapture";

                    Properties.Settings.Default.Save();
                }

                savePath = Properties.Settings.Default.CapturesPath;

            }
            else
            {
                if (!String.IsNullOrEmpty(path))
                {
                    savePath = Path.GetDirectoryName(path);
                }
                else
                {
                    return false;
                }
            }

            //Segon, verifiquem si existeix un directori en la ruta definida per la variable de configuració, si no existeix, el creem.
            if (!Directory.Exists(savePath))
            {
                try
                {
                    Directory.CreateDirectory(savePath);

                    return true;
                }
                catch (Exception e)
                {
                    //Error en la creació del directori
                    return false;
                }
            }
            else
            {
                //El directori ja existeix.
                return true;
            }
        }
    }
}
