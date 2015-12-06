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

using System.Drawing.Imaging;

namespace gladivsSSC.Helpers
{
    public class ImageFormats
    {
        private int _formatIndex;
        private string _formatName;
        private string _formatIcon;
        private ImageFormat _imageFormat;

        public int FormatIndex
        {
            get { return _formatIndex; }
        }

        public string FormatName
        {
            get { return _formatName; }
        }

        public string FormatIcon
        {
            get { return _formatIcon; }
        }

        public ImageFormat ImageFormat
        {
            get { return _imageFormat; }
        }

        /// <summary>
        /// Inicialitzador buit
        /// </summary>
        public ImageFormats()
        {
            
        }

        /// <summary>
        /// Inicialitza el objecte a partir de l'index del format
        /// </summary>
        /// <param name="index"></param>
        public ImageFormats(int index)
        {
            SetImageFormatInfo(index, null, null, null);

        }

        /// <summary>
        /// Inicialitza l'objecte a partir del nom del format
        /// </summary>
        /// <param name="formatName"></param>
        public ImageFormats(string formatName)
        {
            SetImageFormatInfo(-1, formatName, null, null);
        }

        /// <summary>
        /// Inicialitza l'objecte a partir del format ImageFormat
        /// </summary>
        /// <param name="imageFormat"></param>
        public ImageFormats(ImageFormat imageFormat)
        {
            SetImageFormatInfo(-1, null, imageFormat.ToString(), null);
        }

        /// <summary>
        /// Obte la informació d'un format d'imatge a partir de la seua extensió
        /// </summary>
        /// <param name="extension"></param>
        public void ImageFormatByExtension(string extension)
        {
            SetImageFormatInfo(-1, null, null, extension);
        }

        /// <summary>
        /// Classe privada per a gestionar el emplenat dels atributs
        /// </summary>
        /// <param name="index"></param>
        /// <param name="formatName"></param>
        /// <param name="imageFormat"></param>
        private void SetImageFormatInfo(int index=-1, string formatName=null, string imageFormat = null, string formatExtension = null)
        {
            switch(index)
            {
                case 0: //JPEG
                    _formatName = "JPEG";
                    _formatIcon = "Resources\\jpg_96px.png";
                    _formatIndex = 0;
                    _imageFormat = ImageFormat.Jpeg;
                    break;
                case 1: //PNG
                    _formatName = "PNG";
                    _formatIcon = "Resources\\png_96px.png";
                    _formatIndex = 1;
                    _imageFormat = ImageFormat.Png;
                    break;
                case 2: //GIF
                    _formatName = "GIF";
                    _formatIcon = "Resources\\gif_96px.png";
                    _formatIndex = 2;
                    _imageFormat = ImageFormat.Gif;
                    break;
                case 3: //TIFF
                    _formatName = "TIFF";
                    _formatIcon = "Resources\\tif_96px.png";
                    _formatIndex = 3;
                    _imageFormat = ImageFormat.Tiff;
                    break;
            }

            switch(formatName)
            {
                case "JPEG": //JPEG
                    _formatName = "JPEG";
                    _formatIcon = "Resources\\jpg_96px.png";
                    _formatIndex = 0;
                    _imageFormat = ImageFormat.Jpeg;
                    break;
                case "PNG": //PNG
                    _formatName = "PNG";
                    _formatIcon = "Resources\\png_96px.png";
                    _formatIndex = 1;
                    _imageFormat = ImageFormat.Png;
                    break;
                case "GIF": //GIF
                    _formatName = "GIF";
                    _formatIcon = "Resources\\gif_96px.png";
                    _formatIndex = 2;
                    _imageFormat = ImageFormat.Gif;
                    break;
                case "TIFF": //TIFF
                    _formatName = "TIFF";
                    _formatIcon = "Resources\\tif_96px.png";
                    _formatIndex = 3;
                    _imageFormat = ImageFormat.Tiff;
                    break;
            }

            switch (imageFormat)
            {
                case "Jpeg": //JPEG
                    _formatName = "JPEG";
                    _formatIcon = "Resources\\jpg_96px.png";
                    _formatIndex = 0;
                    _imageFormat = ImageFormat.Jpeg;
                    break;
                case "Png": //PNG
                    _formatName = "PNG";
                    _formatIcon = "Resources\\png_96px.png";
                    _formatIndex = 1;
                    _imageFormat = ImageFormat.Png;
                    break;
                case "Gif": //GIF
                    _formatName = "GIF";
                    _formatIcon = "Resources\\gif_96px.png";
                    _formatIndex = 2;
                    _imageFormat = ImageFormat.Gif;
                    break;
                case "Tiff": //TIFF
                    _formatName = "TIFF";
                    _formatIcon = "Resources\\tif_96px.png";
                    _formatIndex = 3;
                    _imageFormat = ImageFormat.Tiff;
                    break;
            }

            switch (formatExtension)
            {
                case ".jpeg":
                    _formatName = "JPEG";
                    _formatIcon = "Resources\\jpg_96px.png";
                    _formatIndex = 0;
                    _imageFormat = ImageFormat.Jpeg;
                    break;
                case ".png":
                    _formatName = "PNG";
                    _formatIcon = "Resources\\png_96px.png";
                    _formatIndex = 1;
                    _imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    _formatName = "GIF";
                    _formatIcon = "Resources\\gif_96px.png";
                    _formatIndex = 2;
                    _imageFormat = ImageFormat.Gif;
                    break;
                case ".tiff":
                    _formatName = "TIFF";
                    _formatIcon = "Resources\\tif_96px.png";
                    _formatIndex = 3;
                    _imageFormat = ImageFormat.Tiff;
                    break;
            }
        }

    }


}
