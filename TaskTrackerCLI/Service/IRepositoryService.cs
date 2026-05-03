using TaskTrackerCLI.Model;

namespace TaskTrackerCLI.Service
{
    public interface IRepositoryService
    {
        void Delete(string id);
        List<MyTask> GetAll();
        void Save(MyTask task);
        void SaveChanges(List<MyTask> myTasks);
        void Update(string id, string description);
    }
}