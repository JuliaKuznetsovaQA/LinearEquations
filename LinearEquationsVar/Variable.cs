using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LinearEquationsVar
{
    public class Variable
    {
        private String textBoxValue;
        private String name;
        private int line;
        private int column;
        private Double value;

        // Конструктор
        public Variable(string textBoxValue, string name, int line, int column)
        {
            this.name = name;
            this.line = line;
            this.column = column;
            this.textBoxValue = textBoxValue;

            {
                if (textBoxValue == "")
                {
                    this.value = 0;
                }

                else
                {

                    try
                    {
                        this.value = Convert.ToDouble(textBoxValue);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Разрешен ввод только чисел");
                        MainWindow.flagIncompatible = true;
                        return;
                    }

                }

            }

        }

        public string Name { get { return name; } }

        public int Line
        {
            set => this.line = value;
            get => this.line;
        }

        public int Column
        {
            set => this.column = value;
            get => this.column;
        }

        public double Value
        {
            set
            {
                if (textBoxValue == "")
                {
                    this.value = 0;
                }

                else
                {
                    this.value = Convert.ToDouble(textBoxValue);

                }

            }
            get => this.value;
        }



        public void SetValue(double value)
        {
            this.value = value;
        }
    }
}