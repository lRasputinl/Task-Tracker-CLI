using TaskTrackerCLI.Model;

namespace TaskTrackerCLI.Service
{
    public class TaskService(IRepositoryService repositoryService)
    {
        public void AddTask(string description)
        {
            var tasks = repositoryService.GetAll();

            if (!string.IsNullOrWhiteSpace(description))
            {
                int maxId = (tasks.Count != 0) ? tasks.Select(x => int.TryParse(x.Id, out int id) ? id : 0).Max() : 0;

                string id = (maxId + 1).ToString();

                var newTask = new MyTask
                {
                    Id = id,
                    Description = description,
                    Status = "todo",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                repositoryService.Save(newTask);
                WriteSuccess("Задача добавлена!");
            }
            else
            {
                WriteError("Описание задачи не может быть пустым.");
            }

        }

        public void UpdateTask(string id, string description)
        {
            var tasks = repositoryService.GetAll();

            if (!string.IsNullOrWhiteSpace(description) && (tasks.Any(x => x.Id == id)))
            {
                repositoryService.Update(id, description);
                WriteSuccess("Описание задачи изменено!");
            }
            else
            {
                WriteError("Введены некорректные параметры.");
            }
        }

        public void DeleteTask(string id)
        {
            var tasks = repositoryService.GetAll();

            if (tasks.Any(x => x.Id == id))
            {
                repositoryService.Delete(id);
                WriteSuccess("Задача удалена!");
            }
            else
            {
                WriteError("Введён несуществующий ID.");
            }
        }

        public void MarkInProgressTask(string id)
        {
            var tasks = repositoryService.GetAll();

            var task = tasks.FirstOrDefault(x => x.Id == id);

            if (task != null)
            {
                task.Status = "in-progress";
                task.UpdatedAt = DateTime.Now;
                repositoryService.SaveChanges(tasks);

                WriteSuccess("Статус задачи успешно изменён!");
            }
            else
            {
                WriteError("Введён несуществующий ID.");
            }
        }

        public void MarkDoneTask(string id)
        {
            var tasks = repositoryService.GetAll();

            var task = tasks.FirstOrDefault(x => x.Id == id);

            if (task != null)
            {
                task.Status = "done";
                task.UpdatedAt = DateTime.Now;
                repositoryService.SaveChanges(tasks);

                WriteSuccess("Статус задачи успешно изменён!");
            }
            else
            {
                WriteError("Введён несуществующий ID.");
            }
        }

        public void ListOfTasks(string? status = null)
        {
            var tasks = repositoryService.GetAll();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("──────────────────────────────");
            Console.WriteLine($"      Список {status ?? "всех"} задач:");
            Console.WriteLine("──────────────────────────────");
            Console.ResetColor();

            var filtered = string.IsNullOrWhiteSpace(status) 
                ? tasks 
                : tasks.Where(x => x.Status == status).ToList();

            if (filtered.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                foreach (var task in filtered)
                {
                    Console.ForegroundColor = task.Status switch
                    {
                        "done" => ConsoleColor.Green,
                        "in-progress" => ConsoleColor.DarkYellow,
                        _ => ConsoleColor.Gray
                    };
                    Console.WriteLine($"[{task.Id}] {task.Description} - {task.Status}");
                    Console.ResetColor();
                }
            }
        }

        public void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}