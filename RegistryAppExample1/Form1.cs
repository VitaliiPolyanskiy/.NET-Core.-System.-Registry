using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegistryAppExample1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // сохранение данных в реестр
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                const string userRoot = "HKEY_CURRENT_USER";// корневой раздел реестра
                const string subkey = "Software\\RegistrySetValueExample"; // подраздел реестра для данных нашей программы
                const string keyName = userRoot + "\\" + subkey;

                // SetValue - устанавливает заданное значение пары "имя-значение" для заданного раздела реестра. 
                // Если заданного раздела не существует, он будет создан.
                Registry.SetValue(keyName/*Полный путь к разделу реестра*/, ""/*имя*/, 5280/*значение*/);
                Registry.SetValue(keyName/*Полный путь к разделу реестра*/, "TestLong"/*имя*/, 12345678901234/*значение*/,
                    RegistryValueKind.QWord/*Тип данных реестра, используемый при сохранении данных*/);

                /*
                    String          Задает строку, заканчивающуюся нулем. Это значение эквивалентно типу данных реестра REG_SZ интерфейса Win32 API. 
                    ExpandString    Задает заканчивающуюся нулем строку, содержащую нерасширенные ссылки на переменные среды, такие как %PATH%, 
                                    которые расширяются при возвращении значения. Это значение эквивалентно типу данных реестра REG_EXPAND_SZ интерфейса Win32 API. 
                    Binary          Задает  двоичные данные в любой форме. Это значение эквивалентно типу данных реестра REG_BINARY интерфейса Win32 API. 
                    DWord           Задает 32-разрядное двоичное число. Это значение эквивалентно типу данных реестра REG_DWORD интерфейса Win32 API. 
                    MultiString     Задает массив заканчивающихся нулем строк, завершаемый двумя нулевыми символами. 
                                    Это значение эквивалентно типу данных реестра REG_MULTI_SZ интерфейса Win32 API. 
                    QWord           Задает 64-разрядное двоичное число. Это значение эквивалентно типу данных реестра REG_QWORD интерфейса Win32 API. 
                */

                // при возвращении значения ссылка на переменную среды %PATH% расширяться не будет
                Registry.SetValue(keyName/*Полный путь к разделу реестра*/, "BadTest"/*имя*/, "My path: %path%"/*значение*/);

                // при возвращении значения ссылка на переменную среды %PATH% будет расширена (вместо ссылки будут пути)
                Registry.SetValue(keyName/*Полный путь к разделу реестра*/, "GoodTest"/*имя*/, "My path: %path%"/*значение*/,
                    RegistryValueKind.ExpandString/*Тип данных реестра, используемый при сохранении данных*/);

                string[] strings = { "One", "Two", "Three" };
                // сохраняем в реестр массив строк
                Registry.SetValue(keyName/*Полный путь к разделу реестра*/, "TestArray"/*имя*/, strings/*значение*/);
                MessageBox.Show("Данные сохранены в реестр!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // загрузка данных из реестра
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                const string userRoot = "HKEY_CURRENT_USER";// корневой раздел реестра
                const string subkey = "Software\\RegistrySetValueExample"; // подраздел реестра для данных нашей программы
                const string keyName = userRoot + "\\" + subkey;

                // GetValue - возвращает значение, связанное с заданным именем, в заданном разделе реестра. 
                //Если имя не найдено в заданном разделе, возвращает предоставленное значение по умолчанию, или null, если заданный раздел не существует. 
                string noSuch = (string)Registry.GetValue(keyName/*Полный путь к разделу реестра*/, "NoSuchName"/*имя*/,
                    "Return this default if NoSuchName does not exist."/*Возвращаемое значение, если имя не существует*/);
                MessageBox.Show("NoSuchName: " + noSuch);

                int tInteger = (int)Registry.GetValue(keyName/*Полный путь к разделу реестра*/, ""/*имя*/, -1/*Возвращаемое значение, если имя не существует*/);
                string default_value = String.Format("(Default): {0}", tInteger);
                MessageBox.Show(default_value);

                long tLong = (long)Registry.GetValue(keyName/*Полный путь к разделу реестра*/, "TestLong"/*имя*/, long.MinValue/*Возвращаемое значение, если имя не существует*/);
                string testLong = String.Format("TestLong: {0}", tLong);
                MessageBox.Show(testLong);

                string[] tArray = (string[])Registry.GetValue(keyName/*Полный путь к разделу реестра*/,
                    "TestArray"/*имя*/,
                    new string[] { "Default if TestArray does not exist." }/*Возвращаемое значение, если имя не существует*/);
                for (int i = 0; i < tArray.Length; i++)
                {
                    string str = String.Format("TestArray({0}): {1}", i, tArray[i]);
                    MessageBox.Show(str);
                }

                string tExpand = (string)Registry.GetValue(keyName/*Полный путь к разделу реестра*/,
                     "BadTest"/*имя*/,
                     "Default if TestExpand does not exist."/*Возвращаемое значение, если имя не существует*/);
                MessageBox.Show("TestExpand: " + tExpand);

                string tExpand2 = (string)Registry.GetValue(keyName/*Полный путь к разделу реестра*/,
                    "GoodTest"/*имя*/,
                    "Default if TestExpand2 does not exist."/*Возвращаемое значение, если имя не существует*/);
                string[] arstr = tExpand2.Split(';');
                foreach (string buf in arstr)
                    MessageBox.Show(buf);
                MessageBox.Show("Данные загружены из реестра!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // удаление значения из реестра
        private void button3_Click(object sender, EventArgs e)
        {
            RegistryKey myKey;
            try
            {
                //OpenSubKey - возвращает заданный вложенный раздел.
                myKey = Registry.CurrentUser.OpenSubKey("Software\\RegistrySetValueExample"/*Имя или путь для открываемого вложенного раздела*/,
                                true/*Если для раздела необходим доступ на запись*/);
                //Удаляет заданное значение из этого раздела.
                myKey.DeleteValue("TestLong");
                MessageBox.Show("Из реестра удалено значение, соответствующее имени TestLong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // удаление подраздела реестра
        private void button4_Click(object sender, EventArgs e)
        {
            const string subkey = "Software\\RegistrySetValueExample";// подраздел реестра для данных нашей программы
            try
            {
                //Удаление заданного вложенного раздела. Строка subkey не учитывает регистр знаков.
                Registry.CurrentUser.DeleteSubKey(subkey, true);
                MessageBox.Show("Из реестра удален подраздел RegistrySetValueExample");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
