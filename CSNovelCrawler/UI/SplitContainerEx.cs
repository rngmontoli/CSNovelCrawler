/*
 * 
 * Email:huliang@yahoo.cn
 * Date:2011-01-12
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace CSNovelCrawler.UI
{
    [ToolboxBitmap(typeof(SplitContainer))]
    public class SplitContainerEx : SplitContainer
    {
        enum MouseState
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal,
            /// <summary>
            /// 鼠标移入
            /// </summary>
            Hover
        }

        public SplitContainerEx()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.SplitterWidth = 9;
            this.Panel1MinSize = 0;
            this.Panel2MinSize = 0;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int SplitterWidth
        {
            get
            {
                return base.SplitterWidth;
            }
            set
            {
                base.SplitterWidth = 9;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int Panel1MinSize
        {
            get
            {
                return base.Panel1MinSize;
            }
            set
            {
                base.Panel1MinSize = 0;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int Panel2MinSize
        {
            get
            {
                return base.Panel2MinSize;
            }
            set
            {
                base.Panel2MinSize = 0;
            }
        }

        public enum SplitterPanelEnum
        {
            Panel1,
            Panel2
        }

        SplitterPanelEnum mCollpasePanel = SplitterPanelEnum.Panel2;
        /// <summary>
        /// 进行折叠或展开的SplitterPanel
        /// </summary>
        [DefaultValue(SplitterPanelEnum.Panel2)]
        public SplitterPanelEnum CollpasePanel
        {
            get
            {
                return mCollpasePanel;
            }
            set
            {
                if (value != mCollpasePanel)
                {
                    mCollpasePanel = value;
                    this.Invalidate(this.ControlRect);
                }
            }
        }

        bool mCollpased = false;
        /// <summary>
        /// 是否为折叠状态
        /// </summary>
        public bool IsCollpased
        {
            get { return mCollpased; }
        }

        Rectangle mRect = new Rectangle();
        /// <summary>
        /// 控制器绘制区域
        /// </summary>
        private Rectangle ControlRect
        {
            get
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    mRect.X = this.Width <= 80 ? 0 : this.Width / 2 - 40;
                    mRect.Y = this.SplitterDistance;
                    mRect.Width = 80;
                    mRect.Height = 9;
                }
                else
                {
                    mRect.X = this.SplitterDistance;
                    mRect.Y = this.Height <= 80 ? 0 : this.Height / 2 - 40;
                    mRect.Width = 9;
                    mRect.Height = 80;
                }
                return mRect;
            }
        }

        /// <summary>
        /// 鼠标状态
        /// </summary>
        MouseState mMouseState = MouseState.Normal;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //绘制参数
            bool collpase = false;
            if ((this.CollpasePanel == SplitterPanelEnum.Panel1 && mCollpased == false)
                || this.CollpasePanel == SplitterPanelEnum.Panel2 && mCollpased)
            {
                collpase = true;
            }
            Color color = mMouseState == MouseState.Normal ? SystemColors.ButtonShadow : SystemColors.ControlDarkDark;
            //需要绘制的图片
            Bitmap bmp = CreateControlImage(collpase, color);
            //绘制区域
            if (this.Orientation == Orientation.Vertical)
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            }
            //清除绘制区域
            e.Graphics.SetClip(this.SplitterRectangle);   //这里需要注意一点就是需要清除拆分器整个区域，如果仅清除控制按钮区域，则会出现虚线状态
            e.Graphics.Clear(this.BackColor);
            //绘制
            e.Graphics.DrawImage(bmp, this.ControlRect);
        }

        public new bool IsSplitterFixed
        {
            get
            {
                return base.IsSplitterFixed;
            }
            set
            {
                base.IsSplitterFixed = value;
                //此处设计防止运行时更改base.IsSplitterFixed属性时导致mIsSplitterFixed变量判断失效
                if (value && mIsSplitterFixed == false)
                {
                    mIsSplitterFixed = true;
                }
            }
        }

        bool mIsSplitterFixed = true;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //鼠标在控制按钮区域
            if (this.SplitterRectangle.Contains(e.Location))
            {
                if (this.ControlRect.Contains(e.Location))
                {
                    //如果拆分器可移动，则鼠标在控制按钮范围内时临时关闭拆分器
                    if (this.IsSplitterFixed == false)
                    {
                        this.IsSplitterFixed = true;
                        mIsSplitterFixed = false;
                    }
                    this.Cursor = Cursors.Hand;
                    mMouseState = MouseState.Hover;
                    this.Invalidate(this.ControlRect);
                }
                else
                {
                    //如果拆分器为临时关闭，则开启拆分器
                    if (mIsSplitterFixed == false)
                    {
                        this.IsSplitterFixed = false;
                        if (this.Orientation == Orientation.Horizontal)
                        {
                            this.Cursor = Cursors.HSplit;
                        }
                        else
                        {
                            this.Cursor = Cursors.VSplit;
                        }
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                    }
                    mMouseState = MouseState.Normal;
                    this.Invalidate(this.ControlRect);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.Cursor = Cursors.Default;
            mMouseState = MouseState.Normal;
            this.Invalidate(this.ControlRect);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (this.ControlRect.Contains(e.Location))
            {
                CollpaseOrExpand();
            }
            base.OnMouseClick(e);
        }

        int _HeightOrWidth;
        /// <summary>
        /// 折叠或展开
        /// </summary>
        public void CollpaseOrExpand()
        {
            if (mCollpased)
            {
                mCollpased = false;
                this.SplitterDistance = _HeightOrWidth;
            }
            else
            {
                mCollpased = true;
                _HeightOrWidth = this.SplitterDistance;
                if (CollpasePanel == SplitterPanelEnum.Panel1)
                {
                    this.SplitterDistance = 0;
                }
                else
                {
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        this.SplitterDistance = this.Height - 9;
                    }
                    else
                    {
                        this.SplitterDistance = this.Width - 9;
                    }
                }
            }
            this.Invalidate(this.ControlRect); //局部刷新绘制
        }


        /// <summary>
        /// 需要绘制的用于折叠窗口的按钮样式
        /// </summary>
        /// <param name="collapse"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Bitmap CreateControlImage(bool collapse, Color color)
        {
            Bitmap bmp = new Bitmap(80, 9);
            for (int x = 5; x <= 30; x += 5)
            {
                for (int y = 1; y <= 7; y += 3)
                {
                    bmp.SetPixel(x, y, color);
                }
            }
            for (int x = 50; x <= 75; x += 5)
            {
                for (int y = 1; y <= 7; y += 3)
                {
                    bmp.SetPixel(x, y, color);
                }
            }
            //控制小三角底边向上或者向下
            if (collapse)
            {
                int k = 0;
                for (int y = 7; y >= 1; y--)
                {
                    for (int x = 35 + k; x <= 45 - k; x++)
                    {
                        bmp.SetPixel(x, y, color);
                    }
                    k++;
                }
            }
            else
            {
                int k = 0;
                for (int y = 1; y <= 7; y++)
                {
                    for (int x = 35 + k; x <= 45 - k; x++)
                    {
                        bmp.SetPixel(x, y, color);
                    }
                    k++;
                }
            }
            return bmp;
        }
    }
}