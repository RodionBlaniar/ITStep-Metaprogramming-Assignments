using System;

class TodoItem
{
    private string _title;
    private bool _isDone;

    public string Title
    {
        get { return this._title; }
    }

    public bool IsDone
    {
        get { return this._isDone; }
    }

    public TodoItem(string title) : this(title, false)
    {
    }

    public TodoItem(string title, bool isDone)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");
        this._title = title;
        this._isDone = isDone;
    }

    public void MarkDone()
    {
        this._isDone = true;
    }

    public void MarkUndone()
    {
        this._isDone = false;
    }

    public bool TryRename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            return false;
        this._title = newTitle;
        return true;
    }
}

class Program
{
    static void Main()
    {
        TodoItem task1 = new TodoItem("Buy groceries");
        Console.WriteLine($"Task: {task1.Title}, Done: {task1.IsDone}");

        task1.MarkDone();
        Console.WriteLine($"After MarkDone: {task1.Title}, Done: {task1.IsDone}");

        task1.MarkUndone();
        Console.WriteLine($"After MarkUndone: {task1.Title}, Done: {task1.IsDone}");

        bool renamed = task1.TryRename("Buy milk");
        Console.WriteLine($"Renamed: {renamed}, New title: {task1.Title}");

        renamed = task1.TryRename("");
        Console.WriteLine($"Rename to empty: {renamed}, Title: {task1.Title}");

        renamed = task1.TryRename("   ");
        Console.WriteLine($"Rename to spaces: {renamed}, Title: {task1.Title}");

        TodoItem task2 = new TodoItem("Finish homework", true);
        Console.WriteLine($"Task2: {task2.Title}, Done: {task2.IsDone}");

        try
        {
            TodoItem invalid = new TodoItem("");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Exception caught: {e.Message}");
        }

        try
        {
            TodoItem invalid = new TodoItem("   ");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Exception caught: {e.Message}");
        }
    }
}