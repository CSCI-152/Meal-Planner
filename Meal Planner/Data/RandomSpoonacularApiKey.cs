using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Data
{
    public class RandomSpoonacularApiKey
    {
        private readonly List<string> keys = new() { "cc3a1f5670574b379c682e1e1ce052af", "20f9b3ba30284ef5b5d289898cd2add3", "30e8e9f781a54cde8962bd0b00c56f40", "89179b372e1d45769aeb387f287e6448", "1f203448802e4c62b7f0d74e99e2e141", "78a4e2c096324f96801e65e7bd507413" };
        private readonly Random rnd;

        public RandomSpoonacularApiKey()
        {
            rnd = new Random();
        }
        public string Generate()
        {
            return keys[rnd.Next(keys.Count)];
        }
    }
}
