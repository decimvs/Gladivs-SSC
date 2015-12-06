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
    class ToolPointer : Tool
    {
        private static Image _image;

        public static Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private enum SelectionMode
        {
            None,
            Move, 
            Size        
        }

        private SelectionMode _selectMode = SelectionMode.None;

        private DrawRectangle _resizedObject;
        private int _resizedObjectHandle;

        private Point _lastPoint = new Point(0, 0);
        private Point _startPoint = new Point(0, 0);

        bool _wasMove;


        public override void OnMouseDown(RegionCapture regionForm, MouseEventArgs e)
        {
            _wasMove = false;

            _selectMode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);

            int handleNumber = regionForm.DrawRectangle.HitTest(point);

            if (handleNumber > 0)
            {
                _selectMode = SelectionMode.Size;


                _resizedObject = regionForm.DrawRectangle;
                _resizedObjectHandle = handleNumber;
            }

            if (_selectMode == SelectionMode.None)
            {
                DrawRectangle o = null;

                if (regionForm.DrawRectangle.HitTest(point) == 0)
                {
                    o = regionForm.DrawRectangle;
                }

                if (o != null)
                {
                    _selectMode = SelectionMode.Move;

                    regionForm.Cursor = Cursors.SizeAll;
                }
            }

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;
            _startPoint.X = e.X;
            _startPoint.Y = e.Y;

            regionForm.Capture = true;

            regionForm.Refresh();
        }


        public override void OnMouseMove(RegionCapture regionForm, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point oldPoint = _lastPoint;

            _wasMove = true;

            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                int n = regionForm.DrawRectangle.HitTest(point);

                if (n > 0)
                {
                    cursor = regionForm.DrawRectangle.GetHandleCursor(n);
                }

                if (cursor == null)
                {
                    cursor = Cursors.Default;
                }

                regionForm.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }


            int dx = e.X - _lastPoint.X;
            int dy = e.Y - _lastPoint.Y;

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;

            if (_selectMode == SelectionMode.Size)
            {
                if (_resizedObject != null)
                {
                    _resizedObject.MoveHandleTo(point, _resizedObjectHandle);
                    regionForm.Refresh();
                }
            }

            if (_selectMode == SelectionMode.Move)
            {
                regionForm.DrawRectangle.Move(dx, dy);

                regionForm.Cursor = Cursors.SizeAll;
                regionForm.Refresh();
            }
        }

        public override void OnMouseUp(RegionCapture regionForm, MouseEventArgs e)
        {
            if (_resizedObject != null)
            {
                //abans de redimensionar
                _resizedObject.Normalize();
                _resizedObject = null;
            }

            regionForm.Capture = false;
            regionForm.Refresh();
        }

        public override void OnMouseDoubleClick(RegionCapture regionForm, MouseEventArgs e)
        {
            Rectangle rect = ((DrawRectangle)regionForm.DrawRectangle).Rectangle;
            
            Image = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(_image);

            
            g.DrawImage(regionForm.BackgroundImage, 0, 0, rect, GraphicsUnit.Pixel);

            g.Dispose();
            
            regionForm.Screenshot = Image;
        }
    }
}
