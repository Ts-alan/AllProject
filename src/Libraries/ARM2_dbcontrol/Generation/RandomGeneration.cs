using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Generation
{
    /// <summary>
    /// Генератор случайных чисел
    /// </summary>
    public class RandomGenerator 
    { 
        private Random gen;
        private int num = 0;
        private int[] cache;

        public RandomGenerator(int Num)
        { 
            this.gen = new Random(unchecked((int)DateTime.Now.Ticks));
            this.num = Num; 
            this.cache = new int[Num];
        }

        public int[] GetArray(int left, int right) 
        { 
            for (int i = 0; i < this.num; i++) 
            { 
                this.cache[i] = this.gen.Next(left, right); 
            } 
            return this.cache; 
        }

        public int Get(int left, int right)
        {
            return this.gen.Next(left, right); 
        }

        public int Get(int num)
        {
            return this.gen.Next();
        }
    }
}
