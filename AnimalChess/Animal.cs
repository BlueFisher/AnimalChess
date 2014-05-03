using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace AnimalChess {
	public enum camp {
		red,
		black
	}
	public delegate void setPiecePositionEventHandler(Point position);
	public delegate void pieceMoveEventHandler(int index ,Point position);
	public delegate void pieceBeEatenEventHandler(int index);

	public class Animal {
		public static event setPiecePositionEventHandler setPiecePosition;
		public static event pieceMoveEventHandler pieceMove;
		public static event pieceBeEatenEventHandler pieceBeEaten;
		public Animal(Point position, camp camp, byte level,int index) {
			this.position = position;
			this.iniPosition = position;
			this.camp = camp;
			this.level = level;
			this.index = index;
			setPiecePosition(position);
		}

		private int index;
		public int Index {
			get {
				return index;
			}
		}

		private byte level;
		public byte Level {
			get {
				return level;
			}
		}

		private camp camp;
		public camp Camp {
			get {
				return camp;
			}
		}

		private Point position;
		public Point Position {
			get {
				return position;
			}
		}

		private Point iniPosition;
		public Point IniPosition {
			get {
				return iniPosition;
			}
		}

		public void MoveTo(Point position) {
			this.position = position;
			pieceMove(this.index, this.position);
		}

		private bool isLive = true;
		public bool IsLive {
			get {
				return isLive;
			}
		}
		public void BeEaten() {
			this.isLive = false;
			this.position = new Point(0,0);
			pieceBeEaten(this.index);
		}

		public virtual Point[] Forcast() {
			return new Point[]{
				new Point(this.Position.X-1,this.Position.Y),
				new Point(this.Position.X+1,this.Position.Y),
				new Point(this.Position.X,this.Position.Y-1),
				new Point(this.Position.X,this.Position.Y+1)
			};
		}
	}
}
