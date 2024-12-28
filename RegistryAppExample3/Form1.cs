using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegistryAppExample3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         public RegistryKey OpenSubKey( //Возвращает заданный вложенный раздел.
            string name, // Имя или путь для открываемого вложенного раздела. 
            bool writable // Если для раздела необходим доступ на запись, следует задать значение true. 
        )
        
        public void SetValue( // Устанавливает значение пары "имя-значение" в разделе реестра. 
            string name,// Имя сохраняемого значения (имя ключа).
            Object value // Сохраняемые данные. 
        )

         public void DeleteValue(// Удаляет заданное значение из указанного раздела.
                string name // Имя удаляемого значения (имя ключа). 
            )

        */

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                myKey.SetValue("Calculator", "calc.exe");
                MessageBox.Show("Программа успешно добавлена в автозагрузку!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                myKey.DeleteValue("Calculator");
                MessageBox.Show("Программа успешно удалена из автозагрузки!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
