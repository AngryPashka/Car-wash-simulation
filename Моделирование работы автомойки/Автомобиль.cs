using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Моделирование_работы_автомойки
{
	class Автомобиль
	{
		public int Плата;
		public int ВремяМойки;
		public string Название;
	}
	class ЛегковойАвтомобиль : Автомобиль
	{
		public ЛегковойАвтомобиль()
		{
			Плата = 100;
			ВремяМойки = 10000;
			Название = "Легковой автомобиль";
		}
	}

	class Джип : Автомобиль
	{
		public Джип()
		{
			Плата = 200;
			ВремяМойки = 15000;
			Название = "Джип";
		}
	}
	class Микроавтобус : Автомобиль
	{
		public Микроавтобус()
		{
			Плата = 500;
			ВремяМойки = 25000;
			Название = "Микроавтобус";
		}
	}
}
