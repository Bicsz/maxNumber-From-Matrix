using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxNubmerGenerator
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<byte> path;


            /// <summary>
            /// Находит индек максимального числа в полученном листе, исключая те элементы, которые содержатся в пройденном пути
            /// </summary>
            /// <param name="arr">масив</param>
            /// <returns>индекс максимального элмента</returns>
            int findMax(List<ElementOfMatrix> neighbourhood_local)
            {
                int IndexFoMaxValue = -1;
                int MaxValue = int.MinValue;

                
                foreach (ElementOfMatrix element_local in neighbourhood_local)
                {
                    if(path.FindIndex(x=>x==element_local.Index)==-1)
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


            List<ElementOfMatrix> ofMatrices = new List<ElementOfMatrix>();//основная структура данных 
            int j = 0;


            //разбираем строки на числа
            Console.WriteLine("Input your matrix:");
            ElementOfMatrix element;
            for (int i=0;i<3;i++)
                Console.ReadLine().Split().ToList().ForEach(x=> {
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

                    j = j < 2 ? j+1 : 0;

                    ofMatrices.Add(element);
                });

            path = new List<byte>();


            int nexIndex = findMax(ofMatrices);//ищем самое максимальное значение чтобы с него начать
            if (nexIndex != -1)
                path.Add((byte)nexIndex);
            else
            {
                Console.WriteLine("it`s not a normal matrix!");
                return;
            }

            
            //генерируем путь, идя по соседям, выбирая того соседа, у которого Value больше
            while (true)
            {
                nexIndex = findMax(ofMatrices[path.Last()].getNeighbourhood());
                if(nexIndex!=-1)
                    path.Add((byte)nexIndex);
                else
                {
                    break;
                }
            }


            bool I_DO_IT = false;


            //если были сипользованы не все элементы матрицы, то следовательно было получено не самое большое число, поэтому ищем другой путь
            if (path.Count<9)
            {
                List<byte> DONT_IT = new List<byte>();//точки максимума матрицы, с которых нельзя начинать поиск пиути
                while (!I_DO_IT)
                {
                    //Console.WriteLine("Bed choise...");

                    DONT_IT.Add(path[0]);
                    path = new List<byte>();
                    path = DONT_IT;

                    nexIndex = findMax(ofMatrices);//иметируем, будто бы мы там уже были, чтобы код вернул точку, не из тех, в которых быть нельзя
                    path = new List<byte>();

                    if (nexIndex != -1)
                        path.Add((byte)nexIndex);


                    //генерируем путь, идя по соседям, выбирая того соседа, у которого Value больше
                    while (true)
                    {
                        nexIndex = findMax(ofMatrices[path.Last()].getNeighbourhood());
                        if (nexIndex != -1)
                            path.Add((byte)nexIndex);
                        else
                        {
                            break;
                        }
                    }

                    //если получилось найти путь, то выходим
                    if (path.Count >= 9)
                        I_DO_IT = true;
                }
            }

            path.ForEach(x=> {
                Console.Write(ofMatrices[x].Value);
            });
            Console.ReadKey();
            //Console.WriteLine("END");

        }


        /// <summary>
        /// Элемент матрицы, который хранит в себе своих соседей, свой индекс в основном масиве и свое значение
        /// </summary>
        class ElementOfMatrix
        {
            List<ElementOfMatrix> neighbourhood;
            internal int Value { get; set; }
            internal byte Index { get; set; }

            internal ElementOfMatrix(int Value,byte Index)
            {
                this.Value = Value;
                this.Index = Index;
                neighbourhood = new List<ElementOfMatrix>();
            }

            internal void addNeighbourhood(ElementOfMatrix element)
            {
                neighbourhood.Add(element);
            }
            internal dynamic getNeighbourhood(int at=-1)
            {
                if (at < 0)
                    return neighbourhood;
                else
                    return neighbourhood[at];
            }
        }

        
    }
}
