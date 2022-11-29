using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace MVVM_pattern_WPF_using.Model
{
    public class Book : INotifyPropertyChanged
    {
        //реализация интерфайса inotify
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        //поля класса
        private string title;
        private string author;
        private string publisher;
        private string year;

        //свойства класса
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }
        public string Author
        {
            get { return author; }
            set { author = value; OnPropertyChanged("Author"); }
        }

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; OnPropertyChanged("Publisher"); }
        }

        public string Year
        {
            get { return year; }
            set { year = value; OnPropertyChanged("Year"); }
        }

        //конструкторы

        public Book() { }
        public Book(string Title, string Author, string Publisher, string Year)
        {
            this.title = Title;
            this.author = Author;
            this.publisher = Publisher;
            this.year = Year;
        }

    }
}
