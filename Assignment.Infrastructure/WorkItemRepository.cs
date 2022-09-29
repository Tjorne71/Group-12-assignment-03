namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int WorkItemId) Create(WorkItemCreateDTO workItem)
    {
        var entity = _context.WorkItems.FirstOrDefault(t => t.Title == workItem.Title);
        Response response;
        
        if (entity is null)
        {
            entity = new WorkItem(workItem.Title);

            _context.WorkItems.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        }
        else 
        {
            response =  Response.Conflict;
        }

        return (response, entity.Id);
    }

    public Response Delete(int workItemId)
    {
        var workItem = _context.WorkItems.FirstOrDefault(t => t.Id == workItemId);
        Response response;

        if (workItem is null) 
        {
            response = Response.NotFound;
        }
        else if (workItem.State == State.Active)
        {
            workItem.State = State.Removed;
            _context.SaveChanges();
            response = Response.Updated;
        }
        else if (workItem.State == State.New)
        {
            _context.WorkItems.Remove(workItem);
            _context.SaveChanges();
            response = Response.Deleted;
            
        }
        else
        {
            response = Response.Conflict;
        }

        return response;
    }

    public WorkItemDetailsDTO? Find(int workItemId)
    {
        var workItems = from c in _context.WorkItems
                     where c.Id == workItemId
                     select new WorkItemDetailsDTO(c.Id, c.Title,c.Description, c.Created, c.AssignedTo.Name, c.Tags as IReadOnlyCollection<String>, c.State, c.Updated);

        return workItems.FirstOrDefault();
    }

    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        var tasks = 
            from w in _context.WorkItems
            select new WorkItemDTO(w.Id, w.Title, w.AssignedTo!.Name, w.Tags.Select(w => w.Name).ToList(), w.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(State state)
    {
        var tasks = 
            from w in _context.WorkItems
            where w.State == state
            select new WorkItemDTO(w.Id, w.Title, w.AssignedTo!.Name, w.Tags.Select(t => t.Name).ToList(), w.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag)
    {
        var specificTag = _context.Tags.Find(tag);
        var tasks = 
            from w in _context.WorkItems
            where w.Tags == specificTag
            select new WorkItemDTO(w.Id, w.Title, w.AssignedTo!.Name, w.Tags.Select(w => w.Name).ToList(), w.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        var specificUser = _context.Users.Find(userId);
        var tasks = 
            from w in _context.WorkItems
            where w.AssignedTo == specificUser
            select new WorkItemDTO(w.Id, w.Title, w.AssignedTo!.Name, w.Tags.Select(w => w.Name).ToList(), w.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadRemoved()
    {
        var workItems = 
            from w in _context.WorkItems
            where w.State == State.Removed
            select new WorkItemDTO(w.Id, w.Title, w.AssignedTo!.Name, w.Tags.Select(t => t.Name).ToList(), w.State);

        return workItems.ToArray();
    }

    public Response Update(WorkItemUpdateDTO workItem)
    {
        Response response;
        var entity = _context.WorkItems.Find(workItem.Id);

        if(workItem.AssignedToId != null && _context.Users.Find(workItem.AssignedToId) == null) {
            response = BadRequest;
            return response;
        }

        // Get all tags from task
        var allTags = new List<Tag> {};
        if(workItem.Tags != null) {
            foreach(string s in workItem.Tags) {
                int tagId = int.Parse(s);
                var tag = _context.Tags.Find(tagId);
                if (tag != null) allTags.Add(tag);
            }
        }

        if (entity is null) 
        {
            response = Response.NotFound;
        }
        else if (_context.WorkItems.FirstOrDefault(t => t.Id != workItem.Id && t.Title == workItem.Title) != null)
        {
            response = Response.Conflict;
        }
        else 
        {
            entity.Title = workItem.Title;
            entity.Tags = allTags;
            if (workItem.State != entity.State) {  
                entity.Updated = DateTime.UtcNow;
            }
            entity.State = workItem.State;
            _context.SaveChanges();
            response = Response.Updated;
        }
        return response;
    }
}
