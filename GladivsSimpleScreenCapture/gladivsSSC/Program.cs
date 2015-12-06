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
using System.Windows.Forms;

namespace gladivsSSC
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            bool silentRun = false;

            if (args.Count() > 0)
            {
                if (args[0] == "silentRun")
                {
                    silentRun = true;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(silentRun));
        }
    }
}
