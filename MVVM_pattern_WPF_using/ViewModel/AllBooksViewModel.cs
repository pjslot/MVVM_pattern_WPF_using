using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MVVM_pattern_WPF_using.Model;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Xml.Serialization;


namespace MVVM_pattern_WPF_using.ViewModel
{
    public class AllBooksViewModel : INotifyPropertyChanged
    {
        //реализация интерфайса inotify
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        //поля класса
        private Book selectedBook;
        private BaseCommand addCommand;
        private BaseCommand delCommand;
        private BaseCommand saveCommand;
        private BaseCommand openCommand;

        //свойства класса
        public Book SelectedBook
        {
            get { return selectedBook; }
            set { selectedBook = value; OnPropertyChanged("SelectedBook"); }
        }
        public ObservableCollection<Book> Books { get; set; }

        //обработчик команды добавления книги
        public BaseCommand AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new BaseCommand(obj =>
                {
                    Book Book = new Book();
                    Books.Insert(0, Book);
                    SelectedBook = Book;
                }));
            }
        }

        //++++++подумать на счёт кнопки удаления+++++
        
        //обработчик команды удаления книги
        public BaseCommand DelCommand
        {
            get
            {
                if (delCommand != null)
                    return delCommand;
                else
                {
                    Action<object> Execute = o =>
                    {
                        Book b = (Book)o;
                        Books.Remove(b);
                    };
                    Func<object, bool> CanExecute = o => Books.Count > 0;
                    delCommand = new BaseCommand(Execute, CanExecute);
                    return delCommand;
                }
            }
        }

        //обработчик команды сохранения списка книг
        public BaseCommand SaveCommand
        {
            get
            {
                if (saveCommand != null)
                    return saveCommand;
                else
                {

                    Action<object> Execute = o =>
                    {

                        //сериализация XML
                        XElement x = new XElement("Books",
                        from book in Books
                        select new XElement("Book",

                                new XElement("Title", book.Title),
                                new XElement("Author", book.Author),
                                new XElement("Publisher", book.Publisher),
                                new XElement("Year", book.Year)

                                ));
                        string s = x.ToString();


                        Stream sr = new FileStream("books.xml", FileMode.Create);
                        XmlSerializer xmlSer = new XmlSerializer(typeof(string));
                        xmlSer.Serialize(sr, s);
                        sr.Close();


                    };

                    saveCommand = new BaseCommand(Execute);
                    return saveCommand;
                }

            }
        }




        //обработчик команды открытия списка книг
        public BaseCommand OpenCommand
        {
            get
            {
                if (saveCommand != null)
                    return saveCommand;
                else
                {

                    Action<object> Execute = o =>
                    {
                        //десереализация XML
                        Stream sr = new FileStream("books.xml", FileMode.Open);
                        XmlSerializer xmlSer = new XmlSerializer(typeof(string));
                        string s = (string)xmlSer.Deserialize(sr);
                        sr.Close();

                var x = XElement.Parse(s);
                var q = from e in x.Elements()
                select new Book(e.Element("Title").Value, e.Element("Author").Value);
 //!!!!!!!!!!                       //добавить конструктор!

                    };

                    openCommand = new BaseCommand(Execute);
                    return openCommand;
                }

            }
        }




        //конструктор
        public AllBooksViewModel()
        {
            Books = new ObservableCollection<Book>
            {
                new Book {Title="WPF Unleashed", Author="Adam Natan", Year="2012" , Publisher="Apub"},
                new Book {Title="F# for Machine Learning", Author="Sudipta Mukherjee", Year="2016", Publisher="Bpub" },
                new Book {Title="F# for Fun and Profit", Author="Scott Wlaschin", Year="2015", Publisher="Cpub" },
                new Book {Title="Learning C# by Developing Games with Unity 3D", Author="Terry Norton", Year="2013", Publisher="Dpub" }
            };
        }
    }
}
