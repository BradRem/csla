using System;
using System.Linq;
using Csla;
using Csla.Serialization;

namespace ProjectTracker.Library
{
  [Serializable()]
  public class ResourceAssignments : BusinessListBase<ResourceAssignments, ResourceAssignmentEdit>
  {
    public void AssignTo(int projectId)
    {
      if (!(Contains(projectId)))
      {
        var project = DataPortal.CreateChild<ResourceAssignmentEdit>(projectId);
        this.Add(project);
      }
      else
      {
        throw new InvalidOperationException("Resource already assigned to project");
      }
    }

    public void Remove(int projectId)
    {
      var item = (from r in this
                  where r.ProjectId == projectId
                  select r).FirstOrDefault();
      if (item != null)
        Remove(item);
    }

    public bool Contains(int projectId)
    {
      var count = (from r in this
                   where r.ProjectId == projectId
                   select r).Count();
      return count > 0;
    }

    public bool ContainsDeleted(int projectId)
    {
      var count = (from r in DeletedList
                   where r.ProjectId == projectId
                   select r).Count();
      return count > 0;
    }

  #if !SILVERLIGHT
    private void Child_Fetch(int resourceId)
    {
      using (var ctx = ProjectTracker.Dal.DalFactory.GetManager())
      {
        var dal = ctx.GetProvider<ProjectTracker.Dal.IAssignmentDal>();
        var data = dal.FetchForResource(resourceId);
        var rlce = RaiseListChangedEvents;
        RaiseListChangedEvents = false;
        foreach (var item in data)
          Add(DataPortal.FetchChild<ResourceAssignmentEdit>(item));
        RaiseListChangedEvents = rlce;
      }
    }
#endif
  }
}