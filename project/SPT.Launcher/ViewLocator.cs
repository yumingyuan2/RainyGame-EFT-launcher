#nullable enable

using SPT.Launcher.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace SPT.Launcher
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data == null)
                return new TextBlock { Text = "Data is null" };

            string name = data.GetType().FullName!.Replace("ViewModel", "View");
            Type? type = Type.GetType(name);

            if (type != null)
            {
                try
                {
                    return (Control?)Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                    return new TextBlock { Text = $"Failed to create view: {ex.Message}" };
                }
            }
            else
            {
                return new TextBlock { Text = $"Not Found: {name}" };
            }
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
