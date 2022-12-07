using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseProvider
{
    /// <summary>
    /// Реализация Энумератора для структуры работник
    /// </summary>
    internal class WorkerEnumenator : IEnumerator<Worker>
    {
        Worker[] workers;
        int position;

        public WorkerEnumenator(Worker[] workers)
        {
            this.workers = workers;
        }

        public Worker Current
        {
            get
            {
                if (position == -1 || position >=workers.Length)
                {
                    throw new ArgumentException();
                }
                return workers[position];
            }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (position <= workers.Length)
            {
                position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
