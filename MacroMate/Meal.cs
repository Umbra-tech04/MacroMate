using System;
using System.Collections.Generic;
using System.Text;

namespace MacroMate
{
    public class Meal
    {
        public string type;
        public string description;
        public double calories;
        public double protein;
        public double fat;
        public double carb;

        public Meal(string type, string description, double calories, double protein, double fat, double carb)
        {
            this.type = type;
            this.description = description;
            this.calories = calories;
            this.protein = protein;
            this.fat = fat;
            this.carb = carb;
        }

    }
}
