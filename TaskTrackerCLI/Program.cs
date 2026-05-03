using TaskTrackerCLI.Service;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔══════════════════════════════╗");
Console.WriteLine("║      TASK TRACKER CLI        ║");
Console.WriteLine("╚══════════════════════════════╝");
Console.ResetColor();

IRepositoryService repositoryService = new RepositoryService();
TaskService taskService = new TaskService(repositoryService);

bool flag = true;
while (flag)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\nДоступные команды:");
    Console.ResetColor();

    Console.WriteLine("  add \"описание\"");
    Console.WriteLine("  update <id> \"новое описание\"");
    Console.WriteLine("  delete <id>");
    Console.WriteLine("  mark-in-progress <id>");
    Console.WriteLine("  mark-done <id>");
    Console.WriteLine("  list");
    Console.WriteLine("  list done | todo | in-progress");
    Console.WriteLine("  exit");

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("\n> ");
    Console.ResetColor();

    string? input = Console.ReadLine().ToLower();

    var parts = input.Split(' ', 3);

    int inputLength = parts.Length;

    var command = parts[0];
    var arg1 = parts.Length > 1 ? parts[1] : "";
    var arg2 = parts.Length > 2 ? parts[2] : "";

    var commands = new[]
    {
        "add",
        "update",
        "delete",
        "mark-in-progress",
        "mark-done",
        "list",
        "list done",
        "list todo",
        "list in-progress",
        "exit"
    };

    switch (command)
    {
        case "add":
            taskService.AddTask(arg1);
            break;

        case "update":
            if (inputLength >= 3)
            {
                taskService.UpdateTask(arg1, arg2);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный ввод. Повторите попытку.");
                Console.ResetColor();
            }
                break;

        case "delete":
            taskService.DeleteTask(arg1);
            break;

        case "mark-in-progress":
            taskService.MarkInProgressTask(arg1);
            break;

        case "mark-done":
            taskService.MarkDoneTask(arg1);
            break;

        case "list":

            switch (arg1)
            {
                case "":
                    taskService.ListOfTasks();
                    break;
                case "done":
                    taskService.ListOfTasks("done");
                    break;
                case "todo":
                    taskService.ListOfTasks("todo");
                    break;
                case "in-progress":
                    taskService.ListOfTasks("in-progress");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введена неверная команда");
                    Console.ResetColor();
                    break;
            }
            break;

        case "exit":
            flag = false;
            break;

        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Введена неверная команда. Попробуйте попытку снова.");
            Console.ResetColor();
            break;
    }
}