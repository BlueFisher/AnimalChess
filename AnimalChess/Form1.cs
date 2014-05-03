using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;


namespace AnimalChess {
	public partial class Form1 : Form {

		#region Create each Animal & static animals
		Elephant[] elephant;
		Lion[] lion;
		Tiger[] tiger;
		Leopard[] leopard;
		Dog[] dog;
		Wolf[] wolf;
		Cat[] cat;
		Mouse[] mouse;

		public static Animal[] animals;
		#endregion

		PictureBox[] picForcast = new PictureBox[4];
		PictureBox[] pieces = new PictureBox[16];
		Bitmap[] b = new Bitmap[16];

		public Form1() {
			InitializeComponent();

			Bitmap sourcePic = Properties.Resources.pieces;
			for (int i = 0; i < 16; i += 2) {
				b[i] = new Bitmap(70, 70);
				b[i + 1] = new Bitmap(70, 70);
				Graphics g1 = Graphics.FromImage(b[i]);
				Graphics g2 = Graphics.FromImage(b[i + 1]);
				g1.DrawImage(sourcePic, new Rectangle(0, 0, 70, 70), new Rectangle(i / 2 * 70, 0, 70, 70), GraphicsUnit.Pixel);
				g2.DrawImage(sourcePic, new Rectangle(0, 0, 70, 70), new Rectangle(i / 2 * 70, 70, 70, 70), GraphicsUnit.Pixel);
				g1.Dispose();
				g2.Dispose();
			}

			Animal.setPiecePosition += onDefinePieces;
			Animal.pieceMove += onPieceMove;
			Animal.pieceBeEaten += onPieceBeEaten;

			#region Create picForcast
			for (int i = 0; i < picForcast.Length; i++) {
				picForcast[i] = new PictureBox();
				picForcast[i].Size = new Size(70,70);
				picForcast[i].BackColor = Color.FromArgb(100, Color.Blue);
				picForcast[i].Visible = false;
				picForcast[i].Cursor = Cursors.Hand;
				picForcast[i].Click += picForcast_Click;
			}
			pic.Controls.AddRange(picForcast);
			#endregion

			#region Instantiate each Animal
			elephant = new Elephant[]{
				new Elephant(new Point(1,7),camp.red,0),
				new Elephant(new Point(7,3),camp.black,1)
			};
			lion = new Lion[]{
				new Lion(new Point(7,9),camp.red,2),
				new Lion(new Point(1,1),camp.black,3)
			};
			tiger = new Tiger[]{
				new Tiger(new Point(1,9),camp.red,4),
				new Tiger(new Point(7,1),camp.black,5)
			};
			leopard = new Leopard[]{
				new Leopard(new Point(5,7),camp.red,6),
				new Leopard(new Point(3,3),camp.black,7)
			};
			dog = new Dog[]{
				new Dog(new Point(6,8),camp.red,8),
				new Dog(new Point(2,2),camp.black,9)
			};
			wolf = new Wolf[]{
				new Wolf(new Point(3,7),camp.red,10),
				new Wolf(new Point(5,3),camp.black,11)
			};
			cat = new Cat[]{
				new Cat(new Point(2,8),camp.red,12),
				new Cat(new Point(6,2),camp.black,13)
			};
			mouse = new Mouse[]{
				new Mouse(new Point(7,7),camp.red,14),
				new Mouse(new Point(1,3),camp.black,15)
			};
			#endregion
			#region Instantiate static animals
			animals = new Animal[16]{
				elephant[0],
				elephant[1],
				lion[0],
				lion[1],
				tiger[0],
				tiger[1],
				leopard[0],
				leopard[1],
				dog[0],
				dog[1],
				wolf[0],
				wolf[1],
				cat[0],
				cat[1],
				mouse[0],
				mouse[1],
			};
			#endregion

			for (int i = 1; i < 16; i += 2) {
				pieces[i].Enabled = false;
			}
		}

		#region Create pieces through EVENT
		int pieceIndexTemp = 0;
		
		
		void onDefinePieces(Point position) {
			pieces[pieceIndexTemp] = new PictureBox();
			pieces[pieceIndexTemp].Size = new Size(70, 70);
			pieces[pieceIndexTemp].Tag = pieceIndexTemp; //To connect pieces' index with animals' index
			pieces[pieceIndexTemp].Location = format(position);
			pieces[pieceIndexTemp].BackColor = Color.Transparent;
			pieces[pieceIndexTemp].BackgroundImage = b[pieceIndexTemp];
			pieces[pieceIndexTemp].Cursor = Cursors.Hand;
			pieces[pieceIndexTemp].Click += pieces_Click;
			pic.Controls.Add(pieces[pieceIndexTemp++]);
		}
		#endregion

		int pieceClick;
		void pieces_Click(object sender, EventArgs e) {
			int index = (int)(((PictureBox)sender).Tag);
			pieceClick = index;
			Point[] p = animals[index].Forcast();
			for (int i = 0; i < 4; i++) {
				picForcast[i].Location = format(p[i]);
				picForcast[i].Visible = true;
			}
		}
		void piecesRefresh(){
			for (int i = 0; i < 16; i++) {
				pieces[i].Visible = true;
				animals[i].MoveTo(animals[i].IniPosition);
			}
			for (int i = 0; i < 4; i++) {
				picForcast[i].Visible = false;
			}
			currentCamp = camp.red;
			for (int i = 1; i < 16; i += 2) {
				pieces[i-1].Enabled = true;
				pieces[i].Enabled = false;
			}
		}

		Point redBase = new Point(4, 9);
		Point blackBase = new Point(4, 1);
		camp currentCamp = camp.red;
		void picForcast_Click(object sender, EventArgs e) {
			foreach (Animal a in animals) {
				if (unFormat(((PictureBox)sender).Location) == a.Position) {
					a.BeEaten();
				}
			}
			animals[pieceClick].MoveTo(unFormat(((PictureBox)sender).Location));
		
			for (int i = 0; i < 4; i++) {
				picForcast[i].Visible = false;
			}
			if (currentCamp == camp.red) {
				for (int i = 0; i < 16; i += 2) {
					pieces[i].Enabled = false;
					pieces[i + 1].Enabled = true;
				}
				currentCamp = camp.black;
			}
			else {
				for (int i = 1; i < 16; i += 2) {
					pieces[i].Enabled = false;
					pieces[i - 1].Enabled = true;
				}
				currentCamp = camp.red;
			}

			//The winning condition
			if (animals[pieceClick].Camp == camp.red && animals[pieceClick].Position == blackBase) {
				MessageBox.Show("Red Wins");
				piecesRefresh();
			}
			else if (animals[pieceClick].Camp == camp.black && animals[pieceClick].Position == redBase) {
				MessageBox.Show("black Wins");
				piecesRefresh();
			}
		}

		private void onPieceMove(int index, Point position) {
			pieces[index].Location = format(position);
		}

		private void onPieceBeEaten(int index) {
			pieces[index].Visible = false;
		}

		private Point format(Point point) {
			point.X = point.X * 75 - 70;
			point.Y = point.Y * 75 - 70;
			return new Point(point.X, point.Y);
		}
		private Point unFormat(Point point) {
			point.X = (point.X + 70) / 75;
			point.Y = (point.Y + 70) / 75;
			return new Point(point.X, point.Y);
		}

		private void button1_Click(object sender, EventArgs e) {
			piecesRefresh();
			
		}


	}
}
