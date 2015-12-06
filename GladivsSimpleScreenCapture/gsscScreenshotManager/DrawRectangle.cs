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
using System.Windows.Forms;


namespace gsscScreenshotManager
{
    public class DrawRectangle
    {
        private Rectangle _rectangle;


        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public DrawRectangle()
            : this(0, 0, 1, 1)
        {
        }

        public DrawRectangle(int x, int y, int width, int height)
            : base()
        {
            _rectangle.X = x;
            _rectangle.Y = y;
            _rectangle.Width = width;
            _rectangle.Height = height;
        }

        public virtual void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.Black);

            g.DrawRectangle(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));
            DrawTracker(g);

            pen.Dispose();
        }

        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }


        public virtual void DrawTracker(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.Black);

            for (int i = 1; i <= HandleCount; i++)
            {
                g.FillRectangle(brush, GetHandleRectangle(i));
            }

            brush.Dispose();
        }

        protected void SetRectangle(int x, int y, int width, int height)
        {
            _rectangle.X = x;
            _rectangle.Y = y;
            _rectangle.Width = width;
            _rectangle.Height = height;
        }

        public virtual int HandleCount
        {
            get { return 8; }
        }

        public virtual Point GetHandle(int handleNumber)
        {
            int x, y, xCenter, yCenter;

            xCenter = _rectangle.X + _rectangle.Width / 2;
            yCenter = _rectangle.Y + _rectangle.Height / 2;
            x = _rectangle.X;
            y = _rectangle.Y;

            switch (handleNumber)
            {
                case 1:
                    x = _rectangle.X;
                    y = _rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = _rectangle.Y;
                    break;
                case 3:
                    x = _rectangle.Right;
                    y = _rectangle.Y;
                    break;
                case 4:
                    x = _rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = _rectangle.Right;
                    y = _rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = _rectangle.Bottom;
                    break;
                case 7:
                    x = _rectangle.X;
                    y = _rectangle.Bottom;
                    break;
                case 8:
                    x = _rectangle.X;
                    y = yCenter;
                    break;
            }

            return new Point(x, y);

        }

        public virtual int HitTest(Point point)
        {
            for (int i = 1; i <= HandleCount; i++)
            {
                if (GetHandleRectangle(i).Contains(point))
                    return i;
            }

            if (PointInObject(point))
            {
                return 0;
            }

            return -1;
        }

        protected virtual bool PointInObject(Point point)
        {
            return _rectangle.Contains(point);
        }

        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch (handleNumber)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }

        public virtual void Move(int deltaX, int deltaY)
        {
            _rectangle.X += deltaX;
            _rectangle.Y += deltaY;
        }

        public virtual void Normalize()
        {
            _rectangle = DrawRectangle.GetNormalizedRectangle(_rectangle);
        }

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if (y2 < y1)
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
    }
}
