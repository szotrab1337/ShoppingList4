﻿namespace ShoppingList4.Maui.Interfaces
{
    public interface IMessageBoxService
    {
        Task ShowAlert(string title, string message, string cancel);
        Task<bool> ShowAlert(string title, string message, string accept, string cancel);
    }
}