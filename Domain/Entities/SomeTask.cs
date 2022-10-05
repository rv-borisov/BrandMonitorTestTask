using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SomeTask
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Statuses Status { get; set; }

        public string GetStatusName()
        {
            return Status switch
            {
                Statuses.Created => "created",
                Statuses.Running => "running",
                Statuses.Finished => "finished",
                _ => "Not defined status",
            };
        }

        public void Running()
        {
            Status = Statuses.Running;
            DateTime = DateTime.Now;
        }

        public void Finish()
        {
            Status = Statuses.Finished;
            DateTime = DateTime.Now;
        }

        public enum Statuses
        {
            Created,
            Running,
            Finished
        }
    }
}
