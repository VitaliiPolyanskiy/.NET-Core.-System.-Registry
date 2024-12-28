using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace RegistryAppExample2
{
    public partial class Form1 : Form
    {
        List<string> names = new ();
        public Form1()
        {
            InitializeComponent();

            
            // инициализация объекта
            names.Add("one");
            names.Add("two");
            names.Add("three");
        }

        // Сохранение данных в реестр
        private void button1_Click(object sender, EventArgs e)
        {
            Save(names/*сохраняемые данные*/, "software\\test"/*подраздел реестра*/, "ValueName"/*имя*/);
            MessageBox.Show("Данные успешно сохранены в реестр!");
        }

        // Загрузка данных из реестра
        private void button2_Click(object sender, EventArgs e)
        {
            // Загружаем сохраненный объект
            List<string> obj = null;
            try
            {
                Load(ref obj/*загружаемые данные*/, "software\\test"/*подраздел реестра*/, "ValueName"/*имя*/);
                foreach (string str in obj)
                    MessageBox.Show(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Save(List<string> obj, string akey, string avalue)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            MemoryStream stream = new MemoryStream();
            // Сериализация в поток
            serializer.Serialize(stream, obj);
            // Описание ключа реестра
            RegistryKey regKey;
            // Открытие ключа реестра
            regKey = Registry.CurrentUser.CreateSubKey(akey);
            // Запись  в реестр
            regKey.SetValue(avalue, stream.ToArray());
            stream.Close();
        }

        public void Load(ref List<string> obj, string akey, string avalue)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            // Описание ключа реестра
            RegistryKey regKey;
            // Открытие ключа реестра
            regKey = Registry.CurrentUser.OpenSubKey(akey);
            // Чтение из реестра в байтовый массив
            byte[] barray = null;
            barray = (byte[])regKey.GetValue(avalue);
            if (barray != null)
            {
                // Создание бинарного потока
                MemoryStream stream = new MemoryStream(barray);
                // десериализация из потока в List
                obj = serializer.Deserialize(stream) as List<string>;
                stream.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            const string subkey = "software\\test";// подраздел реестра для данных нашей программы
            try
            {
                //Удаление заданного вложенного раздела. Строка subkey не учитывает регистр знаков.
                Registry.CurrentUser.DeleteSubKey(subkey, true);
                MessageBox.Show("Из реестра удален подраздел test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
