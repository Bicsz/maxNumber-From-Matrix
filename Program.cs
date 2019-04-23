using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxNubmerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// Находит индек максимального числа в полученном листе, исключая те элементы, которые содержатся в пройденном пути
            /// </summary>
            /// <param name="neighbourhood_local">текущая точка</param>
            /// <param name="path_local">уже пройденный путь</param>
            /// <returns>индекс максимального элмента</returns>
            int findMax(List<ElementOfMatrix> neighbourhood_local,List<byte> path_local)
            {
                int IndexFoMaxValue = -1;
                int MaxValue = int.MinValue;
               
                foreach (ElementOfMatrix element_local in neighbourhood_local)
                {
                    if (path_local.FindIndex(x => x == element_local.Index) == -1)
                    {
                        if (element_local.Value > MaxValue)
                        {
                            MaxValue = element_local.Value;
                            IndexFoMaxValue = element_local.Index;
                        }
                    }
                }
                return IndexFoMaxValue;//есил вернет -1, значит идти некуда
            }
            List <ElementOfMatrix> ofMatrices = new List<ElementOfMatrix>();//основная структура данных 
            int j = 0;

            //разбираем строки на числа
            Console.WriteLine("Input your matrix:");
            ElementOfMatrix element;
            for (int i = 0; i < 3; i++)
                Console.ReadLine().Split().ToList().ForEach(x =>
                {
                    element = new ElementOfMatrix(int.Parse(x), (byte)ofMatrices.Count);
                    if (j > 0)
                    {

                        //запись, кто кому сосед
                        element.addNeighbourhood(ofMatrices[ofMatrices.Count - 1]);
                        ofMatrices[ofMatrices.Count - 1].addNeighbourhood(element);
                    }
                    if (i > 0)
                    {
                        //запись, кто кому сосед
                        element.addNeighbourhood(ofMatrices[ofMatrices.Count - 3]);
                        ofMatrices[ofMatrices.Count - 3].addNeighbourhood(element);
                    }

                    j = j < 2 ? j + 1 : 0;

                    ofMatrices.Add(element);
                });
            List<List<byte>> globalPath = new List<List<byte>>();

            ///Получает на вход текущую точку и уже пройденный путь.
            ///Расчитывает всевозможные пути из точки, основным путем считая большее большие числа из рядом стоящих
            List<byte> walker( ElementOfMatrix element_local, List<byte> path_local)
            {
                int index_local;
                while(true)
                {
                    index_local = findMax(element_local.getNeighbourhood(), path_local);//ищет приоритетную точку для следующего шага          
                    //Все другие соседние точки, если еще в них небыл и если эта точка, не приоритетная, и идет вызов этой же функции
                    ((List<ElementOfMatrix>)element_local.getNeighbourhood()).ForEach(x => {
                        if (index_local != x.Index  && path_local.FindIndex(y => y == x.Index) == -1)
                        {
                            List<byte> path_next_temp = new List<byte>();
                            path_next_temp.AddRange(path_local);//чтобы защитить путь от доступа использовано не прямое назаначение переменных, а через AddRange
                            path_next_temp.Add(x.Index);//доавление в созданный альтернативный путь точки
                            globalPath.Add(walker(x, path_next_temp));//старт процесса для альтернативной точки с альтернативным путем
                        }
                    });
                    if (index_local != -1)
                    {
                        //если путь этой точки еще не закончился, то оан делает шаг в приоритетную точку
                        path_local.Add((byte)index_local);
                        element_local = ofMatrices[(byte)index_local];
                    }
                    else
                        return path_local;//если путь точки зашел в тупик, то вернуть путь
                }
            }
            List<byte> DONT_IT = new List<byte>();
            bool I_DO_IT = false;
            int INDEX;
            List<byte> path_temp;
            List<long> maxs = new List<long>();

            while (!I_DO_IT)
            {
                INDEX = findMax(ofMatrices, DONT_IT);
                path_temp = new List<byte>();
                path_temp.Add((byte)INDEX);
                globalPath.Add(walker(ofMatrices[INDEX], path_temp));//MAIN PROCESS

                //нахождение самого "удачного" пути
                long max = long.MinValue;
                long number = 0;
                globalPath.ForEach(x =>
                {
                    number = 0;
                    for (int i = 0; i < x.Count; i++)
                    {
                        number += ofMatrices[x[i]].Value * (long)Math.Pow(10, x.Count - i - 1);//перевод индексов пути в Value
                    }
                    if (number > max)
                        max = number;
                });
                maxs.Add(max);

                //Если вдруг не существует путь, проходящий через все точки(маловерятно), то найти все пути, начиная с другого числа
                if (max >= 100000000)
                    I_DO_IT = true;
                else
                    DONT_IT.Add(globalPath[0][0]);
            }
            Console.Write(maxs.Max());//вывести самый большой путь из всех путей
            Console.ReadKey();
        }

        /// <summary>
        /// Элемент матрицы, который хранит в себе своих соседей, свой индекс в основном масиве и свое значение
        /// </summary>
        class ElementOfMatrix
        {
            List<ElementOfMatrix> neighbourhood;
            internal int Value { get; set; }
            internal byte Index { get; set; }

            internal ElementOfMatrix(int Value, byte Index)
            {
                this.Value = Value;
                this.Index = Index;
                neighbourhood = new List<ElementOfMatrix>();
            }

            internal void addNeighbourhood(ElementOfMatrix element)
            {
                neighbourhood.Add(element);
            }
            internal dynamic getNeighbourhood(int at = -1)
            {
                if (at < 0)
                    return neighbourhood;
                else
                    return neighbourhood[at];
            }
        }
    }
}
