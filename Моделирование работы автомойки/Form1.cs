using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Моделирование_работы_автомойки
{
	public partial class Form1 : Form
	{
		//Какие автомобили в каких местах
		private Автомобиль автомобиль1;
		private Автомобиль автомобиль2;
		private Автомобиль автомобиль3;
		bool isStart = false;
		enum СостоянияМест
		{
			пусто = 0,
			занято = 1,
			перерыв = 2
		}
		СостоянияМест СостоянияМесто1 = 0;
		СостоянияМест СостоянияМесто2 = 0;
		СостоянияМест СостоянияМесто3 = 0;
		int Выручка = 0;
		readonly string Валюта = " у.е.";
		public Form1()
		{
			InitializeComponent();
		}

		private void Сообщить(string text)
		{
			EventsText.Text = text + Environment.NewLine + EventsText.Text;
		}

		private void Button2_Click(object sender, EventArgs e)
		{
			ДобавитьАвтомобильВОчередь();
		}

		private void TimerAddMashine_Tick(object sender, EventArgs e)
		{
			ДобавитьАвтомобильВОчередь();
		}

		/// <summary>
		/// Генератор автомобиля
		/// Легковой автомобиль - 50% шанс
		/// Джип - 35% шанс
		/// Микроавтобус - 15% шанс
		/// </summary>
		/// <returns></returns>
		private void ДобавитьАвтомобильВОчередь()
		{
			Автомобиль НовыйАвтомобиль;
			Random rnd = new Random(DateTime.Now.Millisecond);
			int grnd = rnd.Next(0, 100);
			if (grnd < 50)
				НовыйАвтомобиль = new ЛегковойАвтомобиль();
			else
			{
				if (grnd > 85)
					НовыйАвтомобиль = new Микроавтобус();
				else
					НовыйАвтомобиль = new Джип();
			}
			listBox1.Items.Add(НовыйАвтомобиль.Название);
			Сообщить("В очереди новый автомобиль: " + НовыйАвтомобиль.Название);
		}

		private Автомобиль ВернутьАвтомобильПоНазванию(string Название)
		{
			switch (Название)
			{
				case "Легковой автомобиль": return new ЛегковойАвтомобиль();
				case "Джип": return new Джип();
				case "Микроавтобус": return new Микроавтобус();
				default: return new Автомобиль();
			}
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			isStart = !isStart;
			if(isStart)
			{
				timerAddMashine.Start();
				Сообщить("Начало работы автомойки");
				for (int i = 0; i < 4; i++)
					ДобавитьАвтомобильВОчередь();
				button1.Text = "Закончить";
				button1.FlatAppearance.BorderColor = Color.Red;
			}
			else
			{
				timerAddMashine.Stop();
				Сообщить("Конец работы мойки");
				button1.Text = "Начать";
				button1.FlatAppearance.BorderColor = Color.Lime;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			timer1.Start();
			timer2.Start();
			timer3.Start();
		}

		private void Timer1_Tick(object sender, EventArgs e)
		{
			//добавление автомобиля
			if (listBox1.Items.Count > 0 && СостоянияМесто1 == СостоянияМест.пусто)
			{
				автомобиль1 = ВернутьАвтомобильПоНазванию(listBox1.Items[0].ToString());
				listBox1.Items.RemoveAt(0);
				labelТипАвто1.Text = автомобиль1.Название;
				СостоянияМесто1 = СостоянияМест.занято;
				labelСостояние1.Text = "Состояние: занято";
				progressBar1.Maximum = автомобиль1.ВремяМойки;
				Сообщить("В место №1 приехал автомобиль: " + автомобиль1.Название);
			}
			//Проверка мойки 
			if (СостоянияМесто1 == СостоянияМест.занято)
				progressBar1.Value += progressBar1.Value <= автомобиль1.ВремяМойки - 100 ? 100 : 0;
			//Проверка окончания
			if (progressBar1.Value == автомобиль1?.ВремяМойки)
			{
				Выручка += автомобиль1.Плата;
				textBox1.Text = Выручка.ToString() + Валюта;
				СостоянияМесто1 = СостоянияМест.перерыв;
				labelСостояние1.Text = "Состояние: перерыв";
				progressBar1.Value = 0;
				progressBar1.Maximum = 5000;
				Сообщить("Место №1 помыло: " + автомобиль1.Название);
				Сообщить("   + " + автомобиль1.Плата + Валюта);
				автомобиль1 = null;
				labelТипАвто1.Text = "Пусто";
			}
			//Проверка перерыва
			if (СостоянияМесто1 == СостоянияМест.перерыв)
			{
				progressBar1.Value += progressBar1.Value <= 5000 - 100 ? 100 : 0;
				if (progressBar1.Value == 5000)
				{
					progressBar1.Value = 0;
					СостоянияМесто1 = СостоянияМест.пусто;
					labelСостояние1.Text = "Состояние: пусто";
					Сообщить("Место №1 закончило перерыв");
				}
			}
		}

		private void Timer2_Tick(object sender, EventArgs e)
		{
			//добавление автомобиля
			if (listBox1.Items.Count > 0 && СостоянияМесто2 == СостоянияМест.пусто)
			{
				автомобиль2 = ВернутьАвтомобильПоНазванию(listBox1.Items[0].ToString());
				listBox1.Items.RemoveAt(0);
				labelТипАвто2.Text = автомобиль2.Название;
				СостоянияМесто2 = СостоянияМест.занято;
				labelСостояние2.Text = "Состояние: занято";
				progressBar2.Maximum = автомобиль2.ВремяМойки;
				Сообщить("В место №2 приехал автомобиль: " + автомобиль2.Название);
			}
			//Проверка мойки 
			if (СостоянияМесто2 == СостоянияМест.занято)
				progressBar2.Value += progressBar2.Value <= автомобиль2.ВремяМойки - 100 ? 100 : 0;
			//Проверка окончания
			if (progressBar2.Value == автомобиль2?.ВремяМойки)
			{
				Выручка += автомобиль2.Плата;
				textBox1.Text = Выручка.ToString() + Валюта;
				СостоянияМесто2 = СостоянияМест.перерыв;
				labelСостояние2.Text = "Состояние: перерыв";
				progressBar2.Value = 0;
				progressBar2.Maximum = 5000;
				Сообщить("Место №2 помыло: " + автомобиль2.Название);
				Сообщить("   + " + автомобиль2.Плата + Валюта);
				автомобиль2 = null;
				labelТипАвто2.Text = "Пусто";
			}
			//Проверка перерыва
			if (СостоянияМесто2 == СостоянияМест.перерыв)
			{
				progressBar2.Value += progressBar2.Value <= 5000 - 100 ? 100 : 0;
				if (progressBar2.Value == 5000)
				{
					progressBar2.Value = 0;
					СостоянияМесто2 = СостоянияМест.пусто;
					labelСостояние2.Text = "Состояние: пусто";
					Сообщить("Место №2 закончило перерыв");
				}
			}
		}

		private void Timer3_Tick(object sender, EventArgs e)
		{
			//добавление автомобиля
			if (listBox1.Items.Count > 0 && СостоянияМесто3 == СостоянияМест.пусто)
			{
				автомобиль3 = ВернутьАвтомобильПоНазванию(listBox1.Items[0].ToString());
				listBox1.Items.RemoveAt(0);
				labelТипАвто3.Text = автомобиль3.Название;
				СостоянияМесто3 = СостоянияМест.занято;
				labelСостояние3.Text = "Состояние: занято";
				progressBar3.Maximum = автомобиль3.ВремяМойки;
				Сообщить("В место №3 приехал автомобиль: " + автомобиль3.Название);
			}
			//Проверка мойки 
			if (СостоянияМесто3 == СостоянияМест.занято)
				progressBar3.Value += progressBar3.Value <= автомобиль3.ВремяМойки - 100 ? 100 : 0;
			//Проверка окончания
			if (progressBar3.Value == автомобиль3?.ВремяМойки)
			{
				Выручка += автомобиль3.Плата;
				textBox1.Text = Выручка.ToString() + Валюта;
				СостоянияМесто3 = СостоянияМест.перерыв;
				labelСостояние3.Text = "Состояние: перерыв";
				progressBar3.Value = 0;
				progressBar3.Maximum = 5000;
				Сообщить("Место №3 помыло: " + автомобиль3.Название);
				Сообщить("   + " + автомобиль3.Плата + Валюта);
				автомобиль3 = null;
				labelТипАвто3.Text = "Пусто";
			}
			//Проверка перерыва
			if (СостоянияМесто3 == СостоянияМест.перерыв)
			{
				progressBar3.Value += progressBar3.Value <= 5000 - 100 ? 100 : 0;
				if (progressBar3.Value == 5000)
				{
					progressBar3.Value = 0;
					СостоянияМесто3 = СостоянияМест.пусто;
					labelСостояние3.Text = "Состояние: пусто";
					Сообщить("Место №3 закончило перерыв");
				}
			}
		}
	}
}
