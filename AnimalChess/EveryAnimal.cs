using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace AnimalChess {
	class Elephant : Animal {
		public Elephant(Point position, camp camp,int index)
			: base(position, camp, 8, index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastUnableToCrossRiver(this);
			return tempP;
		}
	}
	class Lion : Animal {
		public Lion(Point position, camp camp,int index)
			: base(position, camp, 7, index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastAbleCrossRiver(this);
			return tempP;
		}
	}
	class Tiger : Animal {
		public Tiger(Point position, camp camp,int index)
			: base(position, camp, 6,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastAbleCrossRiver(this);
			return tempP;
		}
	}
	class Leopard : Animal {
		public Leopard(Point position, camp camp,int index)
			: base(position, camp, 5,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastUnableToCrossRiver(this);
			return tempP;
		}
	}
	class Dog : Animal {
		public Dog(Point position, camp camp,int index)
			: base(position, camp, 4,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastUnableToCrossRiver(this);
			return tempP;
		}
	}
	class Wolf : Animal {
		public Wolf(Point position, camp camp,int index)
			: base(position, camp, 3,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastUnableToCrossRiver(this);
			return tempP;
		}
	}
	class Cat : Animal {
		public Cat(Point position, camp camp,int index)
			: base(position, camp, 2,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastUnableToCrossRiver(this);
			return tempP;
		}
	}
	class Mouse : Animal {
		public Mouse(Point position, camp camp,int index)
			: base(position, camp, 1,index) {
		}
		public override Point[] Forcast() {
			Point[] tempP = ForcastCenter.ForcastAbleSwim(this);
			return tempP;
		}
	}

	class ForcastCenter {
		static Point[] baseCamp = {
			new Point(4,9),
			new Point(4,1)
		};
		private static Point[] ForcastPublic(Animal o){
			Point[] tempP = {
				new Point(o.Position.X,o.Position.Y-1),
				new Point(o.Position.X+1,o.Position.Y),
				new Point(o.Position.X,o.Position.Y+1),
				new Point(o.Position.X-1,o.Position.Y)
			};
			for(int i =0;i<4;i++){
				if(o.Camp == camp.red && tempP[i] == baseCamp[0]){
					tempP[i] = new Point(0,0);
				}
				else if (o.Camp == camp.black && tempP[i] == baseCamp[1]) {
					tempP[i] = new Point(0, 0);
				}
			}
			return tempP;
		}

		public static Point[] ForcastUnableToCrossRiver(Animal o){
			Point[]  tempP = ForcastPublic(o);
			for (int i = 0; i < 4; i++) {
				if (((tempP[i].X >= 2 && tempP[i].X <= 3) || (tempP[i].X >= 5 && tempP[i].X <= 6)) && tempP[i].Y >= 4 && tempP[i].Y <= 6) {
					tempP[i] = new Point(0, 0);
				}
			}
			tempP = ForcastIfPieceExist(tempP, o);
			return tempP;
		}

		public static Point[] ForcastAbleCrossRiver(Animal o) {
			Point[] tempP = ForcastPublic(o);
			for (int i = 0; i < 4; i++) {
				if (((tempP[i].X >= 2 && tempP[i].X <= 3) || (tempP[i].X >= 5 && tempP[i].X <= 6)) && tempP[i].Y >= 4 && tempP[i].Y <= 6) {
					Point[] mouseP = {
						Form1.animals[14].Position,
						Form1.animals[15].Position
					};
					switch (i) {
						case 0:
							for (int x = 0; x < 3; x++) {
								Debug.WriteLine(tempP[i].Y);
								if (tempP[i] == mouseP[0] || tempP[i] == mouseP[1]) {
									tempP[i] = new Point(0,0);
									break;
								}
								tempP[i].Y--;
							}
							break;
						case 1:
							for (int x = 0; x < 2; x++) {
								if (tempP[i] == mouseP[0] || tempP[i] == mouseP[1]) {
									tempP[i] = new Point(0,0);
									break;
								}
								tempP[i].X++;
							}
							break;
						case 2:
							for (int x = 0; x < 3; x++) {
								if (tempP[i] == mouseP[0] || tempP[i] == mouseP[1]) {
									tempP[i] = new Point(0, 0);
									break;
								}
								tempP[i].Y++;
							}
							break;
						case 3:
							for (int x = 0; x < 2; x++) {
								if (tempP[i] == mouseP[0] || tempP[i] == mouseP[1]) {
									tempP[i] = new Point(0, 0);
									break;
								}
								tempP[i].X--;
							}
							break;
					}
				}
			}
			tempP = ForcastIfPieceExist(tempP, o);
			return tempP;
		}

		public static Point[] ForcastAbleSwim(Animal o) {
			Point[] tempP = ForcastPublic(o);
			tempP = ForcastIfPieceExist(tempP, o);
			return tempP;
		}

		#region The condition of whether eat the opponent's piece
		static Point[] redTrap = {
			new Point(3,9),
			new Point(4,8),
			new Point(5,9)
		};
		static Point[] blackTrap = {
			new Point(3,1),
			new Point(4,2),
			new Point(5,1)
		};
		private static Point[] ForcastIfPieceExist(Point[] tempP, Animal o) {
			foreach (Animal a in Form1.animals) {
				for (int i = 0; i < 4; i++) {
					if (tempP[i] == a.Position) {
						if (o.Camp == a.Camp) {
							tempP[i] = new Point(0, 0);
							break;
						}
						else if (o.Camp == camp.red && (a.Position == redTrap[0] || a.Position == redTrap[1] || a.Position == redTrap[2]) && o.Camp != a.Camp) {
							break;
						}
						else if (o.Camp == camp.black && (a.Position == blackTrap[0] || a.Position == blackTrap[1] || a.Position == blackTrap[2]) && o.Camp != a.Camp) {
							break;
						}
						else if (o.Level == 1 && a.Level == 8 && o.Camp != a.Camp) {
							if (((o.Position.X >= 2 && o.Position.X <= 3) || (o.Position.X >= 5 && o.Position.X <= 6)) && o.Position.Y >= 4 && o.Position.Y <= 6) {
								tempP[i] = new Point(0, 0);
								break;
							}
							else {
								break;
							}
						}
						else if ((o.Level < a.Level && o.Camp != a.Camp) || (o.Level == 8 && a.Level == 1 && o.Camp != a.Camp)) {
							tempP[i] = new Point(0, 0);
							break;
						}
						else {
							break;
						}
					}
				}
			}
			return tempP;
		}
		#endregion
	}
}
