using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseProvider
{
    public struct Worker : IEnumerable<Worker>
    {
        #region Поля
        readonly Worker[] _workers;

        private DateTime _birthday;
        private int _id;

        #endregion

        #region Свойства
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id
        {
            get { return this._id; }
            //в теории ID записи в БД меняться не должен
            private set { this._id = value; }
        }
        public DateTime RecordDate { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public DateTime Birthday
        {
            get { return this._birthday.Date; }
            set { this._birthday = value.Date; }
        }

        public string BirthdayPlace { get; set; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Создание сотрудника
        /// </summary>
        /// <param name="valueId">Идентификатор в БД</param>
        /// <param name="valueRecorDate">Дата и время записи</param>
        /// <param name="valueFIO">ФИО сотрудника</param>
        /// <param name="valueAge">Возраст сотрудника</param>
        /// <param name="valueHeight">Рост сотрудника</param>
        /// <param name="valueBirthday">Дата рождения сотрудника</param>
        /// <param name="valueBirtdayPlace">Место рождения сотрудника</param>
        public Worker(int valueId,
                      DateTime valueRecorDate,
                      string valueFIO,
                      int valueAge,
                      int valueHeight,
                      DateTime valueBirthday,
                      string valueBirtdayPlace)
        {
            this.Id = valueId;
            this.RecordDate = valueRecorDate;
            this.FullName = valueFIO;
            this.Age = valueAge;
            this.Height = valueHeight;
            this.Birthday = valueBirthday;
            this.BirthdayPlace = valueBirtdayPlace;

        }

        #endregion

        /// <summary>
        /// Сведения о сотрдунике в строковом формате
        /// </summary>
        /// <returns>Строка для вывода в консоль</returns>
        public string PrintToString()
        {
            return $"{Id,3} " +
                   $"{RecordDate,20} " +
                   $"{FullName,25} " +
                   $"{Age,7} " +
                   $"{Height,4} " +
                   $"{Birthday.ToString("d"),13} " +
                   $"{BirthdayPlace,15}";
        }

        #region Реализация перебора
        public IEnumerator<Worker> GetEnumerator()
        {
          return new WorkerEnumenator(this._workers);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        #endregion

    }
}
