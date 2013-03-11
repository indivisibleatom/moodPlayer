using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace moodPlayer
{
    partial class MoodGraph
    {
        System.Drawing.Drawing2D.GraphicsPath m_linePoints;
        System.Drawing.Drawing2D.GraphicsPath m_gridPoints;
        System.Drawing.Drawing2D.GraphicsPath m_emphLine;
        System.Drawing.Drawing2D.GraphicsPath m_emphPoints;

        private Point m_lastPoints;

        //Constants...todo msati3: make static??
        private int m_numGridLinesX;
        private int m_numGridLinesY;
        private int m_gridSize;

        private List<float> m_intensityValues;
        PlayListCreator m_playListCreator;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        override protected void OnLoad(EventArgs e)
        {
            m_linePoints = new GraphicsPath();
            m_gridPoints = new GraphicsPath();
            m_emphLine = new GraphicsPath();
            m_emphPoints = new GraphicsPath();

            m_playListCreator = new PlayListCreator("songData.txt");

            m_intensityValues = new List<float>();

            m_numGridLinesX = 15;
            m_numGridLinesY = 10;
            m_gridSize = 40;

            invalidateMouse();
            drawGrid();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Rectangle getGridRect(int row, int col)
        {
            int y = row * m_gridSize;
            int x = col * m_gridSize;
            int width = m_gridSize;
            int height = m_gridSize;

            return new Rectangle(x, y, width, height);
        }

        private Point getPointFor(int row, int col)
        {
            int y = row * m_gridSize;
            int x = col * m_gridSize;
            return new Point(x, y);
        }

        private void drawGrid()
        {
            for (int i = 0; i < m_numGridLinesY; i++)
            {
                for (int j = 0; j < m_numGridLinesX; j++)
                {
                    m_gridPoints.AddRectangle(getGridRect(i, j));
                }
            }
            m_emphLine.AddLine(getPointFor(m_numGridLinesY / 2, 0), getPointFor(m_numGridLinesY / 2, m_numGridLinesX));
        }

        private bool isIntersectingGrid(Point mousePos)
        {
            int eps = 5;
            if (mousePos.X % m_gridSize <= eps && m_intensityValues.Count <= mousePos.X / m_gridSize)
            {
                m_intensityValues.Add(mousePos.Y);
                return true;
            }
            return false;
        }

        private Rectangle getRectCenteredAt(Point point)
        {
            //TODO msati3: put inside somewhere
            int width = 8;
            int height = 8;
            Point start = new Point(point.X - 4, point.Y - 4);
            return new Rectangle(start.X, start.Y, width, height);
        }

        private void invalidateMouse()
        {
            m_lastPoints.X = m_lastPoints.Y = -1;
        }

        private bool mousePositionInvalid()
        {
            return (m_lastPoints.X == -1);
        }

        private Rectangle getRectFrom(Point pointLast, Point pointCur)
        {
            int offset = 10;
            int x = pointLast.X < pointCur.X ? pointLast.X : pointCur.X;
            int width = Math.Abs(pointLast.X - pointCur.X) + 1;
            int y = pointLast.Y < pointCur.Y ? pointLast.Y : pointCur.Y;
            int height = Math.Abs(pointLast.Y - pointCur.Y) + 1;
            return (new Rectangle(x - offset, y - offset, width + offset, height + offset));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawPath(new Pen(Color.Black), m_gridPoints);
            e.Graphics.DrawPath(new Pen(new SolidBrush(Color.Green), 3), m_linePoints);
            e.Graphics.DrawPath(new Pen(new SolidBrush(Color.Red), 3), m_emphLine);
            e.Graphics.DrawPath(new Pen(new SolidBrush(Color.Indigo), 1), m_emphPoints);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left && !mousePositionInvalid())
            {
                Point mousePos = PointToClient(Control.MousePosition);
                m_linePoints.AddLine(m_lastPoints.X, m_lastPoints.Y, mousePos.X, mousePos.Y);
                Rectangle invalidateRect = getRectFrom(m_lastPoints, mousePos);
                Invalidate(invalidateRect);
                m_lastPoints = mousePos;
                if (isIntersectingGrid(mousePos))
                {
                    m_emphPoints.AddRectangle(getRectCenteredAt(mousePos));
                }
            }
        }

        private void label1_OnEnter(object sender, EventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                m_lastPoints = PointToClient(Control.MousePosition);
            }
        }

        private void label_OnLeave(object sender, EventArgs e)
        {
            invalidateMouse();
        }

        private void label1_OnDown(object sender, MouseEventArgs e)
        {
            m_lastPoints = PointToClient(Control.MousePosition);
        }

        private void label1_OnMouseUp(object sender, MouseEventArgs e)
        {
            normalizeCoordinates();
            invalidateMouse();
        }

        private void normalizeCoordinates()
        {
            int maxVal = this.DisplayRectangle.Height - 1;
            for (int i = 0; i < m_intensityValues.Count; i++)
            {
                m_intensityValues[i] = maxVal - m_intensityValues[i];
                m_intensityValues[i] /= maxVal;
                //System.Console.WriteLine(m_intensityValues[i]); //Enable for debugging
            }
            m_playListCreator.createPlayList(m_intensityValues);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MoodGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "MoodGraph";
            this.Size = new System.Drawing.Size(800, 400);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_OnDown);
            this.MouseEnter += new System.EventHandler(this.label1_OnEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_OnMouseUp);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
