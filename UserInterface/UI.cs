using DataBaseProvider;
using System.Runtime.CompilerServices;

namespace UserInterface
{
    /// <summary>
    /// Пользовательский интерфейс работы со справочником
    /// </summary>
    public class UI
    {
        // хранилище данных
        private Repository connectionToDB;

        // конструктор
        public UI()
        {
            connectionToDB = new Repository();
        }

        /// <summary>
        /// Главное меню через которое происходит работа
        /// </summary>
        public void MainMenuStartUp()
        {
            Console.Clear();
            Console.WriteLine("Приложение для ведения списка сотрудников");
            Console.WriteLine("Выберите режим работы: ");
            Console.WriteLine("1. Показать список сотрудников");
            Console.WriteLine("2. Показать сотрудника по ID");
            Console.WriteLine("3. Внести нового сотрудника");
            Console.WriteLine("4. Удалить сотрудника по ID");
            Console.WriteLine("5. Меню сортировки");
            Console.WriteLine("6. Показать записи в диапазоне дней рождения");
            Console.WriteLine("7. Выйти из программы");

            int userChoice;

            do
            {
                Console.WriteLine("Введите номер пункта меню для продолжения:");
                string userInput = Console.ReadLine().ToString();
                int.TryParse(userInput, out userChoice);

            } while (!(0 < userChoice && userChoice <= 7));

            switch (userChoice)
            {
                case 1:
                    ShowAllRecords();
                    break;
                case 2:
                    ShowWorkerByID();
                    break;
                case 3:
                    AddNewRecord();
                    break;
                case 4:
                    DeleteRecord();
                    break;
                case 5:
                    SortingMenu();
                    break;
                case 6:
                    BirthdayRangeToSort();
                    break;
                case 7:
                    ApplicationExit();
                    break;
                default:
                    Console.WriteLine("Что то пошло не так");
                    break;
            }

        }

        /// <summary>
        /// Показ сотрудника по ID
        /// </summary>
        private void ShowWorkerByID()
        {
            string message = "Введите ID сотрудника для отображения";
            int idToShow = ParsingDigitInput(message);

            string title;
            Worker[] allWorkers = this.connectionToDB.GetAllWorkers(out title);

            bool isExist = allWorkers.Any(w => w.Id == idToShow);
            if (isExist)
            {
                IEnumerable<Worker>? workerToshow = from w in allWorkers
                                                    where w.Id == idToShow
                                                    select w;

                PrintWorkers(workerToshow, title);
            }
            else
            {
                Console.WriteLine("Сотрудника с таким ID не существует");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Запрос диапазона двух дат для сортировки списка
        /// </summary>
        private void BirthdayRangeToSort()
        {
            string lowerRequest = "Введите дату рождения с которой начинается отбор: ";
            DateTime lowerBound = ParsingDateInput(lowerRequest);

            string upperRequest = "Введите дату рождения по которую проводится отбор: ";
            DateTime upperBound = ParsingDateInput(upperRequest);

            BirthdayDateSorting(lowerBound, upperBound);
        }

        /// <summary>
        /// Выборка в диапазоне дат
        /// </summary>
        /// <param name="lowerBound">Дата с которой идет отбор</param>
        /// <param name="upperBound">Дата по которую идет отбор</param>
        private void BirthdayDateSorting(DateTime lowerBound, DateTime upperBound)
        {
            string title;
            Worker[] allWorkers = this.connectionToDB.GetAllWorkers(out title);

            var sortedWorkers = from w in allWorkers
                                where w.Birthday >= lowerBound && w.Birthday <= upperBound
                                select w;
            PrintWorkers(sortedWorkers, title);

        }

        /// <summary>
        /// Выход из программы
        /// </summary>
        private void ApplicationExit()
        {
            Environment.Exit(0);
        }


        /// <summary>
        /// Меню выбора сортировки
        /// </summary>
        private void SortingMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите желаемый метод сортировки");
            Console.WriteLine("1. Сортировка по ID сотрудника");
            Console.WriteLine("2. Сортировка по времени записи в БД");
            Console.WriteLine("3. Сортировка по ФИО");
            Console.WriteLine("4. Сортировка по возрасту");
            Console.WriteLine("5. Сортировка по росту");
            Console.WriteLine("6. Сортировка по дате рождения");
            Console.WriteLine("7. Вернуться в главное меню");

            int userChoice;
          
            do
            {
                Console.WriteLine("Введите номер пункта");
                string userInput = Console.ReadLine().ToString();
                int.TryParse(userInput, out userChoice);

            } while ( !(1 <= userChoice && userChoice <= 7));

            SortingRecords(userChoice);
        }

        /// <summary>
        /// Сортировка и вывод в консоль списка
        /// </summary>
        /// <param name="sortingChoice">Тип выбранной сортировки</param>
        private void SortingRecords(int sortingChoice)
        {
            string title;

            Worker[] workersToSort = this.connectionToDB.GetAllWorkers(out title);
            IEnumerable<Worker> sortedWorkers;

            switch (sortingChoice)
            {
                case 1:
                    sortedWorkers = workersToSort.OrderBy(w => w.Id);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 2:
                    sortedWorkers = workersToSort.OrderBy(w => w.RecordDate);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 3:
                    sortedWorkers = workersToSort.OrderBy(w => w.FullName);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 4:
                    sortedWorkers = workersToSort.OrderBy(w => w.Age);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 5:
                    sortedWorkers = workersToSort.OrderBy(w => w.Height);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 6:
                    sortedWorkers = workersToSort.OrderBy(w => w.Birthday);
                    PrintWorkers(sortedWorkers, title);
                    break;
                case 7:
                    return;
                default:
                    Console.WriteLine("Что то пошло не так");
                    return;
            }

        }


        /// <summary>
        /// Вывод в консоль отсортированного списка
        /// </summary>
        /// <param name="sortedWorkers">Список для вывода</param>
        /// <param name="title">Заголовочная часть списка</param>
        private static void PrintWorkers(IEnumerable<Worker> sortedWorkers, string title)
        {
            Console.Clear();
            Console.WriteLine(title);

            foreach (var worker in sortedWorkers)
            {
                Console.WriteLine(worker.PrintToString());
            }

            Console.ReadKey(true);
        }

        /// <summary>
        /// Удаление записи из БД
        /// </summary>
        private void DeleteRecord()
        {
            string message = "Введите ID сотрудника для удаления:";
            int userChoice = ParsingDigitInput(message);
            
            bool isDeleteSuccess = connectionToDB.DeleteWorkerRecord(userChoice);
            
            if (isDeleteSuccess)
            {
                Console.WriteLine("\nЗапись успешно удалена");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("\nЗаписи с таким ID не существует," +
                                    " удаление не произведено");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Проверка введения целого числа
        /// </summary>
        /// <param name="messageToUser">Сообщение пользователю</param>
        /// <returns></returns>
        private static int ParsingDigitInput(string messageToUser)
        {
            int userChoice;
            bool isDigit;
            Console.Clear();
            do
            {
                Console.WriteLine(messageToUser);
                string userInput = Console.ReadLine().ToString();
                isDigit = int.TryParse(userInput, out userChoice);

            } while (!isDigit);
            return userChoice;
        }

        /// <summary>
        /// Вывод списка сотрудников в консоль
        /// </summary>
        private void ShowAllRecords()
        {
            Console.Clear();
            string title;
            Worker[] allWorkers = this.connectionToDB.GetAllWorkers(out title);

            Console.WriteLine(title);
            for (int workerIndex = 0; workerIndex < allWorkers.Length; workerIndex++)
            {
                Console.WriteLine(allWorkers[workerIndex].PrintToString());
            }

            Console.ReadKey(true);
        }

        /// <summary>
        /// Добавление новой записи сотрудника
        /// </summary>
        private void AddNewRecord()
        {
            int id = this.connectionToDB.GetNextId();
            string fullName = InputFullName();
            int age = InputAge();
            int height = InputHeight();
            DateTime birthday = InputBirthday();
            string birthdayPlace = InputBirthdayPlace();
            DateTime recordTime = DateTime.Now;

            Worker newEmployee = new Worker(id,
                                            recordTime,
                                            fullName,
                                            age,
                                            height,
                                            birthday,
                                            birthdayPlace
                                            );

            this.connectionToDB.AddNewWorker(newEmployee);

        }

        /// <summary>
        /// Ввод места рождения сотрудника
        /// </summary>
        /// <returns>Место рождения</returns>
        private static string InputBirthdayPlace()
        {
            string message = "Введите место рождения сотрудника: ";
            return ParsingTextInput(message);
        }

        /// <summary>
        /// Ввод даты рождения сотрудника в формате ДД.ММ.ГГГГ
        /// </summary>
        /// <returns>Дата рождения</returns>
        private static DateTime InputBirthday()
        {
            string condition = "Введите дату рождения сотрудника в формате ДД.ММ.ГГГГ: ";
            DateTime employeeBirthday = ParsingDateInput(condition);

            return employeeBirthday;
        }

        /// <summary>
        /// Проверка ввода корректной даты
        /// </summary>
        /// <param name="textToUser">Текст запроса для пользователя</param>
        /// <returns></returns>
        private static DateTime ParsingDateInput(string textToUser)
        {
            DateTime dateInput;
            bool isValidDate = false;

            Console.Clear();
            do
            {
                Console.WriteLine(textToUser);
                string userInput = Console.ReadLine().ToString();
                isValidDate = DateTime.TryParse(userInput, out dateInput);

            } while (!isValidDate);

            return dateInput;
        }

        /// <summary>
        /// Ввод роста сотрудника от 50 до 250 см
        /// </summary>
        /// <returns>Рост сотрудника</returns>
        private static int InputHeight()
        {
            string message = "Введите рост сотрудника от 50 до 250 см";
            int employeeHeight;

            do
            {
                employeeHeight = ParsingDigitInput(message);
            } while ( !(50 <= employeeHeight && employeeHeight <= 250));

            return employeeHeight;
        }

        /// <summary>
        /// Ввод возраста сотрудника в диапазоне от 18 до 99
        /// </summary>
        /// <returns>Возраст сотрудника</returns>
        private static int InputAge()
        {
            string message = "Введите возраст сотрудника от 18 до 99 лет: ";
            int employeeAge;

            do
            {
                employeeAge = ParsingDigitInput(message);

            } while ( !(18 <= employeeAge && employeeAge <= 99));

            return employeeAge;            
        }

        /// <summary>
        /// Ввод ФИО сотрудника
        /// </summary>
        /// <returns>Введеное значние ФИО</returns>
        private static string InputFullName()
        {
            string message = "Введите Ф.И.О. сотрудника: ";
            return ParsingTextInput(message);
        }

        /// <summary>
        /// Проверка что введен только текст с пробелами
        /// </summary>
        /// <param name="messageToUser">Сообщение пользователю</param>
        /// <returns>Текст пользовательского ввода</returns>
        private static string ParsingTextInput(string messageToUser)
        {
            string userInput;
            bool isValidText = false;

            Console.Clear();
            do
            {
                Console.WriteLine(messageToUser);
                userInput = Console.ReadLine().ToString();
                isValidText = userInput.All(c => Char.IsLetter(c) || c == ' ');

            } while (!isValidText);

            return userInput;
        }
    }
}