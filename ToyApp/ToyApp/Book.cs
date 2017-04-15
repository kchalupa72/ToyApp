using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ToyApp
{
    public class Book
    {
        public enum Category
        {
            Fiction,
            ScienceFiction,
            Biography,
            Educational,
            Comics,
            Children,
            Sex
        }
        private string _Title;
        private string _Author;
        private int _Year;
        private double _Price;
        private _Pen;
        
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public Pen Pen;
        public Category section;

        //Constructor
        public Book(string title, string author, int year, double price, Pen pen, Category cat) {
            _Title = title;
            _Author = author;
            _Year = year;
            _Price = price;
            _Pen = pen;
            Category section = cat;
        }
    }
}
