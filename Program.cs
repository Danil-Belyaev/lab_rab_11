using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace lr11
{
    class Program
    { 
    enum CategoryType
    {
        A,
        B,
        C,
        D,
        E
    }

    class Person
    {
        string ID { get; set; }
        public string Name { get; set; }
        public CategoryType Category { get; set; }
        public float Salary { get; set; }
        public int Product { get; set; }
        public double Price1 { get; set; }

        public static Person Create(String str)
        {
            Person p = new Person();
            string[] e = str.Split(',');
            p.ID = e[0].Trim();
            p.Name = e[1].Trim();
            string tmp = e[2].Trim();
            if (tmp == "A")
                p.Category = CategoryType.A;
            if (tmp == "B")
                p.Category = CategoryType.B;
            if (tmp == "C")
                p.Category = CategoryType.C;
            if (tmp == "D")
                p.Category = CategoryType.D;
            if (tmp == "E")
                p.Category = CategoryType.E;
            else
                tmp = "NONE";
            p.Salary = Convert.ToSingle(e[3].TrimStart('$').Replace('.', ','));
            p.Product = Convert.ToInt32(e[4].Trim());
            p.Price1 = Convert.ToSingle(e[5].TrimStart('$').Replace('.', ','));

            return p;
        }

        public override string ToString()
        {
            String s = string.Format(
                "********************************************************\n" + 
                "ID: {0}, Имя: {1}, ({2})\n" +
                "Зар/плата: {3}, Произведено продукции: {4}\n" + 
                "Цена одного товара производимой продукции: {5}", ID, Name, CategoryToStr(Category), Salary, 
                Product, Price1);
            return s;
        }

        public static String CategoryToStr(CategoryType c)
        {
            if (c == CategoryType.A) return "Категория А";
            if (c == CategoryType.B) return "Категория B";
            if (c == CategoryType.C) return "Категория C";
            if (c == CategoryType.D) return "Категория D";
            if (c == CategoryType.E) return "Без категории";
            else return "0";
        }
    }

        static void Main(string[] args)
        {
            StreamReader f_in = new StreamReader("lr11_27.csv");
#if !DEBUG
            TextWriter save_out = Console.Out;
            var new_out = new StreamWriter(@"lr11_output.txt");
            Console.SetOut(new_out);
#endif
            List<Person> all = new List<Person>();
            try
            {
                String line = f_in.ReadLine();
                while ((line = f_in.ReadLine()) != null)
                {
                    all.Add(Person.Create(line));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Всего пользователей: {0}", all.Count);


            Console.WriteLine("\n******** Задача 1 ***********");
            double ZPmenshe = (from p in all where (p.Salary < (p.Product * p.Price1)) select p.Name).Count();
            Console.WriteLine("Количество рабочих, которые получают меньше, чем вырабатывают продукции: {0}", ZPmenshe);
            Console.WriteLine();

            Console.WriteLine("\n******** Задача 2 ***********");
            int perCountE = all.FindAll(p => p.Category == CategoryType.E).ToList().Count;
            Console.WriteLine("Колличество товаров без категории: {0}", perCountE);
            Console.WriteLine();

            Console.WriteLine("\n******** Задача 3 ***********");
            float TypeA = all.FindAll(p => p.Category == CategoryType.A).ToList().Count;
            float TypeB = all.FindAll(p => p.Category == CategoryType.B).ToList().Count;
            float TypeC = all.FindAll(p => p.Category == CategoryType.C).ToList().Count;
            float TypeD = all.FindAll(p => p.Category == CategoryType.D).ToList().Count;
            float TypeE = all.FindAll(p => p.Category == CategoryType.E).ToList().Count;

            double VolumeA = (from p in all
                             where (p.Category == CategoryType.A)
                             select p.Price1 * p.Product).Sum();
            Console.WriteLine("Суммарный объем (в валюте) произведенной продукции по категории А: {0:0.000}", VolumeA);
            double VolumeB = (from p in all
                              where (p.Category == CategoryType.B)
                              select p.Price1 * p.Product).Sum();
            Console.WriteLine("Суммарный объем (в валюте) произведенной продукции по категории B: {0:0.000}", VolumeB);
            double VolumeC = (from p in all
                              where (p.Category == CategoryType.C)
                              select p.Price1 * p.Product).Sum();
            Console.WriteLine("Суммарный объем (в валюте) произведенной продукции по категории C: {0:0.000}", VolumeC);
            double VolumeD = (from p in all
                              where (p.Category == CategoryType.D)
                              select p.Price1 * p.Product).Sum();
            Console.WriteLine("Суммарный объем (в валюте) произведенной продукции по категории D: {0:0.000}", VolumeD);
            double VolumeE = (from p in all
                              where (p.Category == CategoryType.E)
                              select p.Price1 * p.Product).Sum();
            Console.WriteLine("Суммарный объем (в валюте) произведенной продукции без категорий: {0:0.000}", VolumeE);
            Console.WriteLine();

            Console.WriteLine("\n******** Задача 4 ***********");
            double ZPbolshe = (from p in all where (p.Salary > ((p.Product * p.Price1) / 2)) select p.Name).Count();
            Console.WriteLine("Количество сотрудников, получающих более 50% от суммы производимого продукта: {0}", ZPbolshe);

#if !DEBUG
            Console.SetOut(save_out);
            new_out.Close();
#else
            Console.ReadKey();
#endif
        }
    }
}
