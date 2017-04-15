using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Drawing;

namespace ToyApp
{
    public delegate void bookCollectionChanged(object sender, EventArgs e);

    public partial class Form1 : Form
    {
        //public event EventHandler XMLButtonClicked;
        public event bookCollectionChanged booksChanged;
        ObservableCollection<Book> bookList = new ObservableCollection<Book>();

        public Form1()
        {
            InitializeComponent();
            foreach (Book.Category cat in Enum.GetValues(typeof(Book.Category)))
            {
                comboBox1.Items.Add(cat);
            }
            Book.Category currentCategory = (Book.Category)Enum.Parse(typeof(Book.Category), comboBox1.SelectedValue.ToString());
            //populate the listbox with the current List<books>
            button1.Click += (sender, e) =>
            {
                Book newBook = new Book(textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), double.Parse(textBox4.Text), new Pen(Color.Blue), currentCategory);
                bookList.Add(newBook);
                booksChanged(sender, e);
            };
            booksChanged += new bookCollectionChanged(UpdateBooks);
            bookList.CollectionChanged += UpdateBooks;
            //Listener for all Textboxes
            var TextboxListener = new EventHandler(Txt_TextChanged);
            foreach (var ctrl in Controls) {
                if (ctrl is TextBox txtBox)
                    txtBox.TextChanged += TextboxListener;
            }
            button2.Click += new EventHandler(XMLCreator);
            button1.Click += new EventHandler(ClearTextboxes);
            listBox1.DisplayMember = "title";
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void UpdateBooks(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (Book b in bookList) {
                listBox1.Items.Add(b);
            }
            
            
        }
        private void Txt_TextChanged(object sender, EventArgs e)
        {
            

            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && textBox4.Text.Length > 0)
            {
                button1.Enabled = true;
            }

        }
        private void XMLCreator(object sender, EventArgs e)
        {

            if (bookList.Count > 0)
            {

                SaveFileDialog xmlSaveDialog = new SaveFileDialog()
                {
                    Filter = "xml files (*.xml)|*.xml",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t"
                };
                if (xmlSaveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = xmlSaveDialog.FileName;
                    { using (XmlWriter myWriter = XmlWriter.Create(filePath, settings))
                        {
                            myWriter.WriteStartDocument();
                            myWriter.WriteStartElement("BookList");
                            foreach (Book book in bookList)
                            {
                                myWriter.WriteStartElement("Book");
                                myWriter.WriteElementString("Book-", bookList.IndexOf(book).ToString());
                                myWriter.WriteElementString("Title", book.Title);
                                myWriter.WriteElementString("Author", book.Author);
                                myWriter.WriteElementString("Year", book.Year.ToString());
                                myWriter.WriteElementString("Price", book.Price.ToString());
                                myWriter.WriteElementString("PenARGB", book.Pen.Color.ToArgb().ToString());

                                myWriter.WriteEndElement();
                            }
                            myWriter.WriteEndDocument();
                            myWriter.Close();
                        }
                    }
                }
                
            }

        }
        private void ClearTextboxes(object sender, EventArgs e) {
            foreach (var ctrl in Controls)
            {
                if (ctrl is TextBox txtBox)
                    txtBox.Text = "";
            }
        }
   

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.' && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            List<Book> importBooks = new List<Book>();
            Stream mystream = null;
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                InitialDirectory = "C://Users/kenneth/OneDrive/Documents/Programming Documents/XML Project Test Area",
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true

            };
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                //try
                //{
                    if ((mystream = openDialog.OpenFile()) != null)
                    
                    {
                        using (mystream)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(openDialog.FileName);

                            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                            {
                                Book book = new Book(null,null,0,0,new Pen(Color.Red), Book.Category.Biography);
                                foreach (XmlNode attribute in node.ChildNodes)
                                {

                                    string text = attribute.InnerText; //or loop through its children as well
                                    switch (attribute.Name.ToString()) {
                                        case "1":
                                        case "Title":
                                            book.Title = attribute.InnerText;
                                            break;
                                        case "2":
                                        case "Author":
                                            book.Author = attribute.InnerText;
                                            break;
                                        case "3":
                                        case "Year":
                                            book.Year = int.Parse(attribute.InnerText);
                                            break;
                                        case "4":
                                        case "Price":
                                            book.Price = double.Parse(attribute.InnerText);
                                            break;
                                        case "5":
                                        case "PenARGB":
                                            book.Pen.Color = Color.FromArgb(int.Parse(attribute.InnerText));
                                            break;
                                        case "6":
                                        case "Book-":
                                            break;
                                        
                                    }
                                    
                                }
                                importBooks.Add(book);
                                //string text = node.InnerText; //or loop through its children as well
                                //MessageBox.Show(text);
                            }
                            foreach (Book book in importBooks)
                            {
                            bookList.Add(book);

                            }
                        }
                    }
            }
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                //}

            
        }
    }
}
