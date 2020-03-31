using System;
using System.IO;
using System.Collections.Generic;

namespace ConsoleTaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ToDo toDo = new ToDo();
            Reminder reminder = new Reminder();
            TaskManager taskManager = new TaskManager();

            taskManager.ToRemind("Reminders.txt");

            do
            {
                Console.WriteLine("\nToDo - create ToDo task \nRE - create remainder \nAllToDo - look all saved ToDo tasks" +
                    "\nAllRem - look all saved reminders \nNewToDo - look all new ToDo tasks " +
                    "\nNewRem - look all new reminders \n");
                switch (Console.ReadLine().ToString().ToLower())
                {
                    case "todo":
                        toDo.CreateTask(taskManager.toDoList);
                        break;
                    case "re":
                        reminder.CreateTask(taskManager.remList);
                        break;
                    case "alltodo":
                        taskManager.AllSavedTasks("ToDo.txt");
                        break;
                    case "allrem":
                        taskManager.AllSavedTasks("Reminders.txt");
                        break;
                    case "newtodo":
                        taskManager.AllNewToDoTasks();
                        taskManager.EditOrDeleteToDo();
                        break;
                    case "newrem":
                        taskManager.AllNewReminders();
                        taskManager.EditOrDeleteRem();
                        break;
                }
                Console.WriteLine("Click 'E' to close program, or other key to continue");
            }
            while (Console.ReadKey().Key.ToString().ToLower() != "e");

            Console.WriteLine("Save tasks to file?  Click 'Y' to save");
            if (Console.ReadKey().Key.ToString().ToLower() == "y")
            {
                taskManager.AllToDoToFile("ToDo.txt");
                taskManager.AllRemToFile("Reminders.txt");
            }
        }
    }

    class Task
    {
        private string title;

        public void DeleteTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to delete it");
            title = Console.ReadLine();
            taskList.Remove(title);
            Console.WriteLine("Your task was deleted");
        }
    }

    class ToDo : Task
    {
        private string title;
        private string description;

        public void CreateTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Title:");
            title = Console.ReadLine();
            while (taskList.ContainsKey(title))
            {
                Console.WriteLine("This title already exists, choose another");
                title = Console.ReadLine();
            }
            Console.WriteLine("Description:");
            description = Console.ReadLine();
            Console.WriteLine("Title: " + title + "\nDescription: " + description);
            taskList.Add(title, description);
        }

        public void EditTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to edit it");
            title = Console.ReadLine();
            string task = "";
            description = "";
            taskList.TryGetValue(title, out task);
            Console.WriteLine("Your task:" + task);
            Console.WriteLine("Your new task:");
            description = Console.ReadLine();
            taskList[title] = description;
            Console.WriteLine("Edited task: " + title + " " + description);
        }
    }

    class Reminder : Task
    {
        private string title;
        private string description;
        private DateTime deadline;

        public void CreateTask(Dictionary<string, string> remList)
        {
            Console.WriteLine("Title:");
            title = Console.ReadLine();
            while (remList.ContainsKey(title))
            {
                Console.WriteLine("This title already exists, choose another");
                title = Console.ReadLine();
            }
            Console.WriteLine("Description:");
            description = Console.ReadLine();
            Console.WriteLine("Deadline:");
            if (DateTime.TryParse(Console.ReadLine(), out deadline)) Console.WriteLine("\nDeadline: " + deadline);
            else Console.WriteLine("You have entered an incorrect value.");
            remList.Add(title, description + "| Deadline: |" + deadline);
        }

        public void EditTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to edit it");
            title = Console.ReadLine();
            string task = "";
            string newtask = "";
            taskList.TryGetValue(title, out task);
            Console.WriteLine("Your task:" + task);
            Console.WriteLine("Your new task:");
            newtask = Console.ReadLine();
            Console.WriteLine("Your new deadline:");
            if (DateTime.TryParse(Console.ReadLine(), out deadline)) Console.WriteLine("\nDeadline: " + deadline);
            else Console.WriteLine("You have entered an incorrect value.");
            taskList[title] = newtask + "\nDeadline: " + deadline;
            Console.WriteLine("Edited task: " + title + " " + newtask + "\nDeadline: " + deadline);
        }
    }

    class TaskManager
    {
        public Dictionary<string, string> toDoList = new Dictionary<string, string>();
        public Dictionary<string, string> remList = new Dictionary<string, string>();

        public void EditOrDeleteToDo()
        {
            ToDo toDo = new ToDo();
            Console.WriteLine("Click 'E' to edid any task, 'D' to delete");
            string comm1 = Console.ReadKey().Key.ToString().ToLower();
            if (comm1 == "e") toDo.EditTask(toDoList);
            else if (comm1 == "d") toDo.DeleteTask(toDoList);
        }

        public void EditOrDeleteRem()
        {
            Reminder rem = new Reminder();
            Console.WriteLine("Click 'E' to edid any task, 'D' to delete");
            string comm1 = Console.ReadKey().Key.ToString().ToLower();
            if (comm1 == "e") rem.EditTask(toDoList);
            else if (comm1 == "d") rem.DeleteTask(toDoList);
        }

        public void AllSavedTasks(string fileName)
        {
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            string content;
            if (File.Exists(path)) content = File.ReadAllText(path);
            else content = "You have no saved tasks";
            Console.WriteLine(content);
        }

        public void AllNewToDoTasks()
        {
            foreach (KeyValuePair<string, string> kvp in toDoList)
            {
                Console.WriteLine("Title: {0}, Description: {1}", kvp.Key, kvp.Value);
            }
        }

        public void AllNewReminders()
        {
            foreach (KeyValuePair<string, string> kvp in remList)
            {
                Console.WriteLine("Title: {0}, Description: {1}", kvp.Key, kvp.Value);
            }
        }

        public void AllToDoToFile(string fileName)
        {
            foreach (KeyValuePair<string, string> kvp in toDoList)
            {
                string title = kvp.Key;
                string description = kvp.Value;
                string currdir = Directory.GetCurrentDirectory();
                string path = Path.Combine(currdir, fileName);
                string task = "\nTitle: " + title + "| Description: |" + description;
                File.AppendAllText(path, task);
            }
        }

        public void AllRemToFile(string fileName)
        {
            foreach (KeyValuePair<string, string> kvp in remList)
            {
                string title = kvp.Key;
                string description = kvp.Value;
                string currdir = Directory.GetCurrentDirectory();
                string path = Path.Combine(currdir, fileName);
                string task = "\nTitle: " + title + "| Description: |" + description;
                File.AppendAllText(path, task);
            }
        }

        public void ToRemind(string fileName)
        {
            DateTime today = DateTime.Now.Date;
            DateTime deadline;
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            List<string> list = new List<string>();
            int index = 0;
            if (File.Exists(path))
            {
                foreach (string line in File.ReadLines(path))
                {
                    if (line.Contains("Title: ") & line.Contains("| Description: |") & line.Contains("| Deadline: |"))
                    {
                        list.Add(line.Substring(line.IndexOf("| Deadline: |") + "| Deadline: |".Length, 10)); // get deadline
                        list.Add(line);
                    }
                }
                foreach (string task in list)
                {
                    if (DateTime.TryParse(task, out deadline))
                    {
                        if (DateTime.Compare(deadline, today) == 0)
                        {
                            index = list.IndexOf(task, index);
                            Console.WriteLine("Your tasks for today:");
                            Console.WriteLine(list[index + 1]);
                        }
                    }
                }
            }
        }
    }
}
