using System.Text.Json;
using TaskTrackerCLI.Model;

namespace TaskTrackerCLI.Service
{
    public class RepositoryService : IRepositoryService
    {
        private readonly string _path = Path.Combine(Directory.GetCurrentDirectory(), "tasks.json");

        public RepositoryService()
        {
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, "[]");
            }
        }

        public void Save(MyTask task)
        {
            var tasks = GetAll();
            tasks.Add(task);
            SaveChanges(tasks);
        }

        public void Delete(string id)
        {
            string json = File.ReadAllText(_path);

            var items = JsonSerializer.Deserialize<List<MyTask>>(json);

            var itemToDelete = items.FirstOrDefault(x => x.Id == id);

            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
                SaveChanges(items);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён несуществующий ID.");
                Console.ResetColor();
            }
        }

        public List<MyTask> GetAll()
        {
            string json = File.ReadAllText(_path);

            var items = JsonSerializer.Deserialize<List<MyTask>>(json);

            return items ?? new List<MyTask>();
        }

        public void Update(string id, string description)
        {
            string json = File.ReadAllText(_path);

            var items = JsonSerializer.Deserialize<List<MyTask>>(json);

            var itemToUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Description = description;
                itemToUpdate.UpdatedAt = DateTime.Now;
                SaveChanges(items);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён несуществующий ID.");
                Console.ResetColor();
            }
        }

        public void SaveChanges(List<MyTask> myTasks)
        {
            File.WriteAllText(_path, JsonSerializer.Serialize(myTasks));
        }
    }
}