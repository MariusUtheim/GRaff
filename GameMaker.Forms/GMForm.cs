using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMaker.Forms
{
	internal partial class GMForm : Form
	{
		internal GMForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint	| ControlStyles.OptimizedDoubleBuffer
				, true);
		}
	}
}
