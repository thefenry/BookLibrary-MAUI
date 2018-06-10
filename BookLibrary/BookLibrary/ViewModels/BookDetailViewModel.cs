﻿using BookLibrary.Models;
using BookLibrary.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BookLibrary.ViewModels
{
    public class BookDetailViewModel : BaseViewModel
    {
        Book book;
        public Book Book
        {
            get { return book; }
            set {
                book = value;
                RaisePropertyChanged("Book"); }
        }


        public BookDetailViewModel(Book book = null)
        {
            Book = book;

            MessagingCenter.Subscribe<AddBook, Book>(this, "AddOrUpdateBook", (obj, bookUpdate) =>
            {
                if (bookUpdate != null)
                {
                    Book = bookUpdate as Book;
                }
            });
        }
    }
}
