using System.Text;
using System.Linq;

namespace DataBaseProvider
{
    public class Repository
    {

        #region Поля
        // Путь к файлу
        private readonly string DB_PATH = @"Handbook.txt";
        // Массив сотрудников
        private Worker[]? _allWorkers;
        //индекс массива сотрудников
        private int _index;
        //заголовок файла
        private string[] _title = new string[]
        {
            "ID", "Время записи", "Ф.И.О.","Возраст","Рост",
            "Дата рождения", "Место рождения"
        };
        #endregion

        #region Свойства
        public string Title 
        { 
            get { return $"{this._title[0],3} " +
                         $"{this._title[1],20} " +
                         $"{this._title[2],25} " +
                         $"{this._title[3],7} " +
                         $"{this._title[4],4} " +
                         $"{this._title[5],13} " +
                         $"{this._title[6],15}"; } 
        }

        #endregion

        #region Конструктор
        /// <summary>
        /// Хранилище данных
        /// </summary>
        public Repository()
        {
            if (!File.Exists(DB_PATH))
            {
                CreateDB();
            }
            _allWorkers = new Worker[0];
            _index = 0;
            ReadDataFromFile();
        }
        #endregion

        /// <summary>
        /// Создание файла если его нет
        /// </summary>
        private void CreateDB()
        {
            StringBuilder title = new StringBuilder().AppendJoin('#', _title);
            using(StreamWriter sr =new StreamWriter(DB_PATH, true)) 
            {
                    sr.WriteLine(title.ToString());
            }
        }
        /// <summary>
        /// Чтение данных из файла и сохранение в репозитории
        /// </summary>
        private void ReadDataFromFile()
        {
            using (StreamReader sr = new StreamReader(DB_PATH))
            {
                _title = sr.ReadLine().Split('#');
                string readFromFile;
                while ( (readFromFile = sr.ReadLine()) != null )
                {
                    string[] readWorker = readFromFile.Split('#');
                    Worker workerToAdd = new Worker(
                        Convert.ToInt32(readWorker[0]),
                        Convert.ToDateTime(readWorker[1]),
                        readWorker[2],
                        Convert.ToInt32(readWorker[3]),
                        Convert.ToInt32(readWorker[4]),
                        Convert.ToDateTime(readWorker[5]),
                        readWorker[6]
                        );
                    AddWorkerToArray(workerToAdd);
                }
                
            }

        }

        /// <summary>
        /// Добавить в массив новое значение
        /// </summary>
        /// <param name="valueToAdd">Сотрудник для добавления</param>
        private void AddWorkerToArray(Worker valueToAdd)
        {
            if (this._index >= this._allWorkers.Length)
            {
                Array.Resize(ref this._allWorkers, this._allWorkers.Length + 1);
                this._allWorkers[this._index] = valueToAdd;
                this._index++;
            }
            else
            {
                this._allWorkers[this._index] = valueToAdd;
                this._index++;
            }

        }

        /// <summary>
        /// Добавление нового сотрудника в справочник
        /// </summary>
        /// <param name="newWorker">Сведения о новом сотруднике</param>
        public void AddNewWorker(Worker newWorker)
        {
            AddWorkerToArray(newWorker);
            WriteDataToFile();
        }


        /// <summary>
        /// Запись всей информации в файл
        /// </summary>
        private void WriteDataToFile()
        {
            File.Delete(DB_PATH);
            CreateDB();

            using (StreamWriter sw = new StreamWriter(DB_PATH, true))
            {
                for (int indexWorker = 0; indexWorker < this._index; indexWorker++)
                {
                    StringBuilder sb = new StringBuilder().AppendJoin('#',
                                        this._allWorkers[indexWorker].Id.ToString(),
                                        this._allWorkers[indexWorker].RecordDate.ToString(),
                                        this._allWorkers[indexWorker].FullName,
                                        this._allWorkers[indexWorker].Age.ToString(),
                                        this._allWorkers[indexWorker].Height.ToString(),
                                        this._allWorkers[indexWorker].Birthday.ToString(),
                                        this._allWorkers[indexWorker].BirthdayPlace);
                    sw.WriteLine(sb.ToString());
                }
            }

        }

        /// <summary>
        /// Получает следующий сводобный ID из БД 
        /// </summary>
        /// <returns>Следующий ID</returns>
        public int GetNextId()
        {
            if (this._index != 0)
            {
                return this._allWorkers[this._index - 1].Id + 1;
            }
            else
            {
                return this._index + 1;
            }
        }

        /// <summary>
        /// Удаление записи из БД
        /// </summary>
        /// <param name="valueID">Номер ID для удаления</param>
        /// <returns>True если запись удалена, false если записи с таким ID не существует</returns>
        public bool DeleteWorkerRecord(int valueID)
        {
            bool isValidID = this._allWorkers.Any(w => w.Id == valueID );
                        
            if (isValidID)
            {
                
                //ищем индекс удаляемого элемента
                int indexToDelete =-1;
                for (int i = 0; i < this._index; i++)
                {
                    if (this._allWorkers[i].Id == valueID)
                    {
                        indexToDelete = i;
                        break;
                    }
                }

                Worker[] newAllworkers = new Worker[this._index - 1];

                //"удаляем" элемент из массива
                for (int i = 0; i < indexToDelete; i++)
                {
                    newAllworkers[i] = this._allWorkers[i];
                }
                for (int i = indexToDelete + 1; i < this._allWorkers.Length; i++)
                {
                    newAllworkers[i - 1] = this._allWorkers[i];
                }

                this._allWorkers = newAllworkers;
                this._index--;
                WriteDataToFile();
                return true;

            }
            else
            {
                return false;
            }
                        
        }

        /// <summary>
        /// Получить список сотрудников
        /// </summary>
        /// <param name="header">Заголовочная часть списка</param>
        /// <returns>Список всех сотрудников</returns>
        public Worker[] GetAllWorkers(out string header)
        {
            header = this.Title;
            return this._allWorkers;
        }
    }
}