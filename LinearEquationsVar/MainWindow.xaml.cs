using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinearEquationsVar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static double a, b, c;

        static int line_count = 0;
        static int var_count = 0;
        public static bool flagIncompatible = false;


        

        // Очистка формы.
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = {
                textBox_a1, textBox_b1, textBox_c1, textBox_d1,
                textBox_a2, textBox_b2, textBox_c2, textBox_d2,
                textBox_a3, textBox_b3, textBox_c3, textBox_d3,
                textBox_answer};
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
            }
            
            var_count = 0;
            line_count = 0;
            a = 0;
            b = 0;
            c = 0;
            flagIncompatible = false;
        }

       
        public void enter_Click(object sender, RoutedEventArgs e)
        {
            Variable[] vars = new Variable[12];

            TextBox[] textBoxes = {
                textBox_a1, textBox_b1, textBox_c1, textBox_d1,
                textBox_a2, textBox_b2, textBox_c2, textBox_d2,
                textBox_a3, textBox_b3, textBox_c3, textBox_d3};
            foreach (var textBox in textBoxes)
            {
                if (textBox.Text != "")
                {
                    try
                    {
                        Convert.ToDouble(textBox.Text);
                    }
                    catch (Exception)
                    {
                        textBox_answer.Text = "Разрешен ввод только чисел";
                    }
                }
            }

            Variable A1 = new Variable(textBox_a1.Text, "A1", 1, 1);
            vars[0] = A1;
            Variable B1 = new Variable(textBox_b1.Text, "B1", 1, 2);
            vars[1] = B1;
            Variable C1 = new Variable(textBox_c1.Text, "C1", 1, 3);
            vars[2] = C1;
            Variable D1 = new Variable(textBox_d1.Text, "D1", 1, 4);
            vars[3] = D1;
            Variable A2 = new Variable(textBox_a2.Text, "A2", 2, 1);
            vars[4] = A2;
            Variable B2 = new Variable(textBox_b2.Text, "B2", 2, 2);
            vars[5] = B2;
            Variable C2 = new Variable(textBox_c2.Text, "C2", 2, 3);
            vars[6] = C2;
            Variable D2 = new Variable(textBox_d2.Text, "D2", 2, 4);
            vars[7] = D2;
            Variable A3 = new Variable(textBox_a3.Text, "A3", 3, 1);
            vars[8] = A3;
            Variable B3 = new Variable(textBox_b3.Text, "B3", 3, 2);
            vars[9] = B3;
            Variable C3 = new Variable(textBox_c3.Text, "C3", 3, 3);
            vars[10] = C3;
            Variable D3 = new Variable(textBox_d3.Text, "D3", 3, 4);
            vars[11] = D3;


            // (Нужно на этапе отладки, чтобы показать все значения системы в данный момент)
            //TODO 
            void ShowSystem()
            {
                foreach (var item in vars)
                {
                    textBox_answer.Text += "\n" + item.Name + "=" + item.Value;
                }
            }

            //TODO Убрать после отладки
            //textBox_answer.Text += "После ввода данных: \n";
            //ShowSystem();

            // Проверка на строки вида ( 0...0 | d ), где d != 0.
            void IsIncompatible(int line_number)
            {
                int count = 0;
                foreach (var item in vars)
                {
                    if (item.Line == line_number && item.Value == 0)
                    {
                        count++;
                    }
                }
                if (count == 3)
                {
                    foreach (var item1 in vars)
                    {
                        if (item1.Line == line_number && item1.Column == 4 && item1.Value != 0)
                        {
                            textBox_answer.Text = "Система несовместна и не имеет решений.";
                            MessageBox.Show("Система несовместна и не имеет решений.");
                            flagIncompatible = true;
                        }
                    }
                }
            }

            for (int i = 1; i < 4; i++)
                {
                    IsIncompatible(i);
                }


            // Проверка на нулевые строки
            bool IsLineNull(int line_number)
            {
                int count = 0;
                foreach (var item in vars)
                {
                    if (item.Line == line_number && item.Value == 0)
                    {
                        count++;
                    }
                }
                if (count == 4)
                {
                    return true;
                }
                else { return false; }
            }

            // Проверка на нулевые переменные (столбцы)
            bool IsColumnNull(int column_number)
            {
                int count = 0;
                foreach (var item in vars)
                {
                    if (item.Column == column_number && item.Value == 0)
                    {
                        count++;
                    }
                }
                if (count == 3)
                {
                    return true;
                }
                else { return false; }
            }
            
            
            // Функция меняет строки значениями
            void Change_Lines(int line_number1, int line_number2) 
            {
                double tmp;
                foreach (var item in vars)
                {
                    for (int i = 1; i < 5; i++)
                        if (item.Line == line_number1 && item.Column == i)
                        {
                            tmp = item.Value;
                            foreach (var item1 in vars)
                            {
                                if (item1.Line == line_number2 && item1.Column == i)
                                {
                                    item.SetValue(item1.Value);
                                    item1.SetValue(tmp);
                                }
                            }
                        }
                }
            }


            if (flagIncompatible == false)
            {
                // Проверяем на нулевые строки
                if (IsLineNull(1))
                {
                    // Если 1-я строка нулевая, а 3-я нет - меняем строки местами.
                    if (IsLineNull(3) == false)
                    {
                        Change_Lines(1, 3);
                    }
                    // Если и 1-я и 3-я строки нулевые, проверяем 2-ю строку.
                    else if (IsLineNull(3))
                    {
                        // Если 2-я строка ненулевая, меняем местами 1-ю и 2-ю строки.
                        if (IsLineNull(2) == false)
                        {
                            Change_Lines(1, 2);
                        }
                        // Если все 3 строки нулевые, система имеет бесконечное множество решений.
                        else if (IsLineNull(2))
                        {
                            textBox_answer.Text += "Система имеет бесконечное множество решений.";
                            flagIncompatible = true;
                            return;
                        }
                    }
                }
                // Если 1-я строка ненулевая, а 2-я нулевая - поменяем местами 2-ю и 3-ю строки.
                else if (IsLineNull(2))
                {
                    Change_Lines(2, 3);
                }


                //TODO Убрать после отладки
                //textBox_answer.Text += "Строки поменены местами: \n";
                //ShowSystem();

                // Считаем строки:
                line_count = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (IsLineNull(i) == false)
                    {
                        line_count++;
                    }
                }

                // Считаем переменные:
                var_count = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (IsColumnNull(i) == false)
                    {
                        var_count++;
                    }
                }


                //TODO Убрать после отладки
                //textBox_answer.Text += "\n Строк: " + line_count + ", переменных: " + var_count + "\n";

                // Если переменных больше, чем строк, система либо несовместна, либо имеет бесконечно много решений
                if (var_count > line_count)
                {
                    textBox_answer.Text += "строк: " + line_count + ", переменных: " + var_count + ". Система либо несовместна, либо имеет бесконечно много решений.";
                    flagIncompatible = true;
                    return;
                }



                // Если первый коэффициент в 1-й строке равен 0, ищем строку с ненулевым коэффициентом
                // и меняем их местами
                if (A1.Value == 0)
                {
                    if (A2.Value != 0)
                    {
                        Change_Lines(1, 2);
                    }
                    else if (A3.Value != 0)
                    {
                        Change_Lines(1, 3);
                    }
                    // Если все первые коэффициенты нулевые, переходим к анализу вторых коэффициентов.
                    else if (A1.Value == 0 && A2.Value == 0 && A3.Value == 0 && B1.Value == 0)
                    {
                        // Если во 2-й строке второй коэффициент не равен нулю,
                        // меняем 1-ю и 2-ю строку
                        if (B2.Value != 0)
                        {
                            Change_Lines(1, 2);
                        }
                        // Если в первых двух строках первые 2 коэффициента нулевые, а в 3-й строке 
                        // второй коэффициент ненулевой, меняем местами 1-ю и 3-ю строки
                        else if (B3.Value != 0)
                        {
                            Change_Lines(1, 3);
                        }
                    }
                }


                //TODO Убрать после отладки
                //textBox_answer.Text += "До преобразований строк: \n";
                //ShowSystem();

                // Начинаем преобразования строк.
                void Transform_Lines(int line_number1, int line_number2, double x)
                {
                    foreach (var item in vars)
                    {
                        for (int i = 1; i < 5; i++)
                            if (item.Line == line_number1 && item.Column == i)
                            {
                                foreach (var item1 in vars)
                                {
                                    if (item1.Line == line_number2 && item1.Column == i)
                                    {
                                        double tmp = item.Value + (item1.Value * x);
                                        item.SetValue(tmp);
                                    }
                                }
                            }
                    }
                }


                // Начинаем с первого столбца второй строки. Здесь нужно получить 0.
                // Ищем число x, на которое будем домножать 1-ю строку и прибавлять её ко 2-й строке.
                // Преобразуем вторую строку:
                if (A1.Value != 0 && A2.Value != 0)
                {
                    double x = -A2.Value / A1.Value;
                    Transform_Lines(2, 1, x);
                }
                // Переходим к первому столбцу третьей строки.
                if (A1.Value != 0 && A3.Value != 0)
                {
                    double x = -A3.Value / A1.Value;
                    Transform_Lines(3, 1, x);
                }
                // Преобразуем второй столбец третьей строки.
                // Ищем число x, на которое будем домножать 2-ю строку и прибавлять её к 3-й строке.
                // Преобразуем третью строку:
                if (B3.Value != 0 && B2.Value != 0)
                {
                    double x = -B3.Value / B2.Value;
                    Transform_Lines(3, 2, x);
                }

                // Вариант с двумя линейными уравнениями, когда а равны 0
                if (A1.Value == 0 && A2.Value == 0 && A3.Value == 0 && B3.Value == 0 && C3.Value == 0 && D3.Value == 0 && B2.Value != 0 && B1.Value != 0)
                {
                    // Только в этом случае обнуляем b2:
                    // Ищем число x, на которое будем домножать 1-ю строку и прибавлять её к 2-й строке.
                    // Преобразуем вторую строку:
                    double x = -B2.Value / B1.Value;
                    Transform_Lines(2, 1, x);
                }


                //TODO Убрать после отладки
                //textBox_answer.Text += "После преобразований строк: \n";
                //ShowSystem();

                // Исследуем систему линейных уравнений на совместность.
                // Проверяем на строки вида ( 0...0 | d ), где d != 0.
                for (int i = 1; i < 4; i++)
                {
                    IsIncompatible(i);
                }

                // Исследуем систему линейных уравнений на совместность.
                // Считаем строки:
                line_count = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (IsLineNull(i) == false)
                    {
                        line_count++;
                    }
                }

                // Считаем переменные:
                var_count = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (IsColumnNull(i) == false)
                    {
                        var_count++;
                    }
                }


                //TODO Убрать после отладки
                //ShowSystem();
                //textBox_answer.Text += "\n Строк: " + line_count + ", переменных: " + var_count + "\n (До вычисления результата)\n";

                // Если переменных больше, чем строк, система либо несовместна, либо имеет бесконечно много решений
                if (var_count > line_count)
                {
                    textBox_answer.Text += "строк: " + line_count + ", переменных: " + var_count + ". Система либо несовместна, либо имеет бесконечно много решений.";
                    flagIncompatible = true;
                    return;
                }

                if (flagIncompatible == false)
                {
                    // Если одно уравнение:
                    if (line_count == 1)
                    {
                        if (A1.Value != 0)
                        {
                            a = D1.Value / A1.Value;
                            textBox_answer.Text += "a = " + a;
                        }
                        else if (B1.Value != 0)
                        {
                            b = D1.Value / B1.Value;
                            textBox_answer.Text += "b = " + b;
                        }
                        else if (C1.Value != 0)
                        {
                            c = D1.Value / C1.Value;
                            textBox_answer.Text += "c = " + c;
                        }
                    }

                    // Если 2 уравнения:
                    else if (line_count == 2)
                    {
                        foreach (var item in vars)
                        {
                            textBox_answer.Text += "\n" + item.Name + " = " + item.Value;
                        }
                    }



                    // Если 3 уравнения:
                    else if (line_count == 3)
                    {
                        // Вычисляем c:
                        if (C3.Value != 0)
                        {
                            c = D3.Value / C3.Value;
                        }
                        else
                        {
                            c = 0;
                        }

                        // Подставляем c во вторую строку и находим b:
                        if (B2.Value != 0)
                        {
                            b = (D2.Value - (C2.Value * c)) / B2.Value;
                        }


                        // Подставляем b и c в первую строку и находим a:
                        if (A1.Value != 0)
                        {
                            a = (D1.Value - B1.Value * b - C1.Value * c) / A1.Value;
                        }


                        // Возвращаем результат:
                        textBox_answer.Text += "a = " + a + ", b = " + b + ", c = " + c;
                    }
                }
            }            
        }
    }
}
