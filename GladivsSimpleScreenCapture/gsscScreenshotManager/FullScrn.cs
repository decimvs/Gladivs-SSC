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
using System.Drawing;

namespace gsscScreenshotManager
{
    static public class FullScrn
    {
        private static Image _image;

        public static Image CapturaFullScr
        {
            get { return _image; }
            set { _image = value; }
        }

        public static Image Capture()
        {
            Size size = Screen.PrimaryScreen.Bounds.Size;

            _image = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(_image);

            g.CopyFromScreen(0, 0, 0, 0, size);
            g.Dispose();

            return _image;
        }
    }
}
